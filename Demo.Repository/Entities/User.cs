using Dapper.Contrib.Extensions;
using Demo.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Repository.Entities
{
    [Table("users")]
    public class User : IEntityBase, IUser
    {
        [ExplicitKey]
        public int Id { get; set; }
    }
}
