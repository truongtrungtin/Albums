﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[Table("ProductCategoryModel", Schema = "Warehouse")]
[Index("CategoryCode", Name = "UC_CategoryCode", IsUnique = true)]
public partial class ProductCategoryModel
{
    [Key]
    public Guid CategoryId { get; set; }

    public Guid? CategoryParentId { get; set; }

    [Required]
    [StringLength(50)]
    public string CategoryCode { get; set; }

    [StringLength(500)]
    public string CategoryName { get; set; }

    public bool? Active { get; set; }

    public string MenuImage { get; set; }

    public string CategoryPageImage { get; set; }

    public string ThumbnailImage { get; set; }

    public bool? IsShowInNavigationMenu { get; set; }

    public bool? IsShowInBrandDirectory { get; set; }

    public bool? IsBreakNavigationColumn { get; set; }

    public int? OrderIndex { get; set; }

    public string CustomNavigationURL { get; set; }

    [StringLength(50)]
    public string DefaultSort { get; set; }

    public string Descriptions { get; set; }

    public bool? IsDisplayDescriptions { get; set; }

    [StringLength(50)]
    public string MetaTitle { get; set; }

    [StringLength(500)]
    public string MetaKeywords { get; set; }

    public string MetaDescriptions { get; set; }

    public string GoogleTaxonomyId { get; set; }

    public bool? IsRestrictAccess { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateTime { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastEditTime { get; set; }

    public Guid? CreateBy { get; set; }

    public Guid? LastEditBy { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<ProductModel> ProductModel { get; set; } = new List<ProductModel>();
}