-- Script Type    : alter
-- Name           : 20250621135722549_alter_tblformfieldmappings.sql
-- Created At     : 2025-06-21 13:57:22 UTC (Arijit Roy)
-- Script ID      : 20250621135722549
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'PK_tblFormFieldMappings.RowId'
        AND type = 'PK'
    )
    BEGIN
        ALTER TABLE tblFormFieldMappings
        ADD CONSTRAINT [PK_tblFormFieldMappings.RowId]
        PRIMARY KEY (RowId);
    END

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblFormFieldMappings.FormId_tblForms.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblFormFieldMappings
        ADD CONSTRAINT [FK_tblFormFieldMappings.FormId_tblForms.RowId]
        FOREIGN KEY (FormId)
        REFERENCES tblForms(RowId);
    END

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblFormFieldMappings.FormFieldId_tblFormFields.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblFormFieldMappings
        ADD CONSTRAINT [FK_tblFormFieldMappings.FormFieldId_tblFormFields.RowId]
        FOREIGN KEY (FormFieldId)
        REFERENCES tblFormFields(RowId);
    END

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblFormFieldMappings.CreatedBy_tblUsers.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblFormFieldMappings
        ADD CONSTRAINT [FK_tblFormFieldMappings.CreatedBy_tblUsers.RowId]
        FOREIGN KEY (CreatedBy)
        REFERENCES tblUsers(RowId);
    END

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblFormFieldMappings.UpdatedBy_tblUsers.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblFormFieldMappings
        ADD CONSTRAINT [FK_tblFormFieldMappings.UpdatedBy_tblUsers.RowId]
        FOREIGN KEY (UpdatedBy)
        REFERENCES tblUsers(RowId);
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
