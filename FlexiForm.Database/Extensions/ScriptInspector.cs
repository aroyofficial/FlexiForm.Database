using FlexiForm.Database.Enumerations;
using FlexiForm.Database.Exceptions;
using FlexiForm.Database.Models;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Globalization;

namespace FlexiForm.Database.Extensions
{
    /// <summary>
    /// Provides extension methods for inspecting and analyzing SQL migration scripts,
    /// including syntax validation and metadata extraction from header comments.
    /// </summary>
    public static class ScriptInspector
    {
        /// <summary>
        /// Parses the SQL script file specified in the <paramref name="script"/> instance
        /// using Transact-SQL 17.0 parser to determine whether the script is syntactically valid.
        /// </summary>
        /// <param name="script">The script object containing the absolute path to the SQL file.</param>
        /// <returns>
        /// A <see cref="ParseResult"/> object that contains the list of syntax errors, if any.
        /// </returns>
        /// <exception cref="InvalidScriptException">
        /// Thrown if the <paramref name="script"/> parameter is null.
        /// </exception>
        public static ParseResult Parse(this Script script)
        {
            if (script == null)
            {
                throw new InvalidScriptException();
            }

            using (var reader = new StreamReader(script.AbsolutePath))
            {
                var parser = new TSql170Parser(false);
                parser.Parse(reader, out IList<ParseError> errors);
                return new ParseResult()
                {
                    Errors = errors
                };
            }
        }

        /// <summary>
        /// Parses the header comments of a SQL script file to extract structured metadata into a <see cref="ScriptMetadata"/> object.
        /// The method looks for predefined comment patterns such as Script ID, Script Type, Name, Migration Type, and Created At.
        /// </summary>
        /// <param name="script">The <see cref="Script"/> object representing the SQL file to parse.</param>
        /// <returns>
        /// A populated <see cref="ScriptMetadata"/> object containing extracted metadata from the script header.
        /// </returns>
        public static ScriptMetadata ParseHeader(this Script script)
        {
            if (script == null)
            {
                throw new InvalidScriptException();
            }

            var metadata = new ScriptMetadata();
            var metadataDict = new Dictionary<string, bool>()
            {
                { "ScriptId", false },
                { "ScriptType", false },
                { "Name", false },
                { "MigrationType", false },
                { "CreatedAt", false }
            };

            using (var reader = new StreamReader(script.AbsolutePath))
            {
                var line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith("--"))
                    {
                        // Extract Script Type
                        if (line.StartsWith("-- Script Type"))
                        {
                            try
                            {
                                var info = line.Split(':')[1].Trim();
                                var scriptType = info.ToLower();
                                switch (scriptType)
                                {
                                    case "alter":
                                        metadata.Type = ScriptType.Alter;
                                        break;
                                    case "schema":
                                        metadata.Type = ScriptType.Schema;
                                        break;
                                    case "proc":
                                        metadata.Type = ScriptType.Proc;
                                        break;
                                    default:
                                        metadata.Type = ScriptType.Unknown;
                                        break;
                                }
                                metadataDict["ScriptType"] = true;
                            }
                            catch
                            {
                                metadataDict["ScriptType"] = false;
                            }
                        }

                        // Extract Name
                        if (line.StartsWith("-- Name"))
                        {
                            try
                            {
                                var info = line.Split(':')[1].Trim();
                                metadata.Name = info;
                                metadataDict["Name"] = true;
                            }
                            catch
                            {
                                metadataDict["Name"] = false;
                            }
                        }

                        // Extract Created/Updated At
                        if (line.StartsWith("-- Created At") || line.StartsWith("-- Updated At"))
                        {
                            try
                            {
                                var auditPartIdentifiers = line.Split(':');
                                var auditSignature = string.Join(":", auditPartIdentifiers.Skip(1)).Trim();
                                var auditInfo = auditSignature.Split("UTC");
                                var createdOrUpdatedAt = DateTime.ParseExact(
                                    auditInfo[0].Trim(),
                                    "yyyy-MM-dd HH:mm:ss",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
                                var author = auditInfo[1].Trim()
                                    .Replace("(", string.Empty)
                                    .Replace(")", string.Empty);
                                metadata.AddAudit(new Audit()
                                {
                                    Author = author,
                                    TimeStamp = createdOrUpdatedAt
                                });

                                if (line.StartsWith("-- Created At"))
                                {
                                    metadataDict["CreatedAt"] = true;
                                }
                            }
                            catch
                            {
                                if (line.StartsWith("-- Created At"))
                                {
                                    metadataDict["CreatedAt"] = false;
                                }
                                else
                                {
                                    throw new MalformedAuditEntryException();
                                }
                            }
                        }

                        // Extract Script ID
                        if (line.StartsWith("-- Script ID"))
                        {
                            try
                            {
                                var info = line.Split(':')[1].Trim();
                                metadata.Id = info;
                                metadataDict["ScriptId"] = true;
                            }
                            catch
                            {
                                metadataDict["ScriptId"] = false;
                            }
                        }

                        // Extract Migration Type
                        if (line.StartsWith("-- Migration Type"))
                        {
                            try
                            {
                                var info = line.Split(':')[1].Trim();
                                var migrationType = info.ToLower();
                                switch (migrationType)
                                {
                                    case "up":
                                        metadata.MigrationType = MigrationType.Up;
                                        break;
                                    case "down":
                                        metadata.MigrationType = MigrationType.Down;
                                        break;
                                    case "both":
                                        metadata.MigrationType = MigrationType.Both;
                                        break;
                                    default:
                                        metadata.MigrationType = MigrationType.None;
                                        break;
                                }
                                metadataDict["MigrationType"] = true;
                            }
                            catch
                            {
                                metadataDict["MigrationType"] = false;
                            }
                        }
                    }
                    else
                    {
                        break; // Exit after header
                    }
                }
            }

            // Check for missing required metadata fields
            foreach (var (property, present) in metadataDict)
            {
                if (!present)
                {
                    switch (property)
                    {
                        case "ScriptId":
                            throw new ScriptIdMissingException();
                        case "ScriptType":
                            throw new ScriptTypeMissingException();
                        case "Name":
                            throw new ScriptNameMissingException();
                        case "MigrationType":
                            throw new MigrationTypeMissingException();
                        case "CreatedAt":
                            throw new CreatedAtMissingException();
                    }
                }
            }

            return metadata;
        }
    }
}
