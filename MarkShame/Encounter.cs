using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkShame
{
    public class Encounter
    {
        public string Timestamp { get; set; }
        public string EventType { get; set; }
        public List<SpellCastSuccessLine> SpellCastSuccessLines { get; }
        public int Deaths { get; set; }
        public string EncounterName { get; set; }

        public Encounter(string eventString)
        {
            string[] eventParts = eventString.Split(',');
            string[] timestampParts = eventParts[0].Split(' ');
            Timestamp = timestampParts[0] + " " + timestampParts[1];
            EventType = timestampParts[3];
            EncounterName = eventParts[2];
            SpellCastSuccessLines = new List<SpellCastSuccessLine>();

        }
    }
}
