-- Script Type    : proc
-- Name           : 20250629102950154_proc_usp_updateuser.sql
-- Created At     : 2025-06-29 10:29:50 UTC (Sushmita Das)
-- Script ID      : 20250629102950154
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE 
    IF EXISTS usp_UpdateUser;

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
                DECLARE @L_Active BIT = 0;

                UPDATE tblUsers
                SET    FirstName = @L_FirstName,
                        LastName = @L_LastName,
                        Gender = @L_Gender,
                        UpdatedAt = GETUTCDATE()
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

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH

