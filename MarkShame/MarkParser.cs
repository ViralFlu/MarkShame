using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarkShame
{
    public static class MarkParser
    {
        public static async Task<List<Encounter>> Parse(string filePath)
        {
            var encounters = new List<Encounter>();
            using (var reader = new StreamReader(filePath))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    var parts = line.Split(',');

                    string[] eventParts = line.Split(',');
                    string[] timestampParts = eventParts[0].Split(' ');
                    var timestamp = timestampParts[0] + " " + timestampParts[1];
                    var eventType = timestampParts[3];
                    if (parts.Length < 2)
                    {
                        continue;
                    }

                    switch (eventType)
                    {
                        case "ENCOUNTER_START":
                            if(Encounter.TryParse(line, out var encounter))
                                encounters.Add(encounter);
                            break;
                        case "SPELL_CAST_SUCCESS":
                            if (parts.Length > 10)
                            {
                                var spellName = parts[10];
                                var dathSpell = parts[11];
                                bool isdath = dathSpell == "\"Conductive Mark\"";
                                if (spellName == "\"Conductive Mark\"" || isdath)
                                {
                                    var lastEncounter = encounters.LastOrDefault();
                                    if (lastEncounter != null)
                                    {
                                        if (SpellCastSuccessLineParser.TryParse(line, isdath, out var castSuccess))
                                        {
                                            castSuccess.Deaths++;
                                            lastEncounter.SpellCastSuccessLines.Add(castSuccess);
                                        }
                                    }
                                }
                            }

                            break;
                        case "ENCOUNTER_END":
                            var currentEncounter = encounters.LastOrDefault();
                            if (currentEncounter != null)
                            {
                                currentEncounter = null;
                            }
                            break;
                        case "UNIT_DIED":
                            var currentDeathEncounter = encounters.LastOrDefault();
                            if (currentDeathEncounter != null)
                            {
                                currentDeathEncounter.Deaths++;
                            }
                            break;
                    }
                }
            }

            return encounters;
        }
    }
}