-- Script Type    : alter
-- Name           : 20250624054059670_alter_tblusers.sql
-- Created At     : 2025-06-24 05:40:59 UTC (Arijit Roy)
-- Script ID      : 20250624054059670
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    IF OBJECT_ID('tblUsers', 'U') IS NOT NULL
    BEGIN
        IF EXISTS (
            SELECT 1
            FROM INFORMATION_SCHEMA.COLUMNS
            WHERE TABLE_NAME = 'tblUsers' 
              AND COLUMN_NAME = 'Password'
              AND DATA_TYPE = 'VARCHAR'
        )
        BEGIN
            ALTER TABLE tblUsers
            ALTER COLUMN [Password]
            VARCHAR(64) NOT NULL;
        END
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
