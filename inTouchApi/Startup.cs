using inTouchApi.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using inTouchApi.Service;
using inTouchApi.Common;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using inTouchApi.Interface;
using Microsoft.AspNetCore.Http;

namespace inTouchApi
{
    public class Startup
    {

        readonly string MiCors = "MiCors";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDbContext<intouchContext>(options =>
                                                      options.UseMySQL(Configuration.GetConnectionString("SQLConn")));

            services.AddCors(options =>
            {
                options.AddPolicy(name: MiCors,
                                  builder =>
                                  {
                                      builder.WithHeaders("*");
                                      builder.WithOrigins("*");
                                      builder.WithMethods("*");
                                  });
            });


            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            //A PARTIR DE AQUI VIENE EL JWT JASON WEB TOKEN
            var appSettings = appSettingsSection.Get<AppSettings>();
            var llave = Encoding.ASCII.GetBytes(appSettings.secretKey);
            services.AddAuthentication(d =>
            {
                d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(d =>
            {
                d.RequireHttpsMetadata = false;
                d.SaveToken = true;
                d.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(llave),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });





            //services.AddScoped<IUserService, UserService>();
            services.AddTransient<UserInterface, UserService>();
            services.AddTransient<UrbanizationInterface, UrbanizationService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(MiCors);

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            //app.Run(async (contex) =>
            //{
            //    await contex.Response.WriteAsync("Could Not Find Anything");
            //});
        }
    }
}
