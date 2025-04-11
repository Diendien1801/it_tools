using it_tools.DataAccess.Repositories;
using it_tools.Presentation.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
namespace it_tools.BusinessLogic.Services
{
    public static class AppServices
    {
        public static IServiceProvider Services { get; private set; }

        public static void ConfigureServices()
        {
            var services = new ServiceCollection();

            // Đăng ký các lớp DI
            services.AddSingleton<IToolRepository, ToolRepository>();
            services.AddSingleton<IToolService, ToolService>();
            services.AddTransient<ToolPageViewModel>();
            services.AddSingleton<AuthViewModel>();
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IAuthRepository, AuthRepository>();

            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddTransient<AccountViewModel>();

            services.AddSingleton<IManagementRepository, ManagementRepository>();
            services.AddSingleton<IManagementService, ManageService>();
            services.AddTransient<ManagementViewModel>();

            services.AddSingleton<NavigationViewModel>();


            services.AddSingleton<HttpClient>();
            services.AddSingleton<IConfiguration>(sp =>
                new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build());

            Services = services.BuildServiceProvider();
        }
    }
}
