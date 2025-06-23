-- Script Type   : constraint
-- Name          : 20250621123933719_constraint_tblforms.sql
-- Created At    : 2025-06-21 12:39:33 UTC (Arijit Roy)
-- Script ID     : 20250621123933719
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;
    
    IF OBJECT_ID('tblForms', 'U') IS NOT NULL
    BEGIN
        ALTER TABLE tblForms
        DROP CONSTRAINT
        IF EXISTS [FK_tblForms.UpdatedBy_tblUsers.RowId];
    
        ALTER TABLE tblForms
        DROP CONSTRAINT
        IF EXISTS [FK_tblForms.CreatedBy_tblUsers.RowId];

        ALTER TABLE tblForms
        DROP CONSTRAINT
        IF EXISTS [PK_tblForms.RowId];
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
