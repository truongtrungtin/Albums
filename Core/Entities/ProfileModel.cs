﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Core.Entities;

[Table("ProfileModel", Schema = "Customer")]
public partial class ProfileModel
{
    [Key]
    public Guid ProfileId { get; set; }

    public int ProfileCode { get; set; }

    [StringLength(200)]
    public string FirstName { get; set; }

    [StringLength(200)]
    public string LastName { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreateTime { get; set; }

    public Guid? CreateBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? LastEditTime { get; set; }

    public Guid? LastEditBy { get; set; }

    [StringLength(500)]
    public string Email { get; set; }

    public string Token { get; set; }

    [StringLength(100)]
    public string Password { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Phone { get; set; }

    public bool? Actived { get; set; }

    public string Avatar { get; set; }
}