using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Voting.Core;
using Voting.Persistence.PostgreSQL;
using Voting.Services;

namespace Voting.API
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
            services.AddProblemDetails(opts =>
            {
                opts.IncludeExceptionDetails = (context, ex) =>
                {
                    var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
                    return environment.IsDevelopment() || environment.IsStaging();
                };
            });

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                    options.JsonSerializerOptions.DictionaryKeyPolicy = null;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                        .SetIsOriginAllowedToAllowWildcardSubdomains();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.AllowCredentials();
                });
            });

            services.AddDbContext<VotingDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("VotingDB"),
                    x => x.MigrationsAssembly("Voting.Persistence.PostgreSQL")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddCoreServices();
            services.AddAutoMapper(typeof(Startup));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseProblemDetails();
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseCors();

            using (var scope = app.ApplicationServices.CreateScope())
            using (var context = scope.ServiceProvider.GetService<VotingDbContext>())
            {
                if (context.Database.IsRelational())
                {
                    context.Database.Migrate();
                }
            }

            if (!env.IsProduction())
            {
                app.UseSwagger(x => x.SerializeAsV2 = true);
                app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Voting"); });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }
    }
}
