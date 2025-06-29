-- Script Type    : alter
-- Name           : 20250621131928011_alter_tblformfields.sql
-- Created At     : 2025-06-21 13:19:28 UTC (Arijit Roy)
-- Script ID      : 20250621131928011
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS(
        SELECT 1
        FROM sys.objects
        WHERE name = 'PK_tblFormFields.RowId'
        AND type = 'PK'
    )
    BEGIN
        ALTER TABLE tblFormFields
        ADD CONSTRAINT [PK_tblFormFields.RowId]
        PRIMARY KEY (RowId);
    END

    IF NOT EXISTS(
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblFormFields.CreatedBy_tblUsers.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblFormFields
        ADD CONSTRAINT [FK_tblFormFields.CreatedBy_tblUsers.RowId]
        FOREIGN KEY (CreatedBy)
        REFERENCES tblUsers(RowId);
    END

    IF NOT EXISTS(
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblFormFields.UpdatedBy_tblUsers.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblFormFields
        ADD CONSTRAINT [FK_tblFormFields.UpdatedBy_tblUsers.RowId]
        FOREIGN KEY (UpdatedBy)
        REFERENCES tblUsers(RowId);
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
