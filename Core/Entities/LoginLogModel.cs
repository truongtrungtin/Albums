﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[Table("LoginLogModel", Schema = "Pms")]
public partial class LoginLogModel
{
    [Key]
    public Guid Id { get; set; }

    [StringLength(100)]
    public string IPAddress { get; set; }

    [StringLength(100)]
    public string PortalAddress { get; set; }

    [StringLength(100)]
    public string Computer { get; set; }

    public Guid? AccountId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LoginTime { get; set; }
}