-- Script Type    : index
-- Name           : 20250704200959160_index_tblotp.sql
-- Created At     : 2025-07-04 20:09:59 UTC (Sushmita Das)
-- Script ID      : 20250704200959160
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.indexes
        WHERE name = 'IX_tblOTP.Value'
    )
    BEGIN
        CREATE NONCLUSTERED INDEX [IX_tblOTP.Value]
        ON tblOTP (Value);
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
