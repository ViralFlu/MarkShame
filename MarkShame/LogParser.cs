using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MarkShame
{
    public static class LogParser
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
                            encounters.Add(new Encounter(line));
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
                                        var castSuccess = new SpellCastSuccessLine(line, isdath);
                                        castSuccess.Deaths++;
                                        lastEncounter.SpellCastSuccessLines.Add(castSuccess);
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

        //public static async Task<List<Encounter>> Parse(string filePath)
        //{
        //    var encounters = new List<Encounter>();
        //    var chunkSize = 1000000; // 1MB
        //    var tasks = new List<Task<List<Encounter>>>();

        //    using (var reader = new StreamReader(filePath))
        //    {
        //        var position = 0L;
        //        while (position < reader.BaseStream.Length)
        //        {
        //            var remaining = reader.BaseStream.Length - position;
        //            var chunkLength = Math.Min(chunkSize, remaining);
        //            var buffer = new byte[chunkLength];

        //            await reader.BaseStream.ReadAsync(buffer, 0, buffer.Length);
        //            var chunk = new string(ASCIIEncoding.ASCII.GetString(buffer));

        //            var task = Task.Run(() => ParseChunk(chunk));
        //            tasks.Add(task);

        //            position += chunkLength;
        //        }
        //    }

        //    await Task.WhenAll(tasks);

        //    foreach (var task in tasks)
        //    {
        //        encounters.AddRange(task.Result);
        //    }

        //    return encounters;
        //}

        //private static List<Encounter> ParseChunk(string chunk)
        //{
        //    var successLines = new List<Encounter>();
        //    Encounter currentEncounter = null;
        //    using (var reader = new StringReader(chunk))
        //    {
        //        string line;
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            var parts = line.Split(',');

        //            string[] eventParts = line.Split(',');
        //            string[] timestampParts = eventParts[0].Split(' ');
        //            var timestamp = timestampParts[0] + " " + timestampParts[1];
        //            var eventType = timestampParts[3];
        //            if (parts.Length < 2)
        //            {
        //                continue;
        //            }

        //            switch (eventType)
        //            {
        //                case "ENCOUNTER_Start":
        //                    currentEncounter = new Encounter(line);
        //                    successLines.Add(currentEncounter);

        //                    break;
        //                case "SPELL_CAST_SUCCESS":
        //                    if (parts.Length > 10)
        //                    {
        //                        var spellName = parts[10];

        //                        if (spellName == "\"Conductive Mark\"")
        //                        {
        //                            var castSuccess = new SpellCastSuccessLine(line);
        //                            castSuccess.Deaths = currentEncounter.Deaths;
        //                            currentEncounter.SpellCastSuccessLines.Add(castSuccess);
        //                        }
        //                    }

        //                    break;
        //                case "ENCOUNTER_END":
        //                    break;
        //                case "UNIT_DIED":
        //                    currentEncounter.Deaths++;
        //                    break;
        //            }
        //        }
        //    }

        //    return successLines;
        //}


    }
}