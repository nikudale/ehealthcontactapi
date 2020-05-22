using AutoMapper;
using EHealth.Api.Contacts.Domain.Model;
using EHealth.Api.Contacts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EHealth.Api.Contacts.Mappings
{
    public class ContactMapping : Profile
    {

        public ContactMapping()
        {
            CreateMap<ContactEntity, ContactModel>().ReverseMap();
        }
    }
}
