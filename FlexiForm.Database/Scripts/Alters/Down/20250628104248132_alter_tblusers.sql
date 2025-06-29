-- Script Type    : alter
-- Name           : 20250628104248132_alter_tblusers.sql
-- Created At     : 2025-06-28 10:42:48 UTC (Sushmita Das)
-- Script ID      : 20250628104248132
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    IF EXISTS (
        SELECT 1 
        FROM INFORMATION_SCHEMA.COLUMNS 
        WHERE TABLE_NAME = 'tblUsers' 
          AND COLUMN_NAME = 'Void'
    )
    BEGIN
        ALTER TABLE tblUsers
        DROP CONSTRAINT IF EXISTS [DF_tblUsers.Void];

        ALTER TABLE tblUsers
        DROP COLUMN Void;
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
