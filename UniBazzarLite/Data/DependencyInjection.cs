namespace UniBazaarLite.Data
{
    // Helper class for registering our repositories with dependency injection
    public static class DependencyInjection
    {
        /// <summary>
        /// Call this in Program.cs with builder.Services.AddUniBazaarRepositories();
        /// </summary>
        public static IServiceCollection AddUniBazaarRepositories(this IServiceCollection services)
        {
            // Register our repositories as singletons (one instance for the whole app)
            services.AddSingleton<IEventRepository, InMemoryEventRepository>();
            services.AddSingleton<IItemRepository, InMemoryItemRepository>();
            return services;
        }
    }
}
