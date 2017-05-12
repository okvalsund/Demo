using Dapper.Contrib.Extensions;
using Demo.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo.Repository.Entities
{
    [Table("changeLog")]
    public class ChangeLogItem : IEntityBase, IChangeLogItem
    {
        public int Id { get; set; }
        public string Verb { get; set; }
        public string Uri { get; set; }
        public string Changes { get; set; }

        public ChangeLogItem() { }

        public ChangeLogItem(string verb, string uri, string changes)
        {
            this.Verb = verb;
            this.Uri = uri;
            this.Changes = changes;
        }
    }
}
