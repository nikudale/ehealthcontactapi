using EHealth.Api.Contacts.Domain.Model;
using EHealth.Api.Contacts.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EHealth.Api.Contacts.Infrastructure.Repositories
{
    public class ContactRepository : BaseRepository, IContactRepository
    {

        public ContactRepository(DbContext dbContext) : base(dbContext)
        {
        }

        public AppDBContext DB => (AppDBContext)_apDbContext;

        public async Task CreateAsync(ContactEntity contactEntity) => await DB.Contacts.AddAsync(contactEntity);

        public async Task<ContactEntity> FindByIdAsync(int id) => await DB.Contacts.FindAsync(id);

        public async Task<IEnumerable<ContactEntity>> GetAll() => await DB.Contacts.AsNoTracking().ToListAsync();

        public async Task<ContactEntity> DeleteAsync(int id)
        {
            var existingContact = await FindByIdAsync(id);

            if (existingContact == null)
                return null;

            existingContact.Status = Status.InActive;
            return existingContact;
        }

        public async Task<ContactEntity> UpdateAsync(ContactEntity contactEntity)
        {
            var existingContact = await FindByIdAsync(contactEntity.Id);

            if (existingContact == null)
                return null;

            existingContact.FirstName = contactEntity.FirstName;
            existingContact.LastName = contactEntity.LastName;
            existingContact.Email = contactEntity.Email;
            existingContact.PhoneNumber = contactEntity.PhoneNumber;

            return existingContact;
        }
    }
}
