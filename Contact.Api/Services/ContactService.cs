using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Contact.Api.Infrastructure;
using LiteDB;

namespace Contact.Api.Services
{
    public class ContactService: IContactService
    {
        private const string ContactCollection = "Contact";
        private readonly ILiteDatabase _contactDb;

        public ContactService(IContactDbContext  contactDbContext)
        {
            _contactDb = contactDbContext.Database;
        }

        public async Task<bool> Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Id");

            return await Task.Run(() =>
                _contactDb.GetCollection<Models.Contact>(ContactCollection)
                    .Delete(id));
        }

        public async Task<IEnumerable<Models.Contact>> FindAll()
        {
            return await Task.Run(() => 
                _contactDb.GetCollection<Models.Contact>(ContactCollection)
                    .FindAll());
        }

        public async Task<Models.Contact> FindOne(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Id");

            return await Task.Run(() => 
                _contactDb.GetCollection<Models.Contact>(ContactCollection)
                    .FindById(id));
        }
        public async Task<int> Insert(Models.Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            return await Task.Run(() =>
                _contactDb.GetCollection<Models.Contact>(ContactCollection)
                    .Insert(contact));
        }

        public async Task<bool> Update(int id, Models.Contact contact)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid Id");

            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            return await Task.Run(() =>
            {
                contact.Id = id;
                return _contactDb.GetCollection<Models.Contact>(ContactCollection)
                    .Update(contact);
            });
        }
    }
}
