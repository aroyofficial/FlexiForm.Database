-- Script Type    : proc
-- Name           : 20250629102248824_proc_usp_getuser.sql
-- Created At     : 2025-06-29 10:22:48 UTC (Sushmita Das)
-- Script ID      : 20250629102248824
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE
    IF EXISTS usp_GetUser;

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
