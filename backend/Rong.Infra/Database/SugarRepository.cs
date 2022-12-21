using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Rong.Infra.Helper;
using SqlSugar;

namespace Rong.Infra.Database;

public class SugarRepository<T>: SimpleClient<T> where T : class, new()
{
    public SugarRepository(ISqlSugarClient? context): base(context)
    {
        if (context != null) return;
        var connectionString = AppConfigurationHelper.Configuration.GetConnectionString("default");
        Context = new SqlSugarClient(new ConnectionConfig()
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
        Context.Aop.OnLogExecuting = (sql, _) => { Console.WriteLine(sql + "\r"); };
    }
}