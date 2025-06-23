-- Script Type    : constraint
-- Name           : 20250621143606196_constraint_tblsubmissionentries.sql
-- Created At     : 2025-06-21 14:36:06 UTC (Arijit Roy)
-- Script ID      : 20250621143606196
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    ALTER TABLE tblSubmissionEntries
    ADD CONSTRAINT [PK_tblSubmissionEntries.RowId]
    PRIMARY KEY (RowId);

    ALTER TABLE tblSubmissionEntries
    ADD CONSTRAINT [FK_tblSubmissionEntries.SubmissionId_tblSubmissions.RowId]
    FOREIGN KEY (SubmissionId)
    REFERENCES tblSubmissions(RowId);
    
    ALTER TABLE tblSubmissionEntries
    ADD CONSTRAINT [FK_tblSubmissionEntries.FormFieldMappingId_tblFormFieldMappings.RowId]
    FOREIGN KEY (FormFieldMappingId)
    REFERENCES tblFormFieldMappings(RowId);
    
    ALTER TABLE tblSubmissionEntries
    ADD CONSTRAINT [FK_tblSubmissionEntries.CreatedBy_tblUsers.RowId]
    FOREIGN KEY (CreatedBy)
    REFERENCES tblUsers(RowId);
    
    ALTER TABLE tblSubmissionEntries
    ADD CONSTRAINT [FK_tblSubmissionEntries.UpdatedBy_tblUsers.RowId]
    FOREIGN KEY (UpdatedBy)
    REFERENCES tblUsers(RowId);

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
