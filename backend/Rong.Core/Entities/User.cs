using System.ComponentModel.DataAnnotations;
using Rong.Core.Enums;
using SqlSugar;

namespace Rong.Core.Entities;

[SugarTable("t_user", TableDescription = "用户表")]
public class User: Entity
{
    [Required]
    [SugarColumn(ColumnName = "user_name", Length = 64)]
    public string UserName { get; set; } = "";

    [Required]
    [SugarColumn(ColumnName = "password", Length = 256)]
    public string Password { get; set; } = "";
    [SugarColumn(ColumnName = "status")]
    public Status Status { get; set; }
}