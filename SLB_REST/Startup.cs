﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SLB_REST.Context;
using SLB_REST.Helpers;
using SLB_REST.Helpers.ChangeDatabaseStrategy;
using SLB_REST.Helpers.GetDataBaseStrategy;
using SLB_REST.Helpers.Proxy;
using SLB_REST.Models;

namespace SLB_REST
{
	public class Startup
	{
		protected IConfigurationRoot Configuration;

		public Startup()
		{
			var configurationBuilder = new ConfigurationBuilder();
			configurationBuilder.AddXmlFile("appsettings.xml");
			Configuration = configurationBuilder.Build();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<EFContext>(builer => builer.UseSqlServer(Configuration["connectionString"]));
			
			services.AddTransient<SourceManagerEF>();
            services.AddTransient<DiscogsClientModel>();
            services.AddTransient<SourceManagerViewData>();
            services.AddTransient<SourceManagerDeleteAlbum>();
            services.AddTransient<SourceManagerSaveJson>();
            services.AddTransient<ProxyDiscogs>();
			services.AddTransient<ChangeDatabaseStrategy>();
            services.AddTransient<GetDataBaseStrategy>();


            services.AddIdentity<UserModel, IdentityRole<int>>().AddEntityFrameworkStores<EFContext>().AddDefaultTokenProviders();

			services.AddMvc();
		}

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseAuthentication();
			app.UseStaticFiles();
            app.UseMvc(routes =>{
                routes.MapRoute(
                    name: "default", template: "{controller=Account}/{action=Login}");
            });
		}
	}
}
