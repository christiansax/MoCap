﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace PlexByte.MoCap.Interactions
{
	using PlexByte.MoCap.Logging;
	using PlexByte.MoCap.Security;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public interface ITimeSlice 
	{
		IUser User { get;set; }

		int Duration { get;set; }

		DateTime StartDateTIme { get;set; }

		DateTime EndDateTime { get;set; }

		ITask TimeSlices { get;set; }

		void CreateTimeSlice(IUser pUser, int pDuration);

		void CreateTimeSlice(IUser pUser, DateTime pStart, DateTime pEnd);

	}
}

