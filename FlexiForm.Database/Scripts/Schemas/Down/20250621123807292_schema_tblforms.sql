-- Script Type    : schema
-- Name           : 20250621123807292_schema_tblforms.sql
-- Created At     : 2025-06-21 12:38:07 UTC (Arijit Roy)
-- Script ID      : 20250621123807292
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    IF Object_id('tblForms', 'U') IS NOT NULL
    BEGIN
        DROP TABLE tblForms;
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
