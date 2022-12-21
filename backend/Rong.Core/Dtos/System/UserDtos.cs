using System.ComponentModel.DataAnnotations;

namespace Rong.Core.Dtos.System;

public record Register([Required]string userName, [Required]string password);

public record Login([Required]string userName, [Required]string password);