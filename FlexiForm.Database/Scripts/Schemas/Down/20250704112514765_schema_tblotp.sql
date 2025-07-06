-- Script Type    : schema
-- Name           : 20250704112514765_schema_tblotp.sql
-- Created At     : 2025-07-04 11:25:14 UTC (Sushmita Das)
-- Script ID      : 20250704112514765
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    IF Object_id('tblOTP', 'U') IS NOT NULL
    BEGIN
      DROP TABLE tblOTP;
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
