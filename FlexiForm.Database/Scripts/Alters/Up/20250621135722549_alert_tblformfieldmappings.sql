-- Script Type    : alert
-- Name           : 20250621135722549_alert_tblformfieldmappings.sql
-- Created At     : 2025-06-21 13:57:22 UTC (Arijit Roy)
-- Script ID      : 20250621135722549
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    ALTER TABLE tblFormFieldMappings
    ADD CONSTRAINT [PK_tblFormFieldMappings.RowId]
    PRIMARY KEY (RowId);

    ALTER TABLE tblFormFieldMappings
    ADD CONSTRAINT [FK_tblFormFieldMappings.FormId_tblForms.RowId]
    FOREIGN KEY (FormId)
    REFERENCES tblForms(RowId);

    ALTER TABLE tblFormFieldMappings
    ADD CONSTRAINT [FK_tblFormFieldMappings.FormFieldId_tblFormFields.RowId]
    FOREIGN KEY (FormFieldId)
    REFERENCES tblFormFields(RowId);

    ALTER TABLE tblFormFieldMappings
    ADD CONSTRAINT [FK_tblFormFieldMappings.CreatedBy_tblUsers.RowId]
    FOREIGN KEY (CreatedBy)
    REFERENCES tblUsers(RowId);

    ALTER TABLE tblFormFieldMappings
    ADD CONSTRAINT [FK_tblFormFieldMappings.UpdatedBy_tblUsers.RowId]
    FOREIGN KEY (UpdatedBy)
    REFERENCES tblUsers(RowId);

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
