using EHealth.Api.Contacts.Domain.Model;
using EHealth.Api.Contacts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EHealth.Api.Contacts.Services
{
    public interface IContactService
    {

        Task<ContactBrowseResponse> ListAsync();
        Task<ContactResponse> CreateAsync(ContactModel contact);
        Task<ContactResponse> UpdateAsync(ContactModel contact);
        Task<ContactResponse> DeleteAsync(int id);
        Task<ContactModel> FindByIdAsync(int id);

    }
}
