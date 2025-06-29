-- Script Type    : alter
-- Name           : 20250621143606196_alter_tblsubmissionentries.sql
-- Created At     : 2025-06-21 14:36:06 UTC (Arijit Roy)
-- Script ID      : 20250621143606196
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'PK_tblSubmissionEntries.RowId'
        AND type = 'PK'
    )
    BEGIN
        ALTER TABLE tblSubmissionEntries
        ADD CONSTRAINT [PK_tblSubmissionEntries.RowId]
        PRIMARY KEY (RowId);
    END

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblSubmissionEntries.SubmissionId_tblSubmissions.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblSubmissionEntries
        ADD CONSTRAINT [FK_tblSubmissionEntries.SubmissionId_tblSubmissions.RowId]
        FOREIGN KEY (SubmissionId)
        REFERENCES tblSubmissions(RowId);
    END
    
    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblSubmissionEntries.SubmissionId_tblSubmissions.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblSubmissionEntries
        ADD CONSTRAINT [FK_tblSubmissionEntries.FormFieldMappingId_tblFormFieldMappings.RowId]
        FOREIGN KEY (FormFieldMappingId)
        REFERENCES tblFormFieldMappings(RowId);
    END
    
    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblSubmissionEntries.CreatedBy_tblUsers.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblSubmissionEntries
        ADD CONSTRAINT [FK_tblSubmissionEntries.CreatedBy_tblUsers.RowId]
        FOREIGN KEY (CreatedBy)
        REFERENCES tblUsers(RowId);
    END
    
    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblSubmissionEntries.UpdatedBy_tblUsers.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblSubmissionEntries
        ADD CONSTRAINT [FK_tblSubmissionEntries.UpdatedBy_tblUsers.RowId]
        FOREIGN KEY (UpdatedBy)
        REFERENCES tblUsers(RowId);
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
