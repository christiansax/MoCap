﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace MoCap.Portable.Library.Interaction
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class Vote
	{
		public virtual long ID
		{
			get;
			set;
		}

		public virtual Poll Poll
		{
			get;
			set;
		}

		public virtual PollOption PollOption
		{
			get;
			set;
		}

		public virtual void Submit(object pPoll)
		{
			throw new System.NotImplementedException();
		}

	}
}

