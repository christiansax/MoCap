﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Interaction
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class Account
	{
		public virtual IEnumerable<Expense> Expense
		{
			get;
			set;
		}

		public virtual IEnumerable<TimeSlice> TimeSlice
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
}

