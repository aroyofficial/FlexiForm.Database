-- Script Type    : proc
-- Name           : 20250707043600614_proc_usp_addotp.sql
-- Created At     : 2025-07-07 04:36:00 UTC (Arijit Roy)
-- Script ID      : 20250707043600614
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE
    IF EXISTS usp_AddOTP;

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
