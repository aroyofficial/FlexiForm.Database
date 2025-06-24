-- Script Type    : alter
-- Name           : 20250621143606196_alter_tblsubmissionentries.sql
-- Created At     : 2025-06-21 14:36:06 UTC (Arijit Roy)
-- Script ID      : 20250621143606196
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;
    
    IF OBJECT_ID('tblSubmissionEntries', 'U') IS NOT NULL
    BEGIN
        ALTER TABLE tblSubmissionEntries
        DROP CONSTRAINT
        IF EXISTS [FK_tblSubmissionEntries.UpdatedBy_tblUsers.RowId];
    
        ALTER TABLE tblSubmissionEntries
        DROP CONSTRAINT
        IF EXISTS [FK_tblSubmissionEntries.CreatedBy_tblUsers.RowId];
    
        ALTER TABLE tblSubmissionEntries
        DROP CONSTRAINT
        IF EXISTS [FK_tblSubmissionEntries.FormFieldMappingId_tblFormFieldMappings.RowId];
    
        ALTER TABLE tblSubmissionEntries
        DROP CONSTRAINT
        IF EXISTS [FK_tblSubmissionEntries.SubmissionId_tblSubmissions.RowId];
    
        ALTER TABLE tblSubmissionEntries
        DROP CONSTRAINT
        IF EXISTS [PK_tblSubmissionEntries.RowId];
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
