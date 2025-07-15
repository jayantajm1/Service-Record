using Microsoft.EntityFrameworkCore;
using Npgsql;
using Service_Record.DAL.Context;
using Service_Record.DAL.Enums;

namespace Service_Record.Extensions
{
    public static class PersistenceServiceExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Get your connection string from appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // 2. Create a new data source builder and provide the connection string
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

            // 3. Map all of your C# enums to the PostgreSQL enum types
            // Use the exact lowercase names from your database schema
            dataSourceBuilder.MapEnum<UserRole>("user_role");
            dataSourceBuilder.MapEnum<LogAction>("log_action");
            dataSourceBuilder.MapEnum<PartAction>("part_action");
            dataSourceBuilder.MapEnum<QuotationStatus>("quotation_status");
            dataSourceBuilder.MapEnum<ServiceStatus>("service_status");

            // 4. Build the data source
            var dataSource = dataSourceBuilder.Build();

            // 5. Register your DbContext to use the new data source
            services.AddDbContext<ServiceRecordDbContext>(options =>
            {
                options.UseNpgsql(dataSource);
            });

            // Return the services collection to allow for method chaining
            return services;
        }
    }
}
