-- Script Type    : schema
-- Name           : 20250621141228736_schema_tblsubmissions.sql
-- Created At     : 2025-06-21 14:12:28 UTC (Arijit Roy)
-- Script ID      : 20250621141228736
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    IF Object_id('tblSubmissions', 'U') IS NOT NULL
    BEGIN
        DROP TABLE tblSubmissions;
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
