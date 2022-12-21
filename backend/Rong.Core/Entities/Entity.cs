using SqlSugar;

namespace Rong.Core.Entities;

public abstract class Entity
{
    [SugarColumn(ColumnName = "id", ColumnDescription = "ID", IsPrimaryKey = true, IsIdentity = true)]
    public long Id { get; set; }
    [SugarColumn(ColumnName = "create_time", ColumnDescription = "创建时间", IsNullable = false)]
    public long CreateTime { get; set; }
    [SugarColumn(ColumnName = "update_time", ColumnDescription = "更新时间", IsNullable = false)]
    public long UpdateTime { get; set; }
}