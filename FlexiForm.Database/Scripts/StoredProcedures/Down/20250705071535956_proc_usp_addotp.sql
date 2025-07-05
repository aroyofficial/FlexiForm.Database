-- Script Type    : proc
-- Name           : 20250705071535956_proc_usp_addotp.sql
-- Created At     : 2025-07-05 07:15:35 UTC (Sushmita Das)
-- Script ID      : 20250705071535956
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
