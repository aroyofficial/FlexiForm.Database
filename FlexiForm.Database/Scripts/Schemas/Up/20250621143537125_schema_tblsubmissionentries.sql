-- Script Type    : schema
-- Name           : 20250621143537125_schema_tblsubmissionentries.sql
-- Created At     : 2025-06-21 14:35:37 UTC (Arijit Roy)
-- Script ID      : 20250621143537125
-- Migration Type : Up

BEGIN TRY
    BEGIN TRANSACTION;

    CREATE TABLE tblSubmissionEntries
    (
        Id                 INT IDENTITY(1, 1) NOT NULL,
        RowId              UNIQUEIDENTIFIER DEFAULT Newid() NOT NULL,
        SubmissionId       UNIQUEIDENTIFIER NOT NULL,
        FormFieldMappingId UNIQUEIDENTIFIER NOT NULL,
        Value              NVARCHAR(2048) NOT NULL,
        CreatedBy          UNIQUEIDENTIFIER NOT NULL,
        UpdatedBy          UNIQUEIDENTIFIER NULL,
        CreatedAt          DATETIME NOT NULL DEFAULT Getutcdate(),
        UpdatedAt          DATETIME NULL
     );

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
