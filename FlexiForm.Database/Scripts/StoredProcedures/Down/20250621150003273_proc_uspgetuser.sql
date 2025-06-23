-- Script Type    : proc
-- Name           : 20250621150003273_proc_uspgetuser.sql
-- Created At     : 2025-06-21 15:00:03 UTC (Arijit Roy)
-- Script ID      : 20250621150003273
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
