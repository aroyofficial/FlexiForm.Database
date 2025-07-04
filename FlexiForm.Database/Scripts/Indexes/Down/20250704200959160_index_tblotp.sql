-- Script Type    : index
-- Name           : 20250704200959160_index_tblotp.sql
-- Created At     : 2025-07-04 20:09:59 UTC (Sushmita Das)
-- Script ID      : 20250704200959160
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    DROP INDEX 
    IF EXISTS dbo.tblOTP.[IX_tblOTP.Value];

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
