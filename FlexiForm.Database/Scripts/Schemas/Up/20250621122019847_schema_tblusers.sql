-- Script Type    : schema
-- Name           : 20250621122019847_schema_tblusers.sql
-- Created At     : 2025-06-21 12:20:19 UTC (Arijit Roy)
-- Script ID      : 20250621122019847
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    CREATE TABLE tblUsers
    (
        Id        INT IDENTITY(1, 1) NOT NULL,
        RowId     UNIQUEIDENTIFIER DEFAULT Newid() NOT NULL,
        FirstName VARCHAR(128) NOT NULL,
        LastName  VARCHAR(128) NOT NULL,
        Email     VARCHAR(256) NOT NULL,
        Password  VARCHAR(32) NOT NULL,
        Gender    TINYINT NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT Getutcdate(),
        UpdatedAt DATETIME NULL
    );

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
