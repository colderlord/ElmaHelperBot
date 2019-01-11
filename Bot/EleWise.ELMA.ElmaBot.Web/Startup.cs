using EleWise.ELMA.ElmaBot.Core.Models;
using EleWise.ELMA.ElmaBot.Core.Rest;
using EleWise.ELMA.ElmaBot.Core.Rest.Services;
using EleWise.ELMA.ElmaBot.Core.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EleWise.ELMA.ElmaBot.Web
{
    public sealed class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            RegisterServices(services);
            services.Configure<BotConfiguration>(Configuration.GetSection("BotConfiguration"));
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IRestClient, ELMARestClient>();
            services.AddSingleton<ELMARestClient>();
            services.AddSingleton<IAuthorizationService, AuthorizationService>();
            services.AddSingleton<AuthorizationService>();

            services.AddSingleton<ICommandService, CommandService>();
            services.AddSingleton<CommandService>();
        }
    }
}
