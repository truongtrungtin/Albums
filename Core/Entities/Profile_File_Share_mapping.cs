﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[Table("Profile_File_Share_mapping", Schema = "Customer")]
public partial class Profile_File_Share_mapping
{
    [Key]
    public Guid Profile_File_Share_Id { get; set; }

    public Guid? ProfileId { get; set; }

    public Guid? FileAttachmentId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateTime { get; set; }

    public Guid? CreateBy { get; set; }

    [StringLength(50)]
    public string Status { get; set; }

    public bool? Actived { get; set; }
}