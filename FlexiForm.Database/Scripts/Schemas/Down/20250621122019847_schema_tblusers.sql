-- Script Type    : schema
-- Name           : 20250621122019847_schema_tblusers.sql
-- Created At     : 2025-06-21 12:20:19 UTC (Arijit Roy)
-- Script ID      : 20250621122019847
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    IF Object_id('tblUsers', 'U') IS NOT NULL
    BEGIN
      DROP TABLE tblUsers;
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
