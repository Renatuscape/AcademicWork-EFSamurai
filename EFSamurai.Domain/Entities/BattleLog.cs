using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSamurai.Domain.Entities
{
    public class BattleLog
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BattleId { get; set; }
        public Battle? Battle { get; set; }
        public ICollection<BattleLog>? BattleLogs { get; set; }
    }
}
