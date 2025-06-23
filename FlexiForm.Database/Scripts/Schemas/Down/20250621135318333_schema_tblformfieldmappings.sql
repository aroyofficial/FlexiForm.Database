-- Script Type    : schema
-- Name           : 20250621135318333_schema_tblformfieldmappings.sql
-- Created At     : 2025-06-21 13:53:18 UTC (Arijit Roy)
-- Script ID      : 20250621135318333
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    IF Object_id('tblFormFieldMappings', 'U') IS NOT NULL
    BEGIN
        DROP TABLE tblFormFieldMappings;
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
