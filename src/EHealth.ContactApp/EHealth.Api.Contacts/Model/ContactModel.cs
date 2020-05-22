using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EHealth.Api.Contacts.Model
{
    public class ContactModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please provide firstname")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please provide lastname")]
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Please provide valid email address")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Please provide valid phone number")]
        public string PhoneNumber { get; set; }
        public Status Status { get; set; }
    }


    public class ContactResponse : BaseResponse
    {
        public ContactModel Contact { get; set; }
    }

    public class ContactBrowseResponse : BaseResponse
    {
        public IEnumerable<ContactModel> Contacts { get; set; }

    }

    public class BaseResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }

    public enum Status
    {
        InActive = 0,
        Active
    }
}
