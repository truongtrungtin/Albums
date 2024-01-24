﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[Table("PageModel", Schema = "Pms")]
public partial class PageModel
{
    [Key]
    public Guid PageId { get; set; }

    [StringLength(100)]
    public string PageName { get; set; }

    [StringLength(300)]
    public string PageUrl { get; set; }

    [StringLength(100)]
    public string Parameter { get; set; }

    public Guid? MenuId { get; set; }

    [StringLength(100)]
    public string Icon { get; set; }

    public bool? IsSystem { get; set; }

    public bool? Active { get; set; }

    public int? OrderIndex { get; set; }

    [ForeignKey("MenuId")]
    [InverseProperty("PageModel")]
    public virtual MenuModel Menu { get; set; }

    [InverseProperty("Page")]
    public virtual ICollection<PagePermissionModel> PagePermissionModel { get; set; } = new List<PagePermissionModel>();

    [ForeignKey("PageId")]
    [InverseProperty("Page")]
    public virtual ICollection<FunctionModel> Function { get; set; } = new List<FunctionModel>();
}