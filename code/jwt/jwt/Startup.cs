using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace jwt
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
	        services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

			app.UseJwtBearerAuthentication(new JwtBearerOptions()
			{
				Audience = "Audience",
				AutomaticAuthenticate = true,
				TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is my key for authentication")),
					ValidateIssuer = true,
					ValidIssuer = "Issuer",
					ValidateAudience = true,
					ValidAudience = "Audience",
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
				}
			});

			if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

	        app.UseMvc();
        }
    }
}
