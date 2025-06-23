-- Script Type    : schema
-- Name           : 20250621125537401_schema_tblformfields.sql
-- Created At     : 2025-06-21 12:55:37 UTC (Arijit Roy)
-- Script ID      : 20250621125537401
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    IF Object_id('tblFormFields', 'U') IS NOT NULL
    BEGIN
        DROP TABLE tblFormFields;
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
