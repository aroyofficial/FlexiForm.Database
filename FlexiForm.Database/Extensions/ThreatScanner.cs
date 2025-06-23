using FlexiForm.Database.Enumerations;
using FlexiForm.Database.Exceptions;
using FlexiForm.Database.Models;
using System.Text.RegularExpressions;

namespace FlexiForm.Database.Extensions
{
    /// <summary>
    /// Provides functionality to scan SQL scripts for known threat patterns
    /// such as SQL injection attempts, dangerous procedure calls, or destructive commands.
    /// </summary>
    public static class ThreatScanner
    {
        /// <summary>
        /// A static mapping of known malicious regex patterns to their associated threat types.
        /// These patterns are evaluated line-by-line during script analysis.
        /// </summary>
        private static readonly IDictionary<Regex, ThreatType> KnownThreats;

        /// <summary>
        /// Initializes the <see cref="ThreatScanner"/> class by populating the known threat patterns.
        /// </summary>
        static ThreatScanner()
        {
            KnownThreats = new Dictionary<Regex, ThreatType>()
            {
                { new(@"\bDROP\s+DATABASE\b", RegexOptions.IgnoreCase), ThreatType.DropDatabase },
                { new(@"\bEXEC\s+xp_cmdshell\b", RegexOptions.IgnoreCase), ThreatType.XpCmdShellExecution },
                { new(@"\bUNION\s+SELECT\b", RegexOptions.IgnoreCase), ThreatType.UnionSelectInjection },
                { new(@"\bWAITFOR\s+DELAY\b", RegexOptions.IgnoreCase), ThreatType.TimeDelayAttack },
                { new(@"\bOR\s+1\s*=\s*1\b", RegexOptions.IgnoreCase), ThreatType.BooleanInjection },
                { new(@"0x[0-9a-fA-F]+", RegexOptions.IgnoreCase), ThreatType.HexLiteralPayload }
            };
        }

        /// <summary>
        /// Scans the contents of the specified <see cref="Script"/> for known malicious SQL patterns.
        /// If matching patterns are found, a <see cref="Threat"/> object is returned containing
        /// detailed insight about each threat detected.
        /// </summary>
        /// <param name="script">The SQL script to analyze for threats.</param>
        /// <returns>
        /// A <see cref="Threat"/> object if any threats are found; otherwise, <c>null</c>.
        /// </returns>
        public static Threat? Scan(this Script script)
        {
            if (script == null)
            {
                throw new InvalidScriptException();
            }

            var threats = new List<ThreatInfo>();
            using (var reader = new StreamReader(script.AbsolutePath))
            {
                string? line;
                int currentLine = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    currentLine++;
                    foreach (var (pattern, threat) in KnownThreats)
                    {
                        if (pattern.IsMatch(line))
                        {
                            var threatInfo = new ThreatInfo()
                            {
                                Line = currentLine,
                                Type = threat,
                            };
                            threats.Add(threatInfo);
                        }
                    }
                }
            }

            return threats.Any() ? new Threat() { Insights = threats } : null;
        }
    }
}
