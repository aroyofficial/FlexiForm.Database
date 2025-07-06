-- Script Type    : proc
-- Name           : 20250704191807128_proc_usp_useotp.sql
-- Created At     : 2025-07-04 19:18:07 UTC (Arijit Roy)
-- Script ID      : 20250704191807128
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE
    IF EXISTS usp_UseOTP;

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
