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

public class Account : IAccount, IInteraction
{
	public virtual void OnComplete(InteractionEventArgs pEventArgs)
	{
		throw new System.NotImplementedException();
	}

	public virtual void ChangeOwner(IUser pUser)
	{
		throw new System.NotImplementedException();
	}

	public virtual void ChangeIsActive(bool pActive)
	{
		throw new System.NotImplementedException();
	}

	public virtual void AddExpense(IExpense pProject)
	{
		throw new System.NotImplementedException();
	}

	public virtual void AddTimeslice(ITimeslice pProject)
	{
		throw new System.NotImplementedException();
	}

	public virtual void OnModify(InteractionEventArgs pEventArgs)
	{
		throw new System.NotImplementedException();
	}

	public virtual void ChangeState(InteractionState pState)
	{
		throw new System.NotImplementedException();
	}

	public virtual void OnStateChanged(InteractionEventArgs pEventArgs)
	{
		throw new System.NotImplementedException();
	}

	public virtual void EditExpense(IExpense pExpense, IExpense pNewExpense)
	{
		throw new System.NotImplementedException();
	}

	public virtual void DeleteExpense(IExpense pExpense)
	{
		throw new System.NotImplementedException();
	}

	public virtual void EditTimeslice(ITimeslice PTimeslice, ITimeslice pNewTimeslice)
	{
		throw new System.NotImplementedException();
	}

	public virtual void DeleteTimeslice(ITimeslice pTimeslice)
	{
		throw new System.NotImplementedException();
	}

}

