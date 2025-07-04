-- Script Type    : schema
-- Name           : 20250704112514765_schema_tblotp.sql
-- Created At     : 2025-07-04 11:25:14 UTC (Sushmita Das)
-- Script ID      : 20250704112514765
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

     IF Object_id('tblOTP', 'U') IS NULL
    BEGIN
        CREATE TABLE tblOTP
        (
            Id             INT IDENTITY(1, 1) NOT NULL,
            RowId          UNIQUEIDENTIFIER DEFAULT Newid() NOT NULL,
            Value          VARCHAR(64) NOT NULL,
            Status         TINYINT NOT NULL,
            GeneratedAt    DATETIME,
            ExpiredAt      DATETIME,
            ConsumedAt     DATETIME NULL,
            CreatedAt      DATETIME,
            UpdatedAt      DATETIME NULL,
            CreatedBy      UNIQUEIDENTIFIER, 
            UpdatedBy      UNIQUEIDENTIFIER NULL
        );
    END
    

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
