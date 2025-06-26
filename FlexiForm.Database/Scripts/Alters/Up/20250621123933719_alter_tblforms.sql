-- Script Type    : alter
-- Name           : 20250621123933719_alert_tblforms.sql
-- Created At     : 2025-06-21 12:39:33 UTC (Arijit Roy)
-- Script ID      : 20250621123933719
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    ALTER TABLE tblForms
    ADD CONSTRAINT [PK_tblForms.RowId]
    PRIMARY KEY (RowId);

    ALTER TABLE tblForms
    ADD CONSTRAINT [FK_tblForms.CreatedBy_tblUsers.RowId]
    FOREIGN KEY (CreatedBy)
    REFERENCES tblUsers(RowId);

    ALTER TABLE tblForms
    ADD CONSTRAINT [FK_tblForms.UpdatedBy_tblUsers.RowId]
    FOREIGN KEY (UpdatedBy)
    REFERENCES tblUsers(RowId);

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
