-- Script Type    : proc
-- Name           : 20250621151252057_proc_uspupdateuser.sql
-- Created At     : 2025-06-21 15:12:52 UTC (Arijit Roy)
-- Script ID      : 20250621151252057
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

