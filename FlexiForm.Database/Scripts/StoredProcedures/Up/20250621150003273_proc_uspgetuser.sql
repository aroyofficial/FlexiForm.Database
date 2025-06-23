-- Script Type    : proc
-- Name           : 20250621150003273_proc_uspgetuser.sql
-- Created At     : 2025-06-21 15:00:03 UTC (Arijit Roy)
-- Script ID      : 20250621150003273
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @L_SQL NVARCHAR(MAX) = '
    CREATE PROCEDURE usp_GetUser @id    UNIQUEIDENTIFIER = NULL,
                                 @email VARCHAR(256)     = NULL
    WITH ENCRYPTION
    AS
      BEGIN
          BEGIN TRY
              SET NOCOUNT ON;

              DECLARE @L_Id UNIQUEIDENTIFIER = @id;
              DECLARE @L_Email VARCHAR(256) = @email;

              IF @L_Id IS NULL
                 AND @L_Email IS NULL
                BEGIN
                    RAISERROR(''Either @id or @email should be given.'',16,1);
                END

              SELECT TOP 1 Id,
                           RowId,
                           FirstName,
                           LastName,
                           Email,
                           [Password],
                           Gender,
                           CreatedAt,
                           UpdatedAt
              FROM   tblUsers
              WHERE  ( @L_Id IS NOT NULL
                       AND RowId = @L_Id )
                      OR ( @L_Email IS NOT NULL
                           AND Email = @L_Email );

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
