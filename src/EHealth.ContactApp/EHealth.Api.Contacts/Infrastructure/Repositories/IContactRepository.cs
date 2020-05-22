using EHealth.Api.Contacts.Domain.Model;
using EHealth.Api.Contacts.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EHealth.Api.Contacts.Infrastructure.Repositories
{
    public interface IContactRepository
    {
        Task<IEnumerable<ContactEntity>> GetAll();
        Task CreateAsync(ContactEntity contactEntity);

        Task<ContactEntity> FindByIdAsync(int id);

        Task<ContactEntity> UpdateAsync(ContactEntity contactEntity);

        Task<ContactEntity> DeleteAsync(int id);

        AppDBContext DB { get; }


    }
}
