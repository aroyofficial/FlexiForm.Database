-- Script Type    : schema
-- Name           : 20250621141228736_schema_tblsubmissions.sql
-- Created At     : 2025-06-21 14:12:28 UTC (Arijit Roy)
-- Script ID      : 20250621141228736
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    CREATE TABLE tblSubmissions
    (
        Id          INT IDENTITY(1, 1) NOT NULL,
        RowId       UNIQUEIDENTIFIER DEFAULT Newid() NOT NULL,
        FormId      UNIQUEIDENTIFIER NOT NULL,
        SubmittedBy UNIQUEIDENTIFIER NOT NULL,
        CreatedBy   UNIQUEIDENTIFIER NOT NULL,
        UpdatedBy   UNIQUEIDENTIFIER NULL,
        CreatedAt   DATETIME NOT NULL DEFAULT Getutcdate(),
        UpdatedAt   DATETIME NULL
     );

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
