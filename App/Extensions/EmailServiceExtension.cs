using Application.Options;
using Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Application.Extensions
{
    public static class EmailServiceExtension
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services,
            IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            services.Configure<EmailServiceOptions>(configuration.GetSection("Email"));
            if (hostingEnvironment.IsDevelopment() || hostingEnvironment.IsStaging())
            {
                services.AddSingleton<IEmailService, EmailService>();
            }
            else
            {
                services.AddSingleton<IEmailService, SendGridEmailService>();
            }
            return services;
        }
    }
}
