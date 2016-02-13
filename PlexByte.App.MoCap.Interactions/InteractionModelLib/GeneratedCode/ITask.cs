﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface ITask 
{
	decimal Budget { get;set; }

	int Duration { get;set; }

	int Priority { get;set; }

	int DurationCurrent { get;set; }

	decimal BudgetUsed { get;set; }

	IEnumerable<ITimeslice> TimesliceList { get;set; }

	IEnumerable<IExpense> ExpenseList { get;set; }

	ITask SubTasks { get;set; }

	void AddTimeslice(ITimeslice pTimeslice);

	void AddExpense(IExpense pExpense);

}

