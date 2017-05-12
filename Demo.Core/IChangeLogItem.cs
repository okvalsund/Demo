using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Core
{
    public interface IChangeLogItem
    {
        string Verb { get; set; }
        string Uri { get; set; }
        string Changes { get; set; }
    }
}
