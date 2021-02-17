using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Chat_Application.SocketsManger
{
    public static class SocketsExtenstion
    {
        public static IServiceCollection AddWebSocketManger(this IServiceCollection services)
        {
            services.AddTransient<ConnectionManager>();
            foreach(var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (type.GetTypeInfo().BaseType == typeof(SocketHandler))
                    services.AddSingleton(type);
            }
            return services;

        }
        public static IApplicationBuilder MapSockets(this IApplicationBuilder app, PathString path, SocketHandler socket)
        {
            return app.Map(path, (x) => x.UseMiddleware<SocketMiddleware>(socket));
        }
    }


}
