using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.Api.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public Name Name { get; set; }
        public Address Address { get; set; }
        public List<Phone> Phone { get; set; }
        public string Email { get; set; }
    }
}
