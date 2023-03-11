using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkShame
{
    public class MarkParser
    {
        private readonly string targetName;

        public MarkParser(string targetName)
        {
            this.targetName = targetName;
        }

        public List<Tuple<string, string>> ParseFile(string filePath)
        {
            List<Tuple<string, string>> players = new List<Tuple<string, string>>();
            using (System.IO.StreamReader file = new System.IO.StreamReader(filePath))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    if (ParseLine(line, out string sourceName))
                    {
                        players.Add(Tuple.Create(sourceName, targetName));
                    }
                }
            }
            return players;
        }

        private bool ParseLine(string line, out string sourceName)
        {
            sourceName = null;
            if (line.Contains("SPELL_CAST_SUCCESS") && line.Contains("Conductive Mark") && line.Contains(targetName))
            {
                string[] fields = line.Split(',');
                sourceName = fields[6].Trim('"');
                return true;
            }
            return false;
        }
    }
}
