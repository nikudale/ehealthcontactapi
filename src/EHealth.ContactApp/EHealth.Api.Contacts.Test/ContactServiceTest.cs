using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using EHealth.Api.Contacts.Services;
using EHealth.Api.Contacts.Infrastructure.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using EHealth.Api.Contacts.Domain.Model;
using EHealth.Api.Contacts.Model;
using EHealth.Api.Contacts.Mappings;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EHealth.Api.Contacts.Infrastructure.Context;
using System.Collections;

namespace EHealth.Api.Contacts.Test
{
    public class ContactServiceTest
    {

        private readonly ContactService _contactService;
        private readonly Mock<IContactRepository> conRepoMock = new Mock<IContactRepository>();
        //private readonly Mock<IMapper> mapRepoMock = new Mock<IMapper>();
        private readonly Mock<ILogger<ContactService>> loggerRepoMock = new Mock<ILogger<ContactService>>();
        private IMapper mapper;


        public ContactServiceTest()
        {
            MapperConfiguration mockMapper =  new MapperConfiguration(cfg =>{cfg.AddProfile(new ContactMapping());});
            mapper = mockMapper.CreateMapper();
            _contactService = new ContactService(conRepoMock.Object, mapper, loggerRepoMock.Object);
        }


        private async Task<DbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new AppDBContext(options);
            databaseContext.Database.EnsureCreated();
            if (await databaseContext.Contacts.CountAsync() <= 0)
            {
                for (int i = 1; i <= 10; i++)
                {
                    databaseContext.Contacts.Add(new ContactEntity()
                    {
                        Id = i,
                        Email = $"testuser{i}@example.com",
                        Status = (Domain.Model.Status)(i % 2),
                        FirstName = "FirstName" + i,
                        LastName = "LastName" + i,
                        PhoneNumber = "111111111" + i
                    }); ;
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async Task ListAsync_ShouldReturnContacts_IfExists()
        {
            //Arrange
            IList<ContactEntity> contactEntities = new List<ContactEntity>();
            contactEntities.Add(new ContactEntity { Id = 1, FirstName = "AAA", LastName = "BBB" });
            contactEntities.Add(new ContactEntity { Id = 2, FirstName = "CCC", LastName = "DDD" });

            conRepoMock.Setup(c => c.GetAll()).ReturnsAsync(contactEntities);

            //Act
            var contacts = await _contactService.ListAsync();

            //Assert
            Assert.NotNull(contacts);

            Assert.NotNull(contacts.Contacts);


        }

        [Fact]
        public async Task FindByIdAsync_ShouldReturnContact_IfExists()
        {
            //Arrange

            var contactId = 100;

            var contactEntity = new ContactEntity { Id = contactId, FirstName = "AAA", LastName = "BBB" };

            conRepoMock.Setup(c => c.FindByIdAsync(contactId)).ReturnsAsync(contactEntity);

            //Act
            var contact = await _contactService.FindByIdAsync(contactId);

            //Assert
            Assert.NotNull(contact);

            Assert.Equal(contactId, contact.Id);


        }

        /// <summary>
        /// Using In-Memory database test
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task FindByIdAsync_InDB_ShouldReturnContact_IfExists()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            ContactRepository repository = new ContactRepository(dbContext);
            IContactService service = new ContactService(repository, mapper, loggerRepoMock.Object);
            int contactId = 1;
            //conRepoMock.Setup(c => c.FindByIdAsync(contactId)).ReturnsAsync(contactEntity);

            //Act
            var contact = await service.FindByIdAsync(contactId);

            //Assert
            Assert.NotNull(contact);

            Assert.Equal(contactId, contact.Id);


        }

        [Fact]
        public async Task CreateAsync_InDB_ShouldReturnContact_IfSuccess()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            ContactRepository repository = new ContactRepository(dbContext);
            IContactService service = new ContactService(repository, mapper, loggerRepoMock.Object);
            ContactModel newContact = new ContactModel { Id = 11, FirstName = "AAA", LastName = "BBB", Email = "test@test.com", PhoneNumber = "1111111111", Status = Model.Status.Active };
            //Act
            var contact = await service.CreateAsync(newContact);

            //Assert
            Assert.NotNull(contact);
            Assert.True(contact.IsSuccess);
            Assert.Equal(newContact.Id, contact.Contact.Id);

        }

        [Fact]
        public async Task CreateAsync_InDB_ShouldReturnContactWithErrorMessage_IfFailed()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            ContactRepository repository = new ContactRepository(dbContext);
            IContactService service = new ContactService(repository, mapper, loggerRepoMock.Object);
            ContactModel newContact = new ContactModel { Id = 1, FirstName = "AAA", LastName = "LastName1", Email = "testuser1@example.com", PhoneNumber = "1111111111", Status = Model.Status.Active };

            //Act
            var updateContactResponse = await service.CreateAsync(newContact);

            //Assert
            Assert.NotNull(updateContactResponse);
            Assert.False(updateContactResponse.IsSuccess);
            Assert.Null(updateContactResponse.Contact);

        }


        [Fact]
        public async Task UpdateAsync_ShouldReturnContact_IfSuccess()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            ContactRepository repository = new ContactRepository(dbContext);
            IContactService service = new ContactService(repository, mapper, loggerRepoMock.Object);
            string updateFirstName = "AAA";
            ContactModel updateContact = new ContactModel { Id = 1, FirstName = updateFirstName, LastName = "LastName1", Email = "testuser1@example.com", PhoneNumber = "1111111111", Status = Model.Status.Active };
            //Act
            var updateContactResponse = await service.UpdateAsync(updateContact);

            //Assert
            Assert.NotNull(updateContactResponse);

            Assert.Equal(updateFirstName, updateContactResponse.Contact.FirstName);


        }


        [Fact]
        public async Task DeleteAsync_ShouldUpdateContactInActive_IfSuccess()
        {
            //Arrange
            var dbContext = await GetDatabaseContext();
            ContactRepository repository = new ContactRepository(dbContext);
            IContactService service = new ContactService(repository, mapper, loggerRepoMock.Object);
            var deleteId = 1;
            var expectedStatus = 0;

            //Act

            var deletedContactResponse = await service.DeleteAsync(deleteId);

            //Assert
            Assert.NotNull(deletedContactResponse);

            Assert.Equal(expectedStatus, (int)deletedContactResponse.Contact.Status);


        }


    }
}
