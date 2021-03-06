﻿--	Sproc_TaskAddUpdate updated or inserts a task
--	Author:	Christian B. Sax
--	Date:	2016/03/05
CREATE PROCEDURE [dbo].[Sproc_TaskAddUpdate]
	@TaskId AS BIGINT,
	@InteractionId BIGINT,
    @DueDateTime AS DATETIME,
    @Budget AS DECIMAL,
    @Duration AS INT,
    @Priority AS INT,
    @Progress AS INT,
    @DurationUsed AS INT,
    @BudgetUsed AS DECIMAL,
	@StartDateTime AS DATETIME,
	@EndDateTime AS DATETIME,
    @IsActive AS BIT,
    @Text AS NVARCHAR(MAX),
	@Title AS NVARCHAR(250),
    @CreatorId AS BIGINT,
    @OwnerId AS BIGINT,
    @StateId AS BIGINT,
	@ProjectId AS BIGINT,
	@ResultMsg NVARCHAR(250) OUTPUT
AS
	DECLARE @Id BIGINT;
	SET @ResultMsg = 'OK';

	BEGIN
		IF (NOT EXISTS (
		SELECT	*
		FROM	View_Task
		WHERE	[Id] = @TaskId))
		BEGIN TRY
			-- This is a new user, insert...
			BEGIN TRANSACTION
				select	@Id	=	format(getdate(), 'yyyyMMddHHmmssfff')
				INSERT INTO [dbo].[Interaction] ([Id], [StartDateTime], [EndDateTime], [IsActive], [Text], [Type], [CreatorId],
												[OwnerId], [StateId], [CreatedDateTime], [ModifiedDateTime])
						VALUES					(@Id, @StartDateTime, @EndDateTime, @IsActive, @Text, 2, @CreatorId, 
												@OwnerId, @StateId, GETDATE(), GETDATE())
				
				INSERT INTO [dbo].[Task]		([Id], [Title], [DueDateTime], [Budget], [Duration], [Priority], [Progress], [DurationUsed],
												[BudgetUsed], [CreatedDateTime], [ModifiedDateTime], [InteractionId])
						VALUES					(@TaskId, @Title, @DueDateTime, @Budget, @Duration, @Priority, @Progress, @DurationUsed,
												@BudgetUsed, GETDATE(), GETDATE(), @Id)

				IF (@ProjectId>0)
				BEGIN
					select	@Id	=	format(getdate(), 'yyyyMMddHHmmssfff')
					INSERT INTO [ProjectTaskMapping]([Id], [ProjectId], [TaskId], [CreatedDateTime], [ModifiedDateTime])
							VALUES					(@Id, @ProjectId, @TaskId, GETDATE(), GETDATE())
				END
			COMMIT TRANSACTION
			SET @ResultMsg = @ResultMsg + ': Inserted';
		END TRY
		BEGIN CATCH
			SET @ResultMsg = 'Error in insert try block: ' + ERROR_MESSAGE();
			IF @@TRANCOUNT > 0
			ROLLBACK
			RAISERROR ('Caught exception %s', 16, -1, @ResultMsg);
		END CATCH
		ELSE
		BEGIN TRY
			-- This is an update
			BEGIN TRANSACTION
				UPDATE [dbo].[Interaction]
				   SET [StartDateTime] = @StartDateTime,
					   [EndDateTime] = @EndDateTime,
					   [IsActive] = @IsActive,
					   [Text] = @Text,
					   [CreatorId] = @CreatorId,
					   [OwnerId] = @OwnerId,
					   [StateId] = @StateId,
					   [ModifiedDateTime] = GETDATE()
				 WHERE [Id] = @InteractionId
				
				SELECT @Id = [ProjectId] FROM [ProjectTaskMapping] WHERE [TaskId] = @TaskId;
				IF(@Id <> @ProjectId)
				BEGIN
					-- Task has changed project
					UPDATE	[dbo].[ProjectTaskMapping]
					SET		[ProjectId] = @ProjectId
					WHERE	[TaskId] = @TaskId
				END

				UPDATE [dbo].[Task]
				   SET [DueDateTime] = @DueDateTime,
					   [Title] = @Title,
					   [Budget] = @Budget,
					   [Duration] = @Duration,
					   [Priority] = @Priority,
					   [Progress] = @Progress,
					   [DurationUsed] = @Duration,
					   [BudgetUsed] = @BudgetUsed,
					   [ModifiedDateTime] = GETDATE()
				 WHERE [Id] = @TaskId
			COMMIT TRANSACTION
			SET @ResultMsg = @ResultMsg + ': Updated';
		END TRY
		BEGIN CATCH
			SET @ResultMsg = 'Error in update try block: ' + ERROR_MESSAGE();
			IF @@TRANCOUNT > 0
			ROLLBACK
			RAISERROR ('Caught exception %s', 16, -1, @ResultMsg);
		END CATCH
	END
RETURN 0