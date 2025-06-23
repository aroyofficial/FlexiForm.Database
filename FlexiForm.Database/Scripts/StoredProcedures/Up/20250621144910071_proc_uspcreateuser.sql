-- Script Type    : proc
-- Name           : 20250621144910071_proc_uspcreateuser.sql
-- Created At     : 2025-06-21 14:49:10 UTC (Arijit Roy)
-- Script ID      : 20250621144910071
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @L_SQL NVARCHAR(MAX) = '
    CREATE PROCEDURE usp_CreateUser @firstname VARCHAR(128),
                                    @lastname  VARCHAR(128),
                                    @email     VARCHAR(256),
                                    @password  VARCHAR(64),
                                    @gender    TINYINT
    WITH ENCRYPTION
    AS
      BEGIN
          BEGIN TRY
              SET NOCOUNT ON;

              DECLARE @L_FirstName VARCHAR(128) = @firstname;
              DECLARE @L_LastName VARCHAR(128) = @lastname;
              DECLARE @L_Email VARCHAR(256) = @email;
              DECLARE @L_Password VARCHAR(64) = @password;
              DECLARE @L_Gender TINYINT = @gender;

              IF @L_FirstName IS NULL
                BEGIN
                    RAISERROR(''@firstname must be given.'',16,1);
                END

              IF @L_LastName IS NULL
                BEGIN
                    RAISERROR(''@lastname must be given'',16,1);
                END

              IF @L_Email IS NULL
                BEGIN
                    RAISERROR(''@email must be given'',16,1);
                END

              IF @L_Password IS NULL
                BEGIN
                    RAISERROR(''@password must be given'',16,1);
                END

              IF @L_Gender IS NULL
                BEGIN
                    RAISERROR(''@gender must be given'',16,1);
                END

              INSERT INTO tblUsers
                          (FirstName,
                           LastName,
                           Email,
                           [Password],
                           Gender)
              VALUES      (@L_FirstName,
                           @L_LastName,
                           @L_Email,
                           @L_Password,
                           @L_Gender);

              SELECT Scope_identity() AS Id;

              SET NOCOUNT OFF;

              RETURN;
          END TRY
          BEGIN CATCH
              SET NOCOUNT OFF;
              DECLARE @L_ErrorMessage VARCHAR(512) = Error_message();
              RAISERROR(@L_ErrorMessage,16,1);
              RETURN;
          END CATCH
      END';

      EXEC sp_executesql @L_SQL;

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
