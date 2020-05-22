using AutoMapper;
using EHealth.Api.Contacts.Domain.Model;
using EHealth.Api.Contacts.Infrastructure.Repositories;
using EHealth.Api.Contacts.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EHealth.Api.Contacts.Services
{
    public class ContactService : IContactService
    {

        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ContactService> _logger;
        public ContactService(IContactRepository contactRepository, IMapper mapper, ILogger<ContactService> logger)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
            _logger = logger;
        }

        

        public async Task<ContactBrowseResponse> ListAsync()
        {
            try
            {
                var contacts = await _contactRepository.GetAll();
                return new ContactBrowseResponse {
                    Contacts = _mapper.Map<IEnumerable<ContactEntity>, IEnumerable<ContactModel>>(contacts),
                    IsSuccess = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                var message = $"ContactService: get list failed: {ex.Message}";
                _logger.LogError(ex, message);
                return new ContactBrowseResponse { Contacts = null, IsSuccess = false, Message = message };
            }


        }

        public async Task<ContactResponse> CreateAsync(ContactModel contact)
        {
            try
            {
                var contactEntity = _mapper.Map<ContactModel, ContactEntity>(contact);
                await _contactRepository.CreateAsync(contactEntity);
                await _contactRepository.DB.SaveChangesAsync();


                return new ContactResponse { 
                    Contact = _mapper.Map<ContactModel>(contactEntity), 
                    IsSuccess = true, 
                    Message = "" 
                };

            }catch(Exception ex)
            {
                var message = $"ContactService: save failed: {ex.Message}";
                _logger.LogError(ex, message);
                return new ContactResponse { Contact = null, IsSuccess = false, Message = message};
            }

        }

        public async Task<ContactResponse> UpdateAsync(ContactModel contact)
        {
            var existingContact = await _contactRepository.UpdateAsync(_mapper.Map<ContactEntity>(contact));
            if (existingContact == null)
            {
                _logger.LogWarning($"Contact is not present. Contact id: {contact.Id}");
                return new ContactResponse { Contact = null, IsSuccess = false, Message = "Contact is not present." };
            }

            try
            {
                await _contactRepository.DB.SaveChangesAsync();
                return new ContactResponse
                {
                    Contact = _mapper.Map<ContactModel>(existingContact),
                    IsSuccess = true,
                    Message = ""
                };
            }
            catch (Exception ex)
            {
                var message = $"ContactService: update failed: {ex.Message}";
                _logger.LogError(ex, message);
                return new ContactResponse { Contact = null, IsSuccess = false, Message = message };
            }
        }

        public async Task<ContactResponse> DeleteAsync(int id)
        {
            var existingContact = await _contactRepository.DeleteAsync(id);

            if (existingContact == null)
            {
                _logger.LogWarning($"Contact is not present. Contact id: {id}");
                return new ContactResponse { Contact = null, IsSuccess = false, Message = "Contact is not present." };
            }

            try
            {
                
                await _contactRepository.DB.SaveChangesAsync();

                return new ContactResponse
                {
                    Contact = _mapper.Map<ContactModel>(existingContact),
                    IsSuccess = true,
                    Message = "Contact is deleted successfully."
                };
            }
            catch (Exception ex)
            {
                var  message = $"ContactService: delete failed: {ex.Message}";
                _logger.LogError(ex,message);
                return new ContactResponse { Contact = null, IsSuccess = false, Message = message };
            }
        }

        public async Task<ContactModel> FindByIdAsync(int id)
        {

            var existingcontact = await _contactRepository.FindByIdAsync(id);
            var contact = _mapper.Map<ContactModel>(existingcontact);
            return contact;
        }
    }
}
