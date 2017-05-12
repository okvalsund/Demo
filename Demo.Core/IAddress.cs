using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Core
{
    public interface IAddress
    {
        string Street { get; set; }
        string ZipCode { get; set; }
        string City { get; set; }
    }
}
