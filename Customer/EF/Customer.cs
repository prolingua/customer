using System;
using System.Collections.Generic;

namespace Customer.EF
{
    public partial class Customer
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
    }
}
