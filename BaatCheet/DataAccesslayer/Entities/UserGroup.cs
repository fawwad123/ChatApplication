using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccessLayer.Entities
{
    [Table("UserGroup")]
    public partial class UserGroup
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        [Column("isAdmin")]
        public bool? IsAdmin { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty("UserGroups")]
        public virtual Group Group { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserGroups")]
        public virtual User User { get; set; }
    }
}
