-- Script Type    : constraint
-- Name           : 20250621131928011_constraint_tblformfields.sql
-- Created At     : 2025-06-21 13:19:28 UTC (Arijit Roy)
-- Script ID      : 20250621131928011
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    ALTER TABLE tblFormFields
    ADD CONSTRAINT [PK_tblFormFields.RowId]
    PRIMARY KEY (RowId);

    ALTER TABLE tblFormFields
    ADD CONSTRAINT [FK_tblFormFields.CreatedBy_tblUsers.RowId]
    FOREIGN KEY (CreatedBy)
    REFERENCES tblUsers(RowId);

    ALTER TABLE tblFormFields
    ADD CONSTRAINT [FK_tblFormFields.UpdatedBy_tblUsers.RowId]
    FOREIGN KEY (UpdatedBy)
    REFERENCES tblUsers(RowId);

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
