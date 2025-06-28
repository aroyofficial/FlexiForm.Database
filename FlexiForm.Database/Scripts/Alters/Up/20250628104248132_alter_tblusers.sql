-- Script Type    : alter
-- Name           : 20250628104248132_alter_tblusers.sql
-- Created At     : 2025-06-28 10:42:48 UTC (Sushmita Das)
-- Script ID      : 20250628104248132
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (
        SELECT 1 
        FROM INFORMATION_SCHEMA.COLUMNS 
        WHERE TABLE_NAME = 'tblUsers' 
          AND COLUMN_NAME = 'Void'
    )
    BEGIN
        ALTER TABLE tblUsers  
        ADD Void BIT
        NOT NULL;
    END
    
    IF EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'DF_tblUsers.Void' AND type = 'D'
    )
    BEGIN
        ALTER TABLE tblUsers
        ADD CONSTRAINT [DF_tblUsers.Void]
        DEFAULT 0 FOR Void;
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
