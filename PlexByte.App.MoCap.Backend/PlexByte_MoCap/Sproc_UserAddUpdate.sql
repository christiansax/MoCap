﻿--	Sproc_UserAddUpdate inserts or updates a user
--	Author:	Christian B. Sax
--	Date:	2016/03/05
CREATE PROCEDURE [dbo].[Sproc_UserAddUpdate]
	@UserId BIGINT,
	@FirstName NVARCHAR(MAX),
	@LastName NVARCHAR(MAX),
	@MiddleName NVARCHAR(MAX),
	@EmailAddress NVARCHAR(MAX),
	@BirthDate DATETIME,
	@UserName NVARCHAR(MAX),
	@Password NVARCHAR(MAX),
	@ResultMsg NVARCHAR(250) OUTPUT
AS
	DECLARE @Id BIGINT;
	SET @ResultMsg='OK';

	IF (NOT EXISTS (
		SELECT	*
		FROM	[View_User]
		WHERE	[Id] = @UserId))
	BEGIN TRY
		-- This is a new user, insert...
		BEGIN TRANSACTION
			select	@Id	=	format(getdate(), 'yyyyMMddHHmmssfff')
			INSERT	INTO [Person]	([Id], [FirstName], [LastName], [MiddleName], [EmailAddress], [Birthdate])
					VALUES			(@Id, @FirstName, @LastName, @MiddleName, @EmailAddress, @BirthDate)
			INSERT	INTO [User]		([Id], [PersonId], [Username], [Password])
					VALUES			(@UserId, @Id, @UserName, @Password)
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
			UPDATE	[Person]
			SET		[FirstName]			=	@FirstName,
					[LastName]			=	@LastName,
					[MiddleName]		=	@MiddleName,
					[EmailAddress]		=	@EmailAddress,
					[Birthdate]			=	@BirthDate,
					[ModifiedDateTime]	=	GETDATE()
			WHERE	[Id]				=	(SELECT PersonId FROM [User] WHERE [Id] = @UserId)
			
			UPDATE	[User]
			SET		[Username]			=	@UserName,
					[Password]			=	@Password,
					[ModifiedDateTime]	=	GETDATE()
			WHERE	[Id]				=	@UserId
		COMMIT TRANSACTION
		SET @ResultMsg = @ResultMsg + ': Updated';
	END TRY
	BEGIN CATCH
		SET @ResultMsg = 'Error in update try block: ' + ERROR_MESSAGE();
		IF @@TRANCOUNT > 0
        ROLLBACK
		RAISERROR ('Caught exception %s', 16, -1, @ResultMsg);
	END CATCH
RETURN 0