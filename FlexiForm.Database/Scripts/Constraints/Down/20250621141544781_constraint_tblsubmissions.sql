-- Script Type    : constraint
-- Name           : 20250621141544781_constraint_tblsubmissions.sql
-- Created At     : 2025-06-21 14:15:44 UTC (Arijit Roy)
-- Script ID      : 20250621141544781
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID('tblSubmissions', 'U') IS NOT NULL
    BEGIN
        ALTER TABLE tblSubmissions
        DROP CONSTRAINT
        IF EXISTS [FK_tblSubmissions.UpdatedBy_tblUsers.RowId];
    
        ALTER TABLE tblSubmissions
        DROP CONSTRAINT
        IF EXISTS [FK_tblSubmissions.CreatedBy_tblUsers.RowId];
    
        ALTER TABLE tblSubmissions
        DROP CONSTRAINT
        IF EXISTS [FK_tblSubmissions.SubmittedBy_tblUsers.RowId];
    
        ALTER TABLE tblSubmissions
        DROP CONSTRAINT
        IF EXISTS [FK_tblSubmissions.FormId_tblForms.RowId];

        ALTER TABLE tblSubmissions
        DROP CONSTRAINT
        IF EXISTS [PK_tblSubmissions.RowId];
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
