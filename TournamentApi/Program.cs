using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;
using TournamentApi.Data;
using TournamentApi.Extensions;
using TournamentApi.Profiles;
using TournamentApi.Services;
using TournamentApi.Services.Interfaces;
using TournamentApi.Validators.Games;

namespace TournamentApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -----------------------------
            // Rate Limiting
            // -----------------------------
            builder.Services.AddRateLimiter(options =>
            {
                options.AddPolicy("PostLimit", httpContext =>
                    RateLimitPartition.GetSlidingWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                        factory: _ => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(1),
                            SegmentsPerWindow = 1,
                            QueueLimit = 0,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        }
                    )
                );
            });

            // -----------------------------
            // JWT Authentication
            // -----------------------------
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                        ValidAudience = builder.Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                    };
                });

            // -----------------------------
            // FluentValidation
            // -----------------------------
            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<PatchGameRequestValidator>();

            // -----------------------------
            // Controllers + JSON Patch
            // -----------------------------
            builder.Services.AddControllers();


            // -----------------------------
            // EF Core
            // -----------------------------
            builder.Services.AddDbContext<TournamentDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // -----------------------------
            // Services
            // -----------------------------
            builder.Services.AddScoped<ITournamentService, TournamentService>();
            builder.Services.AddScoped<IGameService, GameService>();

            // -----------------------------
            // AutoMapper
            // -----------------------------
            builder.Services.AddAutoMapper(typeof(TournamentProfile));
            builder.Services.AddAutoMapper(typeof(GameProfile));

            // -----------------------------
            // Swagger
            // -----------------------------
            builder.Services.AddSwaggerWithJwt();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tournament API v1");
                    options.RoutePrefix = "swagger";
                });

            }

            app.UseHttpsRedirection();
            app.UseRateLimiter();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            // -----------------------------
            // SEED DATA 
            // -----------------------------

            DataSeeder.Seed(app);

            app.Run();
        }
    }
}
