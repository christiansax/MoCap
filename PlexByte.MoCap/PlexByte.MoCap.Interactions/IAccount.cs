﻿//////////////////////////////////////////////////////////////
//                      Interface Account                              
//      Author: Fabian Ochsner            Date:   2016/02/19
using System.Collections.Generic;
using PlexByte.MoCap.Security;

namespace PlexByte.MoCap.Interactions
{

    public delegate void ExpenseAdd(object sender, InteractionEventArgs e);
    public delegate void TimesliceAdd(object sender, InteractionEventArgs e);

    public interface IAccount
    {
        event ExpenseAdd ExpenseAdded;
        event TimesliceAdd TimesliceAdded;


        string Id { get; }

        List<IExpense> ExpenseList { get; }

        List<ITimeslice> TimesliceList { get; }

        void AddExpense(IExpense pExpense);

        void AddTimeslice(ITimeslice pTimeslice);

        void DeleteExpense(IExpense pExpense);

        void EditTimeslice(ITimeslice pTimeslice, ITimeslice pNewTimeslice);

        void DeleteTimeslice(ITimeslice pTimeslice);

        decimal UserExpense(IUser pUser);

        int UserTimeslice(IUser pUser);
    }

}