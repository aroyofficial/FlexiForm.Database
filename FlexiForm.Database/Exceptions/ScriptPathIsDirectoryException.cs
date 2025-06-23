using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Exception thrown when a script path points to a directory instead of an expected file.
    /// Typically occurs during validation of script input paths.
    /// </summary>
    public class ScriptPathIsDirectoryException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptPathIsDirectoryException"/> class
        /// with the specified directory path that was incorrectly used as a file.
        /// </summary>
        /// <param name="path">The path that refers to a directory instead of a file.</param>
        public ScriptPathIsDirectoryException(string path)
            : base(ErrorCode.ScriptPathIsDirectory, $"Expected a file but found a directory at: {path}") { }
    }
}