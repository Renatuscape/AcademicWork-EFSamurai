using Azure;
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
        public ICollection<BattleEvent>? BattleEvents { get; set; }

        public override string ToString()
        {
            StringBuilder text = new();
            text.AppendLine(Name);
            if (BattleEvents != null)
            {
                text.AppendLine("Events:");
                foreach (BattleEvent batEvent in BattleEvents)
                {
                    text.AppendLine($"\t Order: {batEvent.Order} Summary: {batEvent.Summary}");
                }
            }

            return text.ToString();
        }
    }
}
