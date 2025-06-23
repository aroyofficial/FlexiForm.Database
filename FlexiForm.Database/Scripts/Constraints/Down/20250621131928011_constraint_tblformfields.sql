-- Script Type    : constraint
-- Name           : 20250621131928011_constraint_tblformfields.sql
-- Created At     : 2025-06-21 13:19:28 UTC (Arijit Roy)
-- Script ID      : 20250621131928011
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;
    
    IF OBJECT_ID('tblFormFields', 'U') IS NOT NULL
    BEGIN
        ALTER TABLE tblFormFields
        DROP CONSTRAINT
        IF EXISTS [FK_tblFormFields.UpdatedBy_tblUsers.RowId];

        ALTER TABLE tblFormFields
        DROP CONSTRAINT
        IF EXISTS [FK_tblFormFields.CreatedBy_tblUsers.RowId];

        ALTER TABLE tblFormFields
        DROP CONSTRAINT
        IF EXISTS [PK_tblFormFields.RowId];
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
