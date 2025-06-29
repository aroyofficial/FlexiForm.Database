-- Script Type    : proc
-- Name           : 20250629102950154_proc_usp_updateuser.sql
-- Created At     : 2025-06-29 10:29:50 UTC (Sushmita Das)
-- Script ID      : 20250629102950154
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE 
    IF EXISTS usp_UpdateUser;

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
