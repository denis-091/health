using System;
using System.IO;
using System.Net;
using System.Text;
using health.Data;
using health.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace health
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

            services.AddSingleton<HealthService>();
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration["ConnectionStrings:Default"])
            );

            services.AddHealthChecks()
                .AddUrlGroup(new Uri("http://iswiftdata.1c-work.net/api/refdata/version"), "refdata", null, new[] { "refdata" })
                .AddUrlGroup(new Uri("http://ibonus.1c-work.net/api/ibonus/version"), "ibonus", null, new[] { "ibonus" })
                .AddCheck("catalog", () => {
                    try
                    {
                        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("http://iswiftdata.1c-work.net/api/catalog/catalog");
                        webRequest.Method = "GET";
                        webRequest.Headers.Add("AccessKey", "test_05fc5ed1-0199-4259-92a0-2cd58214b29c");
                        webRequest.ContentType = "application/json; charset=utf-8";

                        HttpWebResponse myHttpWebResponse = (HttpWebResponse)webRequest.GetResponse();

                        string statusResponse;
                        float durationResponce;
                        using (var reader = new StreamReader(myHttpWebResponse.GetResponseStream()))
                        {
                            var resource = JObject.Parse(reader.ReadToEnd());
                            statusResponse = resource.SelectToken("result.status").ToString();
                            durationResponce = float.Parse(resource.SelectToken("result.duration").ToString());
                        }

                        if (statusResponse != "0")
                        {
                            return HealthCheckResult.Unhealthy();
                        }

                        if (durationResponce > int.Parse(Configuration["HealthChecksUI:EvaluationTimeOnSeconds"]))
                        {
                            return HealthCheckResult.Degraded();
                        }

                        return HealthCheckResult.Healthy();
                    }
                    catch (Exception)
                    {
                        return HealthCheckResult.Unhealthy();
                    }
                })
                ;

            services.AddTransient<IDataHealthCheckExecutionEntries, DataRepository>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                //{
                //    Predicate = _ => true,
                //    ResponseWriter = async (context, report) =>
                //    {
                //        context.Response.ContentType = "application/json; charset=utf-8";
                //        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(report));
                //        await context.Response.Body.WriteAsync(bytes);
                //    }
                //});
            });
        }
    }
}
