-- Script Type    : proc
-- Name           : 20250705071535956_proc_usp_addotp.sql
-- Created At     : 2025-07-05 07:15:35 UTC (Sushmita Das)
-- Script ID      : 20250705071535956
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

        DROP PROCEDURE
        IF EXISTS usp_AddOTP;

        DECLARE @L_SQL NVARCHAR(MAX) = '
        CREATE PROCEDURE usp_AddOTP     @value        VARCHAR(64),
                                        @generatedat  DATETIME,
                                        @expiredat    DATETIME,
                                        @createdby    UNIQUEIDENTIFIER                                
        WITH ENCRYPTION
        AS
          BEGIN
              BEGIN TRY
                  SET NOCOUNT ON;

                  DECLARE @L_Value VARCHAR(64) = @value;
                  DECLARE @L_GeneratedAt DATETIME = @generatedat;
                  DECLARE @L_ExpiredAt DATETIME = @expiredat;
                  DECLARE @L_CreatedBy UNIQUEIDENTIFIER   = @createdby;
                  DECLARE @L_Expired TINYINT = 0;
                  DECLARE @L_New TINYINT = 1;

                  IF @L_Value IS NOT NULL
                  BEGIN
                      IF @L_GeneratedAt >= @L_ExpiredAt
                      BEGIN
                          RAISERROR(''@generatedat is beyond @expiredat'', 16, 1);
                      END

                      UPDATE tblOTP
                      SET Status    = @L_Expired,
                          UpdatedAt = GETUTCDATE(),
                          UpdatedBy = @L_CreatedBy
                      WHERE CreatedBy = @L_CreatedBy
                      AND Status = @L_New;

                     INSERT INTO tblOTP (
                        Value,
                        Status,
                        GeneratedAt,
                        ExpiredAt,
                        CreatedBy)
                     VALUES (
                        @L_Value,
                        @L_New,
                        @L_GeneratedAt,
                        @L_ExpiredAt,
                        @L_CreatedBy
                     );
                  END
                  ELSE
                  BEGIN
                      RAISERROR(''@value cannot be null'', 16, 1);
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
