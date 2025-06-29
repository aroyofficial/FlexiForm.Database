-- Script Type    : alter
-- Name           : 20250621123933719_alter_tblforms.sql
-- Created At     : 2025-06-21 12:39:33 UTC (Arijit Roy)
-- Script ID      : 20250621123933719
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'PK_tblForms.RowId'
        AND type = 'PK'
    )
    BEGIN
        ALTER TABLE tblForms
        ADD CONSTRAINT [PK_tblForms.RowId]
        PRIMARY KEY (RowId);
    END

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblForms.CreatedBy_tblUsers.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblForms
        ADD CONSTRAINT [FK_tblForms.CreatedBy_tblUsers.RowId]
        FOREIGN KEY (CreatedBy)
        REFERENCES tblUsers(RowId);
    END

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblForms.UpdatedBy_tblUsers.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblForms
        ADD CONSTRAINT [FK_tblForms.UpdatedBy_tblUsers.RowId]
        FOREIGN KEY (UpdatedBy)
        REFERENCES tblUsers(RowId);
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
