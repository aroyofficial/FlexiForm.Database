using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Exception thrown when a script file expected to exist on disk cannot be found at the specified path.
    /// </summary>
    public class ScriptFileNotFoundException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptFileNotFoundException"/> class with the path of the missing file.
        /// </summary>
        /// <param name="path">The absolute path to the script file that could not be located.</param>
        public ScriptFileNotFoundException(string path)
            : base(ErrorCode.ScriptFileNotFound, $"Script file not found at path: {path}") { }
    }
}
