-- Script Type    : alert
-- Name           : 20250621122508435_alert_tblusers.sql
-- Created At     : 2025-06-21 12:25:08 UTC (Arijit Roy)
-- Script ID      : 20250621122508435
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    ALTER TABLE tblUsers
    ADD CONSTRAINT [PK_tblUsers.RowId]
    PRIMARY KEY (RowId);

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
