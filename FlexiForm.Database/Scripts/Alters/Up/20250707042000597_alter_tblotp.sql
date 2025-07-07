-- Script Type    : alter
-- Name           : 20250707042000597_alter_tblotp.sql
-- Created At     : 2025-07-07 04:20:00 UTC (Arijit Roy)
-- Script ID      : 20250707042000597
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.columns
        WHERE Name = 'Salt'
        AND Object_ID = OBJECT_ID('dbo.tblOTP')
    )
    BEGIN
        ALTER TABLE tblOTP
        ADD Salt VARCHAR(32) NOT NULL;
    END


    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
