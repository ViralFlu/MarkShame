using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkShame
{
    using System;

    public class SpellCastSuccessLine
    {
        public string Timestamp { get; set; }
        public string EventType { get; set; }
        public string CasterId { get; set; }
        public string CasterInfo { get; set; }
        public string TargetId { get; set; }
        public string TargetInfo { get; set; }
        public int SpellId { get; set; }
        public string SpellName { get; set; }
        public string SourceId { get; set; }
        public int XCoord { get; set; }
        public int YCoord { get; set; }
        public int ZCoord { get; set; }
        public int Deaths { get; set; }
        public SpellCastSuccessLine(string eventString, bool isDath)
        {
            Deaths = 0;
            string[] eventParts = eventString.Split(',');
            string[] timestampParts = eventParts[0].Split(' ');
            Timestamp = timestampParts[0] + " " + timestampParts[1];
            EventType = timestampParts[3];
            CasterId = eventParts[1];
            CasterInfo = eventParts[2];
            if (isDath)
            {
                TargetId = eventParts[6];
                TargetInfo = eventParts[7];
                SpellName = eventParts[11].Trim('"');
                SourceId = eventParts[13];
                XCoord = int.Parse(eventParts[15]);
                YCoord = int.Parse(eventParts[16]);
                ZCoord = int.Parse(eventParts[17]);
            }
            else
            {
                TargetId = eventParts[5];
                TargetInfo = eventParts[6];
                SpellId = int.Parse(eventParts[9]);
                SpellName = eventParts[10].Trim('"');
                SourceId = eventParts[13];
                XCoord = int.Parse(eventParts[15]);
                YCoord = int.Parse(eventParts[16]);
                ZCoord = int.Parse(eventParts[17]);
            }
        }
    }

}
