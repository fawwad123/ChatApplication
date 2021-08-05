using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Model
{
    public class UserModel
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string MiddleName { get; set; }
        [MaxLength(50)]
        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }
        [MaxLength(255)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [MaxLength(255)]
        [Required(ErrorMessage = "Password is required")]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [MaxLength(255)]
        [Required(ErrorMessage = "Confirm password is required")]
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        [Required(ErrorMessage = "Date of birth is required")]
        public DateTime DateOfBirth { get; set; }
    }
}
