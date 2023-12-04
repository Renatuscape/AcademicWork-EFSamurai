using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSamurai.Domain.Entities
{
    public class BattleEvent
    {
        public int Id { get; set; }
        public int Order { get; set; }
        public string Summary { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int BattleLogId { get; set; }
        public BattleLog? BattleLog { get; set; }
    }
}
