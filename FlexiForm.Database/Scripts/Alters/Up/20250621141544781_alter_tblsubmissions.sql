-- Script Type    : alter
-- Name           : 20250621141544781_alert_tblsubmissions.sql
-- Created At     : 2025-06-21 14:15:44 UTC (Arijit Roy)
-- Script ID      : 20250621141544781
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    ALTER TABLE tblSubmissions
    ADD CONSTRAINT [PK_tblSubmissions.RowId]
    PRIMARY KEY (RowId);

    ALTER TABLE tblSubmissions
    ADD CONSTRAINT [FK_tblSubmissions.FormId_tblForms.RowId]
    FOREIGN KEY (FormId)
    REFERENCES tblForms(RowId);

    ALTER TABLE tblSubmissions
    ADD CONSTRAINT [FK_tblSubmissions.SubmittedBy_tblUsers.RowId]
    FOREIGN KEY (SubmittedBy)
    REFERENCES tblUsers(RowId);

    ALTER TABLE tblSubmissions
    ADD CONSTRAINT [FK_tblSubmissions.CreatedBy_tblUsers.RowId]
    FOREIGN KEY (CreatedBy)
    REFERENCES tblUsers(RowId);
    
    ALTER TABLE tblSubmissions
    ADD CONSTRAINT [FK_tblSubmissions.UpdatedBy_tblUsers.RowId]
    FOREIGN KEY (UpdatedBy)
    REFERENCES tblUsers(RowId);

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
