
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using SimpleWeb.NetCore.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace SimpleWeb.NetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors();
            services.AddDbContext<Models.ApplicationDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("AppDb")));
            services.AddDbContext<Models.ApplicationUserDbContext>(options => options.UseMySQL(Configuration.GetConnectionString("AppUserDb")));
            services.AddApiVersioning(
                options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                });

            services.AddSwaggerGen(options =>
            {
                //options.SwaggerDoc("v2.0", new Microsoft.OpenApi.Models.OpenApiInfo { Version = "v2.0", Title = "My API V2" });
                options.SwaggerDoc("v1.0", new Microsoft.OpenApi.Models.OpenApiInfo { Version = "v1.0", Title = "My API V1" });


                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var versions = apiDesc.CustomAttributes()
                        .OfType<Microsoft.AspNetCore.Mvc.ApiVersionAttribute>()
                        .SelectMany(attr => attr.Versions);

                    return versions.Any(v => $"v{v}" == docName);
                });

                options.OperationFilter<RemoveVersionParameterOperationFilter>();
                options.DocumentFilter<SetVersionInPathDocumentFilter>();


                var scheme = new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                };

                options.AddSecurityDefinition("Bearer", scheme);

                var requirement = new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme() {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                };

                options.AddSecurityRequirement(requirement);
            });

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<Models.ApplicationUserDbContext>().AddDefaultTokenProviders();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

 
            services.AddAuthorization(options =>
            {
                options.AddPolicy("SuperUserRoleOnly", policy =>
                    policy.Requirements.Add(new Auth.RoleRequirement(new string[] { "superuser" })));
            });


            services.AddScoped<IAuthorizationHandler, Auth.AuthorizationBaseOnRolesHandler>();
            services.AddScoped<Services.EmailService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //app.UseCors(options => 
                //     options.WithOrigins("http://localhost:5000")
                //    .AllowAnyMethod()
                //    .AllowAnyHeader()
                //    .SetIsOriginAllowed((host) => true)
                //    .AllowCredentials());

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    //c.SwaggerEndpoint("/swagger/v2.0/swagger.json", "My API V2");
                    c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "My API V1");

                    c.DocumentTitle = "zhihuzhuanlan API";
                });
            }


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
