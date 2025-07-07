-- Script Type    : proc
-- Name           : 20250706044843282_proc_usp_resetpassword.sql
-- Created At     : 2025-07-06 04:48:43 UTC (Arijit roy)
-- Script ID      : 20250706044843282
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE
    IF EXISTS usp_ResetPassword;

    DECLARE @L_SQL NVARCHAR(MAX) = '
    CREATE PROCEDURE usp_ResetPassword @otp VARCHAR(64),
                             @userid UNIQUEIDENTIFIER,
                             @password NVARCHAR(255)
    WITH ENCRYPTION
    AS
    BEGIN
        BEGIN TRY
            DECLARE @L_OTP VARCHAR(64) = @otp;
            DECLARE @L_UserId UNIQUEIDENTIFIER = @userid;
            DECLARE @L_Password NVARCHAR(255) = @password;
            DECLARE @L_New TINYINT = 1;
            DECLARE @L_Consumed TINYINT = 2;
            DECLARE @L_CurrentUTC DATETIME = GETUTCDATE();
            DECLARE @L_Active TINYINT = 0;

            UPDATE tblOTP
            SET Status = @L_Consumed,
                ConsumedAt = @L_CurrentUTC,
                UpdatedAt = @L_CurrentUTC,
                UpdatedBy = @L_UserId
            WHERE CreatedBy = @L_UserId
            AND Value = @L_OTP
            AND @L_CurrentUTC BETWEEN GeneratedAt AND ExpiredAt
            AND Status = @L_New;

            DECLARE @L_AffectedRows INT = @@ROWCOUNT;

            IF @L_AffectedRows = 0
            BEGIN
                RAISERROR(''A valid OTP not found.'', 16, 1);
            END
            ELSE
            BEGIN
                UPDATE tblUsers
                SET Password = @L_Password,
                    UpdatedAt = @L_CurrentUTC
                WHERE RowId = @L_UserId
                AND Void = @L_Active;
            END
        END TRY
        BEGIN CATCH
            SET NOCOUNT OFF;
            DECLARE @L_ErrorMessage VARCHAR(512) = Error_message();
            RAISERROR(@L_ErrorMessage, 16, 1);
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
