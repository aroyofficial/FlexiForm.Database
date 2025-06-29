-- Script Type    : schema
-- Name           : 20250621135318333_schema_tblformfieldmappings.sql
-- Created At     : 2025-06-21 13:53:18 UTC (Arijit Roy)
-- Script ID      : 20250621135318333
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    IF Object_id('tblFormFieldMappings', 'U') IS NULL
    BEGIN
        CREATE TABLE tblFormFieldMappings
        (
            Id          INT IDENTITY(1, 1) NOT NULL,
            RowId       UNIQUEIDENTIFIER DEFAULT Newid() NOT NULL,
            FormId      UNIQUEIDENTIFIER NOT NULL,
            FormFieldId UNIQUEIDENTIFIER NOT NULL,
            Required    BIT NOT NULL,
            [Order]     TINYINT NOT NULL,
            CreatedBy   UNIQUEIDENTIFIER NOT NULL,
            UpdatedBy   UNIQUEIDENTIFIER NULL,
            CreatedAt   DATETIME NOT NULL DEFAULT Getutcdate(),
            UpdatedAt   DATETIME NULL
         );
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
