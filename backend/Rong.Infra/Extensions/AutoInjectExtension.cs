using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Rong.Core.Enums;
using Rong.Infra.Attributes;

namespace Rong.Infra.Extensions;

public static class AutoInjectExtension
{
    /// <summary>
    /// 自动注入所有的程序集有InjectAttribute标签
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <returns></returns>
    public static IServiceCollection AddAutoDi(this IServiceCollection serviceCollection)
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;
        var assemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFrom).ToList();
        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes().Where(a => a.GetCustomAttribute<AutoInjectAttribute>() != null)
                .ToList();
            if (types.Count <= 0) continue;
            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<AutoInjectAttribute>();
                if (attr?.Type == null) continue;
                switch (attr.InjectType)
                {
                    case InjectType.Scoped:
                        serviceCollection.AddScoped(attr.Type, type);
                        break;
                    case InjectType.Single:
                        serviceCollection.AddSingleton(attr.Type, type);
                        break;
                    case InjectType.Transient:
                        serviceCollection.AddTransient(attr.Type, type);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
  
        return serviceCollection;
    }
}