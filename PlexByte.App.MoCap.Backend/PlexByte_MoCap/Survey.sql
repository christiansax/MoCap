﻿--	Survey table holding all survey configured
--	Author:	Christian B. Sax
--	Date:	2016/02/21
CREATE TABLE [dbo].[Survey]
(
	[Id] BIGINT NOT NULL , 
	[InteractionId] BIGINT NOT NULL , 
    [MaxVotePerUser] INT NOT NULL DEFAULT 1, 
	[DueDateTime] DATETIME NULL,
    [TaskId] BIGINT NULL, 
    [CreatedDateTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    [ModifiedDateTime] DATETIME NOT NULL DEFAULT GETDATE(), 
    [Title] NVARCHAR(250) NOT NULL, 
    CONSTRAINT [FK_Survey_Task] FOREIGN KEY ([TaskId]) REFERENCES [Task]([Id]), 
    CONSTRAINT [FK_Survey_Interaction] FOREIGN KEY ([InteractionId]) REFERENCES [Interaction]([Id]), 
    CONSTRAINT [PK_Survey] PRIMARY KEY ([Id]) 
)

GO

CREATE INDEX [IX_Survey_TaskId] ON [dbo].[Survey] ([TaskId])

GO

CREATE INDEX [IX_Survey_CreatedDateTime] ON [dbo].[Survey] ([CreatedDateTime])

GO

CREATE INDEX [IX_Survey_ModifiedDateTime] ON [dbo].[Survey] ([ModifiedDateTime])
