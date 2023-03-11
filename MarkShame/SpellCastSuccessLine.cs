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

        public SpellCastSuccessLine(string timestamp, string eventType, string casterId, string casterInfo,
            string targetId, string targetInfo, int spellId, string spellName, string sourceId, int xCoord, int yCoord,
            int zCoord)
        {
            Timestamp = timestamp;
            EventType = eventType;
            CasterId = casterId;
            CasterInfo = casterInfo;
            TargetId = targetId;
            TargetInfo = targetInfo;
            SpellId = spellId;
            SpellName = spellName;
            SourceId = sourceId;
            XCoord = xCoord;
            YCoord = yCoord;
            ZCoord = zCoord;
            Deaths = 0;
        }
    }

    public static class SpellCastSuccessLineParser
    {
        public static bool TryParse(string eventString, bool isDath, out SpellCastSuccessLine spellCastSuccessLine)
        {
            spellCastSuccessLine = null;
            try
            {
                string[] eventParts = eventString.Split(',');
                if (eventParts.Length < 18)
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
                var casterId = eventParts[1];
                var casterInfo = eventParts[2];
                var targetId = "";
                var targetInfo = "";
                var spellId = 0;
                var spellName = "";
                var sourceId = "";
                var xCoord = 0;
                var yCoord = 0;
                var zCoord = 0;

                if (isDath)
                {
                    targetId = eventParts[6];
                    targetInfo = eventParts[7];
                    spellName = eventParts[11].Trim('"');
                    sourceId = eventParts[13];
                    int.TryParse(eventParts[15], out xCoord);
                    int.TryParse(eventParts[16], out yCoord);
                    int.TryParse(eventParts[17], out zCoord);
                }
                else
                {
                    targetId = eventParts[5];
                    targetInfo = eventParts[6];
                    int.TryParse(eventParts[9], out spellId);
                    spellName = eventParts[10].Trim('"');
                    sourceId = eventParts[13];
                    int.TryParse(eventParts[15], out xCoord);
                    int.TryParse(eventParts[16], out yCoord);
                    int.TryParse(eventParts[17], out zCoord);
                }

                spellCastSuccessLine = new SpellCastSuccessLine(timestamp, eventType, casterId, casterInfo, targetId, targetInfo, spellId, spellName, sourceId, xCoord, yCoord, zCoord);
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }



    }


}
