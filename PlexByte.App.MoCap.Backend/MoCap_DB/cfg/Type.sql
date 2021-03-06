﻿CREATE TABLE [cfg].[Type]
(
	[Id] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Component] NVARCHAR(50) NOT NULL, 
    [CreatedDateTime] datetime2(3) NOT NULL DEFAULT GETDATE(), 
    [ModifiedDateTime] datetime2(3) NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [AK_Type_NameComponent] UNIQUE ([Name], [Component])
)

GO

CREATE INDEX [IX_Type_Name] ON [cfg].[Type] ([Name])

GO

CREATE INDEX [IX_Type_Component] ON [cfg].[Type] ([Component])

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This table defines types and their corresponding component',
    @level0type = N'SCHEMA',
    @level0name = N'cfg',
    @level1type = N'TABLE',
    @level1name = N'Type',
    @level2type = NULL,
    @level2name = NULL