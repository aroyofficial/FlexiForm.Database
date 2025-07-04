-- Script Type    : alter
-- Name           : 20250704131947212_alter_tblotp.sql
-- Created At     : 2025-07-04 13:19:47 UTC (Sushmita Das)
-- Script ID      : 20250704131947212
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'PK_tblOTP.RowId'
        AND type = 'PK'
    )
    BEGIN
        ALTER TABLE tblForms
        ADD CONSTRAINT [PK_tblOTP.RowId]
        PRIMARY KEY (RowId);
    END

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'DF_tblOTP.Status'
        AND type = 'D'
    )
    BEGIN
        ALTER TABLE tblForms
        ADD CONSTRAINT [DF_tblOTP.Status]
        DEFAULT 0 FOR Status;
    END

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'DF_tblOTP.CreatedAt'
        AND type = 'D'
    )
    BEGIN
        ALTER TABLE tblForms
        ADD CONSTRAINT [DF_tblOTP.CreatedAt]
        DEFAULT GETUTCDATE() FOR CreatedAt;
    END

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblOTP.CreatedBy_tblUsers.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblOTP
        ADD CONSTRAINT [FK_tblOTP.CreatedBy_tblUsers.RowId]
        FOREIGN KEY (CreatedBy)
        REFERENCES tblUsers(RowId);
    END

    IF NOT EXISTS (
        SELECT 1
        FROM sys.objects
        WHERE name = 'FK_tblOTP.UpdatedBy_tblUsers.RowId'
        AND type = 'F'
    )
    BEGIN
        ALTER TABLE tblForms
        ADD CONSTRAINT [FK_tblOTP.UpdatedBy_tblUsers.RowId]
        FOREIGN KEY (UpdatedBy)
        REFERENCES tblUsers(RowId);
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
