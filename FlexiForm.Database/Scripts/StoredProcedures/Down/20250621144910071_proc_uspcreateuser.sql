-- Script Type    : proc
-- Name           : 20250621144910071_proc_uspcreateuser.sql
-- Created At     : 2025-06-21 14:49:10 UTC (Arijit Roy)
-- Script ID      : 20250621144910071
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    DROP PROCEDURE
    IF EXISTS usp_CreateUser;

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
