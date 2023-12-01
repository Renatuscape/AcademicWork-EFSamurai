using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSamurai.Domain.Entities
{
    public class Samurai
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public HairStyle? HairStyle { get; set; }

        public ICollection<Quote>? Quotes { get; set; }

        public SecretIdentity? SecretIdentity { get; set; }
    }
}
