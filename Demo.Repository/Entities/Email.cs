using Dapper.Contrib.Extensions;
using Demo.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Repository.Entities
{
    [Table("emails")]
    public class Email : IEntityBase, IEmail
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string EmailAddress { get; set; }
    }
}
