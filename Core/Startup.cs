using DataAccess.EFCore.AccidentModels;
using DataAccess.EFCore.RvpOfficeModels;
using DataAccess.EFCore.iPolicyModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VueCliMiddleware;
using DataAccess.EFCore.DigitalClaimModels;
using DataAccess.EFCore.ClaimDataModels;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Core.Mappers;
using System.IO;
using DinkToPdf.Contracts;
using DinkToPdf;
using System.Reflection;
using System.Runtime.Loader;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using DataAccess.EFCore.RvpSystemModels;
using Microsoft.AspNetCore.Http;

namespace Core
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAutoMapper(typeof(DataMapperProfile));
            services.AddAutoMapper(typeof(ServiceDataMapperProfile));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "core-client-app/dist";
            });
            services.AddDbContext<RvpaccidentContext>(o => o.UseSqlServer(Configuration.GetConnectionString("RvpAccident")));
            services.AddDbContext<RvpofficeContext>(o => o.UseSqlServer(Configuration.GetConnectionString("Rvpoffice")));
            services.AddDbContext<IpolicyContext>(o => o.UseSqlServer(Configuration.GetConnectionString("iPolicy")));
            services.AddDbContext<DigitalclaimContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DigitalClaim")));
            services.AddDbContext<ClaimDataContext>(o => o.UseSqlServer(Configuration.GetConnectionString("ClaimData")));
            services.AddDbContext<RvpSystemContext>(o => o.UseSqlServer(Configuration.GetConnectionString("RVPSystem")));
            services.AddTransient<IAccidentService, AccidentService>();
            services.AddScoped<IAccidentService, AccidentService>();
            services.AddTransient<IUserService, UserService>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<IMasterService, MasterService>();
            services.AddScoped<IMasterService, MasterService>();
            services.AddTransient<IApprovalService, ApprovalService>();
            services.AddScoped<IApprovalService, ApprovalService>();
            services.AddTransient<IAttachmentService, AttachmentService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddTransient<IWarmupService, WarmupService>();
            services.AddScoped<IWarmupService, WarmupService>();
            services.AddScoped<DigitalclaimContextProcedures>();



            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:key"]))
                };
            });


            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
            app.UseAuthentication();           
            app.UseRouting();
            app.UseSpaStaticFiles();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "My API V1");
            });

            app.UseSpa(spa =>
            {
                if (env.IsDevelopment())
                    spa.Options.SourcePath = "core-client-app/";
                else
                    spa.Options.SourcePath = "dist";

                if (env.IsDevelopment())
                {
                    spa.UseVueCli(npmScript: "serve");
                }

            });
        }
    }
}
