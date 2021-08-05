using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DataAccessLayer.Entities
{
    [Table("Group")]
    public partial class Group
    {
        public Group()
        {
            GroupConversations = new HashSet<GroupConversation>();
            UserGroups = new HashSet<UserGroup>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ModifiedOn { get; set; }
        [StringLength(50)]
        public string GroupHash { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        [InverseProperty(nameof(User.GroupCreatedByNavigations))]
        public virtual User CreatedByNavigation { get; set; }
        [ForeignKey(nameof(ModifiedBy))]
        [InverseProperty(nameof(User.GroupModifiedByNavigations))]
        public virtual User ModifiedByNavigation { get; set; }
        [InverseProperty(nameof(GroupConversation.Group))]
        public virtual ICollection<GroupConversation> GroupConversations { get; set; }
        [InverseProperty(nameof(UserGroup.Group))]
        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
