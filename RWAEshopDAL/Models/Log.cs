using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RWAEshopDAL.Models;

public partial class Log
{
    [Key]
    public int Id { get; set; }

    public string Message { get; set; } = null!;

    public int Level { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Timestamp { get; set; }
}
