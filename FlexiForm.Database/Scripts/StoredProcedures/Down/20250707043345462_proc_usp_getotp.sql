-- Script Type    : proc
-- Name           : 20250707043345462_proc_usp_getotp.sql
-- Created At     : 2025-07-07 04:33:45 UTC (Arijit Roy)
-- Script ID      : 20250707043345462
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE
    IF EXISTS usp_GetOTP;

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
