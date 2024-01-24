﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[PrimaryKey("ProductId", "FileAttachmentId")]
[Table("Product_Images_Mapping", Schema = "Warehouse")]
public partial class Product_Images_Mapping
{
    [Key]
    public Guid ProductId { get; set; }

    [Key]
    public Guid FileAttachmentId { get; set; }

    public bool? IsMain { get; set; }
}