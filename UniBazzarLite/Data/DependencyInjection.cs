namespace UniBazzarLite.Data
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Program.cs içinde <c>builder.Services.AddUniBazaarRepositories();</c> diyerek çağır.
        /// </summary>
        public static IServiceCollection AddUniBazaarRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IEventRepository, InMemoryEventRepository>();
            services.AddSingleton<IItemRepository, InMemoryItemRepository>();
            return services;
        }
    }
}
