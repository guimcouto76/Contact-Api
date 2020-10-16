using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;

namespace Contact.Api.Infrastructure
{
    public interface IContactDbContext
    {
        ILiteDatabase Database { get; }
    }
}
