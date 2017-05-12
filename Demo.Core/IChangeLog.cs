using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core
{
    public interface IChangeLog
    {
        
        Task<int> LogCreated(string verb, string uri, string obj);
        Task<int> LogUpated(string verb, string uri, object oldObject, object newObject);
        Task<int> LogDeleted(string verb, string uri, string obj);
        Task<string> Changes(object oldObject, object newObject);
    }
}
 