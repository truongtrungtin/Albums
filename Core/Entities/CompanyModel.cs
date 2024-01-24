﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[Table("CompanyModel", Schema = "MasterData")]
public partial class CompanyModel
{
    [Key]
    public Guid CompanyId { get; set; }

    [StringLength(50)]
    public string CompanyCode { get; set; }

    [StringLength(100)]
    public string CompanyName { get; set; }

    [StringLength(100)]
    public string CompanyShortName { get; set; }

    [StringLength(50)]
    public string PhoneNumber { get; set; }

    [StringLength(50)]
    public string Hotline { get; set; }

    [StringLength(500)]
    public string Address { get; set; }

    [StringLength(100)]
    public string Logo { get; set; }

    public int? OrderIndex { get; set; }

    public bool? Actived { get; set; }

    [StringLength(50)]
    public string Fax { get; set; }

    [StringLength(50)]
    public string TaxNumber { get; set; }

    public Guid? WardId { get; set; }

    public Guid? DistrictId { get; set; }

    public Guid? ProvinceId { get; set; }

    [InverseProperty("Company")]
    public virtual ICollection<StoreModel> StoreModel { get; set; } = new List<StoreModel>();
}