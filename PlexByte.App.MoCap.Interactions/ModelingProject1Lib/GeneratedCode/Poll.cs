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

public class Poll
{
	public virtual IEnumerable<Option> Option
	{
		get;
		set;
	}

	public virtual IEnumerable<Vote> Vote
	{
		get;
		set;
	}

	public virtual User User
	{
		get;
		set;
	}

	public virtual Project Project
	{
		get;
		set;
	}

}

