﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

public class Expense : IExpense, IInteraction
{
    DateTime IInteraction.CreatedDateTime
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    DateTime IInteraction.EndDateTime
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    string IInteraction.Id
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    InteractionEventArgs IInteraction.InteractionEventArgs
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    bool IInteraction.IsActive
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    DateTime IInteraction.ModifiedDateTime
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    Image IExpense.Receipt
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    DateTime IInteraction.StartDateTime
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    string IInteraction.Text
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    InteractionType IInteraction.Type
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    decimal IExpense.Value
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    void IExpense.AddReceipt(Image pImage)
    {
        throw new NotImplementedException();
    }

    void IInteraction.ChangeIsActive(bool pActive)
    {
        throw new NotImplementedException();
    }

    void IInteraction.ChangeOwner(IUser pUser)
    {
        throw new NotImplementedException();
    }

    void IInteraction.OnComplete(IInteraction pInteraction, InteractionEventArgs e)
    {
        throw new NotImplementedException();
    }
}

