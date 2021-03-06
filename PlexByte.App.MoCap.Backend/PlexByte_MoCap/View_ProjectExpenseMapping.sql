﻿--	View_ProjectExpenseMapping displays an overview of Expenses reported against projects in a grouped view
--	Author:	Christian B. Sax
--	Date:	2016/03/06
CREATE VIEW [dbo].[View_ProjectExpenseMapping]
	AS
	SELECT	[a].[ProjectId], [e].[Username], [e].[CreatedDateTime], [e].[Value], [e].[Receipt],  [p].[IsActive] AS IsActiveProject
	FROM	[Accounting] a INNER JOIN [View_Expense] e
			ON [a].[ExpenseId] = [e].[Id]
			INNER JOIN [View_Project] p
			ON [a].[ProjectId]=[p].[Id]