-- Script Type    : alter
-- Name           : 20250704131947212_alter_tblotp.sql
-- Created At     : 2025-07-04 13:19:47 UTC (Sushmita Das)
-- Script ID      : 20250704131947212
-- Migration Type : Down

BEGIN TRY
    BEGIN TRANSACTION;

    ALTER TABLE tblOTP
    DROP CONSTRAINT 
    IF EXISTS [FK_tblOTP.UpdatedBy_tblUsers.RowId];
       
    ALTER TABLE tblOTP
    DROP CONSTRAINT 
    IF EXISTS [FK_tblOTP.CreatedBy_tblUsers.RowId];
    
    ALTER TABLE tblOTP
    DROP CONSTRAINT 
    IF EXISTS [DF_tblOTP.CreatedAt];
    
    ALTER TABLE tblOTP
    DROP CONSTRAINT 
    IF EXISTS [DF_tblOTP.Status];
       
    ALTER TABLE tblOTP
    DROP CONSTRAINT 
    IF EXISTS [PK_tblOTP.RowId];

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
