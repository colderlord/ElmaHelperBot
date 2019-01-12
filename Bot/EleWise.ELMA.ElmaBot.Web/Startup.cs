using EleWise.ELMA.ElmaBot.Core.Handlers;
using EleWise.ELMA.ElmaBot.Core.Models;
using EleWise.ELMA.ElmaBot.Core.Rest;
using EleWise.ELMA.ElmaBot.Core.Rest.Services;
using EleWise.ELMA.ElmaBot.Core.Services;
using EleWise.ELMA.TelegramBot.Handlers;
using EleWise.ELMA.TelegramBot.Services;
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
            services.AddSingleton<IStartProcessService, StartProcessService>();
            services.AddSingleton<StartProcessService>();

            services.AddSingleton<ICommandService, CommandService>();
            services.AddSingleton<CommandService>();

            services.AddSingleton<IChatRepository, ChatRepository>();
            services.AddSingleton<ChatRepository>();

            services.AddSingleton<IAuthRepository, AuthRepository>();
            services.AddSingleton<AuthRepository>();

            services.AddSingleton<IStartProcessRepository, StartProcessRepository>();
            services.AddSingleton<StartProcessRepository>();

            services.AddSingleton<IProcessContextRepository, ProcessContextRepository>();
            services.AddSingleton<ProcessContextRepository>();

            services.AddSingleton<ICreateEventRepository, CreateEventRepository>();
            services.AddSingleton<CreateEventRepository>();

            services.AddSingleton<IBotWebApiService, BotWebApiService>();
            services.AddSingleton<BotWebApiService>();

            services.AddScoped<IUpdateService, UpdateService>();
            services.AddSingleton<IBotService, BotService>();
            services.AddSingleton<BotService>();
            services.AddSingleton<ICommand, StartCommand>();
            services.AddSingleton<StartCommand>();
            services.AddSingleton<ICommand, UnknownCommand>();
            services.AddSingleton<UnknownCommand>();
            services.AddSingleton<ICommand, AllCommands>();
            services.AddSingleton<AllCommands>();
            services.AddSingleton<ICommand, AuthorizationCommand>();
            services.AddSingleton<AuthorizationCommand>();
            services.AddSingleton<ICommand, StartProcessCommand>();
            services.AddSingleton<StartProcessCommand>();
            services.AddSingleton<ICommand, ResetCommand>();
            services.AddSingleton<ResetCommand>();
            services.AddSingleton<ICommand, EventCommand>();
            services.AddSingleton<EventCommand>();
            
        }
    }
}
