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

public abstract class Survey : ISurvey, IInteraction
{
	private System.Timers.Timer _stateTimer
	{
		get;
		set;
	}

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

	public virtual void AddVote(IVote pOption)
	{
		throw new System.NotImplementedException();
	}

	public virtual void AddOption(ISurveyOption pOption)
	{
		throw new System.NotImplementedException();
	}

	public Survey(string pId, string pText, List<ISurveyOption> pOptions, IUser pCreator)
	{
	}

	public Survey(string pId, string pText, List<string> pOptions, IUser pCreator)
	{
	}

	private void InitializeProperties(string pId, string pText, List<ISurveyOption> pOptions, IUser pCreator)
	{
		throw new System.NotImplementedException();
	}

	public virtual void OnModify(InteractionEventArgs pEventArgs)
	{
		throw new System.NotImplementedException();
	}

	public virtual void AddUser(IUser pUser)
	{
		throw new System.NotImplementedException();
	}

	public virtual void RemoveUser(IUser pUser)
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

	private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
	{
		throw new System.NotImplementedException();
	}

}

