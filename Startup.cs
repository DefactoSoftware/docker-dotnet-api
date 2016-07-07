using System.IO;

using apicore.Models;
using apicore.Config;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;
using System.Linq;

namespace apicore
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment Env{ get; }
        
        public static void Main(string[] args)
        {
            var env = "Production";

            if (args.Contains("dev"))
                env = "Development";
            else if (args.Contains("staging"))
                env = "Staging";

            var host = new WebHostBuilder()
                .UseUrls("http://*:5000")
                .UseEnvironment(env)
                .UseKestrel(options => {
                    //options.??;
                })
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("customer/config/appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            Configuration = builder.Build();
            Env = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Add framework services.
            services.AddEntityFramework()
                .AddEntityFrameworkNpgsql();
                

            if (Env.IsDevelopment())
                services.AddDbContext<ApiContext>(options => options.UseNpgsql(Configuration["ConnectionString"]));
            else
                services.AddDbContext<ApiContext>(options => options.UseNpgsql(Configuration["Data:ConnectionString"]));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApiContext>()
                .AddDefaultTokenProviders();

            var mvcBuilder = services.AddMvc();
            mvcBuilder.AddJsonOptions(o => {
                o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                o.SerializerSettings.Converters.Add(new StringEnumConverter());
                o.SerializerSettings.Formatting = Formatting.Indented;
                o.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                o.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            });

            services.AddSwaggerGen(c => {
                c.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Docker DotnetCore API",
                    Description = "API Documentation for the Docker DotnetCore API",
                    TermsOfService = "((Terms of Service...))"
                });
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
                c.DescribeAllEnumsAsStrings();
                c.CustomSchemaIds(type => type.FriendlyId(true));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddConsole(LogLevel.Debug);
            }
            else
            {
                app.UseExceptionHandler();
                loggerFactory.AddConsole(LogLevel.Warning);
            }

            // Execute migrations on database
            using (var context = (ApiContext) app.ApplicationServices.GetService<ApiContext>())
                context.Database.Migrate();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUi();
        }
    }
}
