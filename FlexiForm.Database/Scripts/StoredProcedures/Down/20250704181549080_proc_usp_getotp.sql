-- Script Type    : proc
-- Name           : 20250704181549080_proc_usp_getotp.sql
-- Created At     : 2025-07-04 18:15:49 UTC (Arijit Roy)
-- Script ID      : 20250704181549080
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
