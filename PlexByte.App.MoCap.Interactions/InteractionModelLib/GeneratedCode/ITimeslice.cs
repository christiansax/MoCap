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

public interface ITimeslice 
{
	int Duration { get;set; }

	IUser User { get;set; }

	IInteraction Target { get;set; }

	string TaskId { get;set; }

	int CalculateDuration(DateTime pStartDT, DateTime pEndDT);

}

