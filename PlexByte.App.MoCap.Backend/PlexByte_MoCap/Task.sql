﻿--	Task Table holding all tasks
--	Author:	Christian B. Sax
--	Date:	2016/02/21
CREATE TABLE [dbo].[Task]
(
	[Id] BIGINT NOT NULL PRIMARY KEY, 
	[InteractionId] BIGINT NOT NULL,
	[Title] NVARCHAR(250) NULL, 
    [DueDateTime] DATETIME NULL, 
    [Budget] DECIMAL(18, 2) NULL, 
    [Duration] INT NULL, 
    [Priority] INT NULL DEFAULT 1, 
    [Progress] INT NULL, 
    [DurationUsed] INT NULL, 
    [BudgetUsed] DECIMAL(18, 2) NULL, 
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    [ModifiedDateTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_Task_Interaction] FOREIGN KEY ([InteractionId]) REFERENCES [Interaction]([Id]), 
)

GO

CREATE INDEX [IX_Task_ModifiedDateTime] ON [dbo].[Task] ([ModifiedDateTime])
GO

CREATE INDEX [IX_Task_StartDateTime] ON [dbo].[Task] ([DueDateTime])
GO

CREATE INDEX [IX_Task_Priority] ON [dbo].[Task] ([Priority])
GO

CREATE INDEX [IX_Task_InteractionId] ON [dbo].[Task] ([InteractionId])
