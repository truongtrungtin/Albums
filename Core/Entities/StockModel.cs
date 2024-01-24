﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[Table("StockModel", Schema = "Warehouse")]
public partial class StockModel
{
    [Key]
    public Guid StockId { get; set; }

    [StringLength(50)]
    public string StockCode { get; set; }

    [StringLength(500)]
    public string StockName { get; set; }

    [StringLength(500)]
    public string Address { get; set; }

    public Guid? ProvinceId { get; set; }

    public Guid? DistrictId { get; set; }

    public Guid? WardId { get; set; }

    public bool? Active { get; set; }
}