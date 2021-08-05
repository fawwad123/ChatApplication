using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccessLayer.Entities
{
    [Table("User")]
    public partial class User
    {
        public User()
        {
            Conversations = new HashSet<Conversation>();
            GroupConversations = new HashSet<GroupConversation>();
            GroupCreatedByNavigations = new HashSet<Group>();
            GroupModifiedByNavigations = new HashSet<Group>();
            UserContactContacts = new HashSet<UserContact>();
            UserContactUsers = new HashSet<UserContact>();
            UserGroups = new HashSet<UserGroup>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string Password { get; set; }
        public string Token { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedOn { get; set; }
        public string ImageUrl { get; set; }

        [InverseProperty(nameof(Conversation.MessageByNavigation))]
        public virtual ICollection<Conversation> Conversations { get; set; }
        [InverseProperty(nameof(GroupConversation.MessageByNavigation))]
        public virtual ICollection<GroupConversation> GroupConversations { get; set; }
        [InverseProperty(nameof(Group.CreatedByNavigation))]
        public virtual ICollection<Group> GroupCreatedByNavigations { get; set; }
        [InverseProperty(nameof(Group.ModifiedByNavigation))]
        public virtual ICollection<Group> GroupModifiedByNavigations { get; set; }
        [InverseProperty(nameof(UserContact.Contact))]
        public virtual ICollection<UserContact> UserContactContacts { get; set; }
        [InverseProperty(nameof(UserContact.User))]
        public virtual ICollection<UserContact> UserContactUsers { get; set; }
        [InverseProperty(nameof(UserGroup.User))]
        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
