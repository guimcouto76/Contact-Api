using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contact.Api.Infrastructure;
using Contact.Api.Models;
using Contact.Api.Services;
using LiteDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Contact.Test
{
    [TestClass]
    public class ContactServiceTest
    {
        #region Initialization

        private const string ContactCollection = "Contact";
        private const int ValidId = 123;
        private const int InvalidId = 999;
        private readonly IContactDbContext _mockedContactDbContext;

        public ContactServiceTest()
        {
            _mockedContactDbContext = GetMockedContactDbContext();
        }

        #endregion

        #region Mocks
        private IContactDbContext GetMockedContactDbContext()
        {
            var mockedContactDbContext = new Mock<IContactDbContext>();

            var liteDatabase = GetMockedLiteDatabase();

            mockedContactDbContext
                .SetupGet(x => x.Database)
                .Returns(liteDatabase);

            return mockedContactDbContext.Object;
        }

        private ILiteDatabase GetMockedLiteDatabase()
        {
            var mockedLiteDatabase = new Mock<ILiteDatabase>();

            mockedLiteDatabase
                .Setup(x => x.GetCollection<Api.Models.Contact>(It.Is<string>(q => q == ContactCollection), It.IsAny<BsonAutoId>()))
                .Returns(GetMockedLiteCollection());

            return mockedLiteDatabase.Object;
        }

        private ILiteCollection<Api.Models.Contact> GetMockedLiteCollection()
        {
            var mockedLiteCollection = new Mock<ILiteCollection<Api.Models.Contact>>();

            mockedLiteCollection
                .Setup(x => x.FindAll())
                .Returns(new List<Api.Models.Contact>());

            mockedLiteCollection
                .Setup(x => x.FindById(It.Is<BsonValue>(q => q == ValidId)))
                .Returns(new Api.Models.Contact());

            mockedLiteCollection
                .Setup(x => x.FindById(It.Is<BsonValue>(q => q != ValidId)))
                .Returns<Api.Models.Contact>(null);

            mockedLiteCollection
                .Setup(x => x.Insert(It.Is<Api.Models.Contact>(q => q != null)))
                .Returns(1);

            mockedLiteCollection
                .Setup(x => x.Update(It.Is<Api.Models.Contact>(q => q != null)))
                .Returns(true);

            mockedLiteCollection
                .Setup(x => x.Delete(It.Is<BsonValue>(q => q == ValidId)))
                .Returns(true);

            mockedLiteCollection
                .Setup(x => x.Delete(It.Is<BsonValue>(q => q != ValidId)))
                .Returns(false);

            return mockedLiteCollection.Object;

        }

        #endregion

        #region FindAll

        [TestMethod]
        public async Task FindAll_MockedDatabase_ReturnNotNull()
        {
            // Arrange
            var contactService = new ContactService(_mockedContactDbContext);

            // Act
            var contacts = await contactService.FindAll();

            // Assert
            Assert.IsNotNull(contacts);
        }

        #endregion

        #region FindOne

        [TestMethod]
        public async Task FindOne_ValidId_ReturnNotNull()
        {
            // Arrange
            var contactService = new ContactService(_mockedContactDbContext);

            // Act
            var contacts = await contactService.FindOne(ValidId);

            // Assert
            Assert.IsNotNull(contacts);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public async Task FindOne_InvalidId_RaiseArgumentException(int id)
        {
            // Arrange
            var contactService = new ContactService(_mockedContactDbContext);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => contactService.FindOne(id));
        }

        [DataTestMethod]
        [DataRow(111)]
        [DataRow(222)]
        [DataRow(333)]
        public async Task FindOne_InvalidId_ReturnNull(int id)
        {
            // Arrange
            var contactService = new ContactService(_mockedContactDbContext);

            // Act
            var contacts = await contactService.FindOne(id);

            // Assert
            Assert.IsNull(contacts);
        }

        #endregion

        #region Insert

        [TestMethod]
        public async Task Insert_ValidContact_ReturnTrue()
        {
            // Arrange
            var contactService = new ContactService(_mockedContactDbContext);
            var contact = new Api.Models.Contact
            {
                Name = new Name
                {
                    First = "Guilherme",
                    Middle = "M",
                    Last = "Couto"
                },
                Address = new Address
                {
                    Street = "12345 Main Street",
                    City = "West Melboune",
                    State = "FL",
                    Zip = "32904"
                },
                Phone = new List<Phone>() 
                { 
                    new Phone
                    {
                        Number = "321-312-9692" ,
                        Type = PhoneType.Mobile
                    }
                },
                Email = "guimcouto76@gmail.com"
            };

            // Act
            var id = await contactService.Insert(contact);

            // Assert
            Assert.IsTrue(id > 0);
        }

        [TestMethod]
        public async Task Insert_NullContact_RaiseArgumentNullException()
        {
            // Arrange
            var contactService = new ContactService(_mockedContactDbContext);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => contactService.Insert(null));
        }

        #endregion

        #region Update

        [TestMethod]
        public async Task Update_ValidContact_ReturnTrue()
        {
            // Arrange
            var contactService = new ContactService(_mockedContactDbContext);
            var contact = new Api.Models.Contact
            {
                Name = new Name
                {
                    First = "Guilherme",
                    Middle = "Marques",
                    Last = "Couto"
                },
                Address = new Address
                {
                    Street = "12345 Main Street",
                    City = "West Melboune",
                    State = "FL",
                    Zip = "32904"
                },
                Phone = new List<Phone>()
                {
                    new Phone
                    {
                        Number = "321-312-9692" ,
                        Type = PhoneType.Mobile
                    }
                },
                Email = "guimcouto76@gmail.com"
            };

            // Act
            var result = await contactService.Update(ValidId, contact);

            // Assert
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public async Task Update_InvalidId_RaiseArgumentException(int id)
        {
            // Arrange
            var contactService = new ContactService(_mockedContactDbContext);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => contactService.Update(id, null));
        }


        [TestMethod]
        public async Task Update_NullContact_RaiseArgumentNullException()
        {
            // Arrange
            var contactService = new ContactService(_mockedContactDbContext);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() => contactService.Update(ValidId,null));
        }

        #endregion
        
        #region Delete

        [TestMethod]
        public async Task Delete_ValidId_ReturnTrue()
        {
            // Arrange
            var contactService = new ContactService(_mockedContactDbContext);

            // Act
            var contacts = await contactService.Delete(ValidId);

            // Assert
            Assert.IsTrue(contacts);
        }

        [DataTestMethod]
        [DataRow(-1)]
        [DataRow(0)]
        public async Task Delete_InvalidId_RaiseArgumentException(int id)
        {
            // Arrange
            var contactService = new ContactService(_mockedContactDbContext);

            // Act and Assert
            await Assert.ThrowsExceptionAsync<ArgumentException>(() => contactService.Delete(id));
        }

        [DataTestMethod]
        [DataRow(111)]
        [DataRow(222)]
        [DataRow(333)]
        public async Task Delete_InvalidId_ReturnFalse(int id)
        {
            // Arrange
            var contactService = new ContactService(_mockedContactDbContext);

            // Act
            var contacts = await contactService.Delete(id);

            // Assert
            Assert.IsFalse(contacts);
        }

        #endregion
    }
}
