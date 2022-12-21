using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rong.Core.Entities;
using Rong.Infra.Helper;
using SqlSugar;

namespace Rong.Infra.Database;

public static class SugarContext
{
    public static void AddSqlSugarSetup(this IServiceCollection services)
    {
        // var settings = AppConfigurationHelper.GetSection<Core.Models.ShiFangSettings>("ShiFangSettings");
        var connectionString = AppConfigurationHelper.Configuration.GetConnectionString("default");
        var sugar = new SqlSugarScope(new ConnectionConfig()
        {
            ConnectionString = connectionString,
            DbType = DbType.MySql,
            IsAutoCloseConnection = true,
            InitKeyType = InitKeyType.Attribute,
            ConfigureExternalServices = new ConfigureExternalServices
            {
                EntityService = (c, p) =>
                {
                    // int?  decimal?这种 isNullable=true
                    if (c.PropertyType.IsGenericType &&
                        c.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        p.IsNullable = true;
                    }
                    else if (c.PropertyType == typeof(string) &&
                             c.GetCustomAttribute<RequiredAttribute>() == null)
                    { //string类型如果没有Required isNullable=true
                        p.IsNullable = true;
                    }
                }
            }
        });
        sugar.DbMaintenance.CreateDatabase();
        sugar.CodeFirst.InitTables<User>();
        sugar.Aop.OnLogExecuting = (sql, _) => { Console.WriteLine(sql + "\r"); };
        services.AddSingleton<ISqlSugarClient>(sugar);
    }
}