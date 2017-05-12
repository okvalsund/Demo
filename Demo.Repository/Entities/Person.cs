using Dapper.Contrib.Extensions;
using Demo.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Repository.Entities
{
    [Table("persons")]
    public class Person : IEntityBase, IPerson
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        //public IEnumerable<IEmail> EmailAddresses { get; set; }
        //public IEnumerable<IAddress> Address { get; set; }
    }
}
