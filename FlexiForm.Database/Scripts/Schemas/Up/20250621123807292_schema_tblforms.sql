-- Script Type : schema
-- Name        : 20250621123807292_schema_tblforms.sql
-- Created At  : 2025-06-21 12:38:07 UTC (Arijit Roy)
-- Script ID   : 20250621123807292
-- Migration Type: Up

BEGIN TRY
    BEGIN TRANSACTION;

    IF Object_id('tblForms', 'U') IS NULL
    BEGIN
        CREATE TABLE tblForms
        (
            Id             INT IDENTITY(1, 1) NOT NULL,
            RowId          UNIQUEIDENTIFIER DEFAULT Newid() NOT NULL,
            Name           NVARCHAR(256) NOT NULL,
            HeaderImageUrl VARCHAR(512) NOT NULL,
            HeaderText     VARCHAR(1024) NOT NULL,
            CreatedBy      UNIQUEIDENTIFIER NOT NULL,
            UpdatedBy      UNIQUEIDENTIFIER NULL,
            CreatedAt      DATETIME NOT NULL DEFAULT Getutcdate(),
            UpdatedAt      DATETIME NULL,
        );
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
