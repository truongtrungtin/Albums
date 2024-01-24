﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[Table("WardModel", Schema = "MasterData")]
public partial class WardModel
{
    [Key]
    public Guid WardId { get; set; }

    [StringLength(50)]
    public string WardCode { get; set; }

    [StringLength(20)]
    public string Appellation { get; set; }

    [StringLength(200)]
    public string WardName { get; set; }

    public Guid? DistrictId { get; set; }

    public int? OrderIndex { get; set; }
}