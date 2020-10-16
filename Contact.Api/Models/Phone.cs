using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contact.Api.Models
{
    public enum PhoneType
    {
        Home,
        Work,
        Mobile
    }
    public class Phone
    {
        public string Number { get; set; }
        public PhoneType Type { get; set; }
    }
}
