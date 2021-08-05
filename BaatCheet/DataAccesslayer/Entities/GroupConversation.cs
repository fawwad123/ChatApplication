using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccessLayer.Entities
{
    [Table("GroupConversation")]
    public partial class GroupConversation
    {
        [Key]
        public int Id { get; set; }
        public int GroupId { get; set; }
        [Required]
        public string Message { get; set; }
        public int MessageBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime MessageOn { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(GroupId))]
        [InverseProperty("GroupConversations")]
        public virtual Group Group { get; set; }
        [ForeignKey(nameof(MessageBy))]
        [InverseProperty(nameof(User.GroupConversations))]
        public virtual User MessageByNavigation { get; set; }
    }
}
