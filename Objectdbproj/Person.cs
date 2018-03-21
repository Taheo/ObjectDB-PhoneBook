using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objectdbproj
{
    class Person
    {
        public string Name { get; set; }
        public string Lastname { get; set; }
        public Address Adress { get; set; }
        public List<Phone> Contacts { get; set; }
    }
}
