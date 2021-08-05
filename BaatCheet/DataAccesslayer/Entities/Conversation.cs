using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccessLayer.Entities
{
    [Table("Conversation")]
    public partial class Conversation
    {
        [Key]
        public int Id { get; set; }
        public int UserContactId { get; set; }
        [Required]
        public string Message { get; set; }
        public int MessageBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime MessageOn { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(MessageBy))]
        [InverseProperty(nameof(User.Conversations))]
        public virtual User MessageByNavigation { get; set; }
        [ForeignKey(nameof(UserContactId))]
        [InverseProperty("Conversations")]
        public virtual UserContact UserContact { get; set; }
    }
}
