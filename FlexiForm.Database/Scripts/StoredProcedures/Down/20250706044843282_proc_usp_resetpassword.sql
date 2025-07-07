-- Script Type    : proc
-- Name           : 20250706044843282_proc_usp_resetpassword.sql
-- Created At     : 2025-07-06 04:48:43 UTC (Arijit Roy)
-- Script ID      : 20250706044843282
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE
    IF EXISTS usp_ResetPassword;

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
