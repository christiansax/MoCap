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

public class SurveyFactory : ISurveyFactory
{
	public virtual IVote CreateVote(string pId, IUser pUser, ISurveyOption pOption)
	{
		throw new System.NotImplementedException();
	}

	public virtual ISurveyOption CreateSurveyOption(string pId, string pText)
	{
		throw new System.NotImplementedException();
	}

	public virtual ISurvey CreateSurvey(string pId, string pText, List<ISurveyOptions> pOptions, IUser pCreator)
	{
		throw new System.NotImplementedException();
	}

	public virtual ISurvey CreateSurvey(string pId, string pText, List<string> pOptions, IUser pCreator)
	{
		throw new System.NotImplementedException();
	}

	public virtual void AddOption(ISurvey pSurvey, ISurveyOption pOption)
	{
		throw new System.NotImplementedException();
	}

	public virtual void AddVote(ISurvey pSurvey, IVote pVote)
	{
		throw new System.NotImplementedException();
	}

}

