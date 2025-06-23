-- Script Type    : schema
-- Name           : 20250621143537125_schema_tblsubmissionentries.sql
-- Created At     : 2025-06-21 14:35:37 UTC (Arijit Roy)
-- Script ID      : 20250621143537125
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    IF Object_id('tblSubmissionEntries', 'U') IS NOT NULL
    BEGIN
        DROP TABLE tblSubmissionEntries;
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
