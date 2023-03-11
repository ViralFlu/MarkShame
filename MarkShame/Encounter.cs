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

        public Encounter(string timestamp, string eventType, string encounterName)
        {
            Timestamp = timestamp;
            EventType = eventType;
            EncounterName = encounterName;
            SpellCastSuccessLines = new List<SpellCastSuccessLine>();
        }

        public static bool TryParse(string eventString, out Encounter encounter)
        {
            encounter = null;
            try
            {
                string[] eventParts = eventString.Split(',');
                if (eventParts.Length < 3)
                {
                    return false;
                }

                string[] timestampParts = eventParts[0].Split(' ');
                if (timestampParts.Length < 4)
                {
                    return false;
                }

                var eventType = timestampParts[3];
                var timestamp = timestampParts[0] + " " + timestampParts[1];
                var encounterName = eventParts[2];

                encounter = new Encounter(timestamp, eventType, encounterName);

                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

    }

}
