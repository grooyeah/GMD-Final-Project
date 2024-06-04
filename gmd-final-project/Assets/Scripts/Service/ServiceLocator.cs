using System;
using System.Collections.Generic;

public class ServiceLocator
{
    private static readonly Lazy<ServiceLocator> lazy = new Lazy<ServiceLocator>(() => new ServiceLocator());
    public static ServiceLocator Instance => lazy.Value;

    private readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

    public void RegisterService<T>(T service) where T : class
    {
        var type = typeof(T);
        if (!services.ContainsKey(type))
        {
            services.Add(type, service);
        }
    }

    public T GetService<T>() where T : class
    {
        var type = typeof(T);
        if (services.ContainsKey(type))
        {
            return services[type] as T;
        }
        return null;
    }
}



