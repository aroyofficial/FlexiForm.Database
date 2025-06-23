using FlexiForm.Database.Enumerations;

namespace FlexiForm.Database.Exceptions
{
    /// <summary>
    /// Represents an exception thrown when the specified migration scripts folder
    /// does not exist or cannot be accessed.
    /// </summary>
    public class MigrationFolderNotFoundException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationFolderNotFoundException"/> class with the specified folder path.
        /// </summary>
        /// <param name="folderPath">The path of the folder that was not found or accessible.</param>
        public MigrationFolderNotFoundException(string folderPath)
            : base(
                ErrorCode.ScriptFolderNotFound,
                $"Migration scripts folder not found or inaccessible: {folderPath}")
        {
        }
    }
}
