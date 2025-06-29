-- Script Type    : proc
-- Name           : 20250629090545753_proc_usp_deleteuser.sql
-- Created At     : 2025-06-29 09:05:45 UTC (Sushmita Das)
-- Script ID      : 20250629090545753
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE
    IF EXISTS usp_DeleteUser;

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
