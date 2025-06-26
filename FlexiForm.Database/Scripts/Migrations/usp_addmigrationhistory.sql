BEGIN TRY
    BEGIN TRANSACTION;

    DECLARE @L_SQL NVARCHAR(MAX) = '
    CREATE PROCEDURE usp_AddMigrationHistory @id         VARCHAR(17),
                                                @scriptname VARCHAR(256),
                                                @scripthash VARCHAR(64),
                                                @type       TINYINT
    WITH ENCRYPTION
    AS
        BEGIN
            BEGIN TRY
                SET NOCOUNT ON;

                DECLARE @L_Id VARCHAR(17) = @id;
                DECLARE @L_ScriptName VARCHAR(256) = @scriptname;
                DECLARE @L_ScriptHash VARCHAR(64) = @scripthash;
                DECLARE @L_Type TINYINT = @type;

                IF @L_Id IS NULL
                BEGIN
                    RAISERROR(''@id must be given.'',16,1);
                END

                IF @L_ScriptName IS NULL
                BEGIN
                    RAISERROR(''@scriptname must be given'',16,1);
                END

                IF @L_ScriptHash IS NULL
                BEGIN
                    RAISERROR(''@scripthash must be given'',16,1);
                END

                IF @L_Type IS NULL
                BEGIN
                    RAISERROR(''@type must be given'',16,1);
                END

                IF NOT EXISTS (
                SELECT 1
                FROM tblMigrationHistory
                WHERE Id = @L_Id
                AND ScriptName = @L_ScriptName
                AND ScriptHash = @L_ScriptHash
                AND [Type] = @L_Type
                )
                BEGIN
                    INSERT INTO tblMigrationHistory
                                (Id,
                                ScriptName,
                                ScriptHash,
                                [Type])
                    VALUES      (@L_Id,
                                @L_ScriptName,
                                @L_ScriptHash,
                                @L_Type);
                END

                SET NOCOUNT OFF;

                RETURN;
            END TRY
            BEGIN CATCH
                SET NOCOUNT OFF;
                DECLARE @L_ErrorMessage VARCHAR(512) = Error_message();
                RAISERROR(@L_ErrorMessage,16,1);
                RETURN;
            END CATCH
        END';

    EXEC sp_executesql @L_SQL;

    COMMIT;
END TRY
BEGIN CATCH
    ROLLBACK;
    PRINT 'An error occurred: ' + ERROR_MESSAGE();
END CATCH
