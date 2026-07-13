using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessEntities;

namespace WebApi.Models.Users
{
    public class UserModel
    {
        [Required(ErrorMessage = "The name value is required")]
        [MaxLength(100, ErrorMessage = "The name is too long")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The email value is required")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        public UserTypes Type { get; set; }
        public int Age { get; set; } // tst comments: the age was missing in the model, but exists in the business entity and in the postman  request data
        public decimal? AnnualSalary { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}