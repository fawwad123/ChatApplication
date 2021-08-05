using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccessLayer.Entities
{
    [Table("UserContact")]
    public partial class UserContact
    {
        public UserContact()
        {
            Conversations = new HashSet<Conversation>();
        }

        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ContactId { get; set; }
        public bool? IsBlocked { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

        [ForeignKey(nameof(ContactId))]
        [InverseProperty("UserContactContacts")]
        public virtual User Contact { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserContactUsers")]
        public virtual User User { get; set; }
        [InverseProperty(nameof(Conversation.UserContact))]
        public virtual ICollection<Conversation> Conversations { get; set; }
    }
}
