-- Script Type    : alter
-- Name           : 20250621122508435_alter_tblusers.sql
-- Created At     : 2025-06-21 12:25:08 UTC (Arijit Roy)
-- Script ID      : 20250621122508435
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID('tblUsers', 'U') IS NOT NULL
    BEGIN
        ALTER TABLE tblUsers
        DROP CONSTRAINT
        IF EXISTS [PK_tblUsers.RowId];
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
