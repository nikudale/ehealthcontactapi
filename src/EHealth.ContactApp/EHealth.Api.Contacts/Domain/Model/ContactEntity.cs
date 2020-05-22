using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EHealth.Api.Contacts.Domain.Model
{
    public class ContactEntity
    {

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public Status Status { get; set; }

    }

    public enum Status
    {
        InActive = 0,
        Active
    }
}
