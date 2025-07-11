﻿-- Script Type    : proc
-- Name           : 20250707043345462_proc_usp_getotp.sql
-- Created At     : 2025-07-07 04:33:45 UTC (Arijit Roy)
-- Script ID      : 20250707043345462
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE
    IF EXISTS usp_GetOTP;

    DECLARE @L_SQL NVARCHAR(MAX) = '
    CREATE PROCEDURE usp_GetOTP @userid UNIQUEIDENTIFIER
    WITH ENCRYPTION
    AS
    BEGIN
        BEGIN TRY
            SET NOCOUNT ON;

            DECLARE @L_UserId UNIQUEIDENTIFIER = @userid;
            DECLARE @L_CurrentUTC DATETIME = GETUTCDATE();
            DECLARE @L_New TINYINT = 1;
            DECLARE @L_Active TINYINT = 0;

            IF EXISTS (
                SELECT 1
                FROM tblUsers
                WHERE RowId = @L_UserId
                AND Void = @L_Active
            )
            BEGIN
                SELECT TOP 1 Id,
                             RowId,
                             Value,
                             Salt,
                             Status,
                             GeneratedAt,
                             ExpiredAt,
                             ConsumedAt,
                             CreatedAt,
                             UpdatedAt,
                             CreatedBy,
                             UpdatedBy
                FROM tblOTP
                WHERE CreatedBy = @L_UserId
                AND @L_CurrentUTC BETWEEN CreatedAt AND ExpiredAt
                AND Status = @L_New;
            END
            ELSE
            BEGIN
                RAISERROR(''User with the given @userid does not exist'', 16, 1)
            END

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
