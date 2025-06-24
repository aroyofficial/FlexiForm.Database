-- Script Type    : alter
-- Name           : 20250621135722549_alter_tblformfieldmappings.sql
-- Created At     : 2025-06-21 13:57:22 UTC (Arijit Roy)
-- Script ID      : 20250621135722549
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID('tblFormFieldMappings', 'U') IS NOT NULL
    BEGIN
        ALTER TABLE tblFormFieldMappings
        DROP CONSTRAINT
        IF EXISTS [FK_tblFormFieldMappings.UpdatedBy_tblUsers.RowId];
    
        ALTER TABLE tblFormFieldMappings
        DROP CONSTRAINT
        IF EXISTS [FK_tblFormFieldMappings.CreatedBy_tblUsers.RowId];
    
        ALTER TABLE tblFormFieldMappings
        DROP CONSTRAINT
        IF EXISTS [FK_tblFormFieldMappings.FormFieldId_tblFormFields.RowId];
    
        ALTER TABLE tblFormFieldMappings
        DROP CONSTRAINT
        IF EXISTS [FK_tblFormFieldMappings.FormId_tblForms.RowId];
    
        ALTER TABLE tblFormFieldMappings
        DROP CONSTRAINT
        IF EXISTS [PK_tblFormFieldMappings.RowId];
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
