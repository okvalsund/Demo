using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Core
{
    public interface IPerson
    {
        string FirstName { get; set; }
        string MiddleName { get; set; }
        string LastName { get; set; }
    }
}
