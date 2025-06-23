-- Script Type    : schema
-- Name           : 20250621125537401_schema_tblformfields.sql
-- Created At     : 2025-06-21 12:55:37 UTC (Arijit Roy)
-- Script ID      : 20250621125537401
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    CREATE TABLE tblFormFields
    (
        Id            INT IDENTITY(1, 1) NOT NULL,
        RowId         UNIQUEIDENTIFIER DEFAULT Newid() NOT NULL,
        Name          NVARCHAR(256) NOT NULL,
        Type          TINYINT NOT NULL,
        Configuration NVARCHAR(MAX) NOT NULL,
        CreatedBy     UNIQUEIDENTIFIER NOT NULL,
        UpdatedBy     UNIQUEIDENTIFIER NULL,
        CreatedAt     DATETIME NOT NULL DEFAULT Getutcdate(),
        UpdatedAt     DATETIME NULL,
     );

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
