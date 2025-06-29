-- Script Type    : proc
-- Name           : 20250621151252057_proc_uspupdateuser.sql
-- Created At     : 2025-06-21 15:12:52 UTC (Arijit Roy)
-- Script ID      : 20250621151252057
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'usp_UpdateUser'
        AND type = 'P'
    )
    BEGIN
        DECLARE @L_SQL NVARCHAR(MAX) = '
        CREATE PROCEDURE usp_UpdateUser @id        UNIQUEIDENTIFIER,
                                        @firstname VARCHAR(128),
                                        @lastname  VARCHAR(128),
                                        @gender    TINYINT
        WITH ENCRYPTION
        AS
          BEGIN
              BEGIN TRY
                  SET NOCOUNT ON;

                  DECLARE @L_Id UNIQUEIDENTIFIER = @id;
                  DECLARE @L_FirstName VARCHAR(128) = @firstname;
                  DECLARE @L_LastName VARCHAR(128) = @lastname;
                  DECLARE @L_Gender TINYINT = @gender;

                  UPDATE tblUsers
                  SET    FirstName = @L_FirstName,
                         LastName = @L_LastName,
                         Gender = @L_Gender,
                         UpdatedAt = GETUTCDATE()
                  WHERE  RowId = @L_Id;

                  SET NOCOUNT OFF;

                  RETURN;
              END TRY
              BEGIN CATCH
                  SET NOCOUNT OFF;
                  DECLARE @L_ErrorMessage VARCHAR(512) = Error_message();
                  RAISERROR(@L_ErrorMessage,16,1);
                  THROW
              END CATCH
          END';

          EXEC sp_executesql @L_SQL;
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
