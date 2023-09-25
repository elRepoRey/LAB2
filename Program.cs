using Lab2.Data;
using Lab2.Interfaces;
using Lab2.Views.Shared;
using Lab2.Views;
using Microsoft.Extensions.DependencyInjection;
using Lab2.Models;
using Lab2;
using Lab2.Services;
using Lab2.Utils;

public class Program
{
    private static IServiceProvider _serviceProvider;

    public static void Main(string[] args)
    {
        RegisterServices();

        var currencyManager = (CurrencyServices)_serviceProvider.GetRequiredService<ICurrencyServices>();
        var storeConfig = _serviceProvider.GetRequiredService<IStoreConfig>();
        GlobalState.Initialize(currencyManager, storeConfig);

        var databaseInitializer = _serviceProvider.GetRequiredService<DatabaseInitializer>();
        databaseInitializer.Initialize();

        var layout = _serviceProvider.GetRequiredService<Layout>();
        layout.Render();

        DisposeServices();
    }

    private static void RegisterServices()
    {
        var collection = new ServiceCollection();

        // Register the StoreConfig with its file path
        collection.AddSingleton<IStoreConfig>(sp => new StoreConfig("StoreConfigData.json"));

        // Register the DatabaseServices with the paths from StoreConfig
        collection.AddTransient(sp =>
        {
            var storeConfig = sp.GetRequiredService<IStoreConfig>();
            return new DatabaseService<CustomerDAO>(storeConfig.FilePaths["Customers"]);
        });

        collection.AddTransient(sp =>
        {
            var storeConfig = sp.GetRequiredService<IStoreConfig>();
            return new DatabaseService<Product>(storeConfig.FilePaths["Products"]);
        });
        collection.AddSingleton(sp =>
        {
            var storeConfig = sp.GetRequiredService<IStoreConfig>();
            return new DatabaseService<ProductDAO>(storeConfig.FilePaths["Products"]);
        });

        collection.AddSingleton<ICurrencyServices, CurrencyServices>();
        collection.AddTransient<Settings>();
        collection.AddTransient<Layout>();
        collection.AddTransient<DatabaseInitializer>();
        collection.AddTransient<AuthFlow>();
        collection.AddTransient<CustomerServices>();
        collection.AddTransient<Store>();
        collection.AddTransient<CheckoutCart>();
        collection.AddTransient<Navbar>();
        collection.AddTransient<Sidebar>();
        collection.AddTransient<ProductService>();
        collection.AddTransient<Footer>();
        collection.AddTransient<Notification>();
        collection.AddSingleton<ProductService>();
        collection.AddSingleton<Cart>();



        _serviceProvider = collection.BuildServiceProvider();
    }

    private static void DisposeServices()
    {
        if (_serviceProvider == null) return;
        if (_serviceProvider is IDisposable disposable) disposable.Dispose();
    }
}
