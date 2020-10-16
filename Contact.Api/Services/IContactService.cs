using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contact.Api.Models;

namespace Contact.Api.Services
{
    public interface IContactService
    {
        Task<IEnumerable<Models.Contact>> FindAll();
        Task<Models.Contact> FindOne(int id);
        Task<int> Insert(Models.Contact contact);
        Task<bool> Update(int id, Models.Contact contact);
        Task<bool> Delete(int id);
    }
}
