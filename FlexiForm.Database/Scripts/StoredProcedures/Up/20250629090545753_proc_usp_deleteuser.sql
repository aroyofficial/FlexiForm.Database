-- Script Type    : proc
-- Name           : 20250629090545753_proc_usp_deleteuser.sql
-- Created At     : 2025-06-29 09:05:45 UTC (Sushmita Das)
-- Script ID      : 20250629090545753
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE
    IF EXISTS usp_DeleteUser;

    DECLARE @L_SQL NVARCHAR(MAX) = '
    CREATE PROCEDURE usp_DeleteUser @id UNIQUEIDENTIFIER,
    WITH ENCRYPTION
    AS
      BEGIN
          BEGIN TRY
              SET NOCOUNT ON;
              
              DECLARE @L_Id UNIQUEIDENTIFIER = @id;
              DECLARE @L_Active BIT = 0;
              DECLARE @L_Deleted BIT = 1;

              UPDATE tblUsers
              SET Void = @L_Deleted
              WHERE  RowId = @L_Id 
              AND Void = @L_Active;

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

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
