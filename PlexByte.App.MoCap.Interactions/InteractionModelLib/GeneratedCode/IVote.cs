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

public interface IVote 
{
	IUser User { get; }

	ISurveyOption Option { get; }

	DateTime CreatedDateTime { get;set; }

	string Id { get; }

}

