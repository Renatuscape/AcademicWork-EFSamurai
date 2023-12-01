using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSamurai.Domain.Entities
{
    public class SecretIdentity
    {
        public int Id { get; set; }
        public string RealName { get; set; } = string.Empty;
        public int SamuraiID { get; set; }
        public Samurai? Samurai { get; set; }
    }
}
