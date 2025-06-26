BEGIN TRY
    BEGIN TRANSACTION;

    IF Object_id('tblMigrationHistory', 'U') IS NULL
    BEGIN
        CREATE TABLE tblMigrationHistory
        (
            Id         VARCHAR(17) NOT NULL,
            RowId      UNIQUEIDENTIFIER DEFAULT Newid() NOT NULL,
            ScriptName VARCHAR(256) NOT NULL,
            ScriptHash VARCHAR(64) NOT NULL,
            RanAt      DATETIME NOT NULL DEFAULT Getutcdate(),
            [Type]     TINYINT NOT NULL
        );

        ALTER TABLE tblMigrationHistory
        ADD CONSTRAINT [PK_tblMigrationHistory.RowId]
        PRIMARY KEY (RowId);

        ALTER TABLE tblMigrationHistory
        ADD CONSTRAINT [UNI_tblMigrationHistory.Id]
        UNIQUE (Id);
    END

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
