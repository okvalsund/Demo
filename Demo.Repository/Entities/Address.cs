using Dapper.Contrib.Extensions;
using Demo.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Repository.Entities
{
    [Table("addresses")]
    public class Address : IEntityBase, IAddress
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
    }
}
