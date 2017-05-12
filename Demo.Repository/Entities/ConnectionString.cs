using Demo.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Repository.Entities
{
    public class ConnectionString : IConnectionString
    {
        public ConnectionString(string path)
        {
            this.Path = path;
        }
        public string Path { get; set; }
    }
}
