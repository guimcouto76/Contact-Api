using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Options;

namespace Contact.Api.Infrastructure
{
    public class ContactDbContext : IContactDbContext
    {
        public ILiteDatabase Database { get; }

        public ContactDbContext(IOptions<LiteDbOptions> options)
        {
            Database = new LiteDatabase(options.Value.DatabaseLocation);
        }

    }
}
