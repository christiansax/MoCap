﻿//////////////////////////////////////////////////////////////////////////////
//                            LogMessage                                    //
//      Author:     Christian B. Sax            Date:       04.01.2016      //
//      Project:    MoCap                       Reviewed:   <Name>          //
//      Component:  Logging                     Owner:      SAXC            //
//      Description: Class defining and implmenting a log message           //
//////////////////////////////////////////////////////////////////////////////

#region usings
#region Includes (Microsoft based)
//////////////////////////////////////////////
//      using includes here (Microsoft)     //
//////////////////////////////////////////////
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
#endregion

#region Includes (3rd parties)
//////////////////////////////////////////////
//      using includes here (3rd parties)   //
//////////////////////////////////////////////
#endregion

#region Includes (MoCap based)
//////////////////////////////////////////////
//      using includes here (MoCap)         //
//////////////////////////////////////////////
#endregion
#endregion

namespace MoCap.Logging
{
    #region Delegates are declared here

    #endregion

    #region Global variables are declared here

    #endregion

    #region Enum(-s) or type(-s) declaration here

    public enum MessageType
    {
        Error,
        Warning,
        Info,
        Detail,
        EnterScope,
        ExitScope,
        All
    };

    #endregion

    [Serializable]
    /// <summary>
    /// This is the summary of the class
    /// </summary>
    public class LogMessage
    {
        #region Class members

        #region Events

        #endregion

        #region Properties

        /// <summary>
        /// ReadOnly: The time stamp on which this message was created
        /// </summary>
        public DateTime TimeStamp { get; } = DateTime.Now;

        /// <summary>
        /// ReadOnly: The text of the log message
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// ReadOnly: The type of message
        /// </summary>
        public MessageType Type{ get; }

        /// <summary>
        /// ReadOnly: The log level under which this message is written
        /// </summary>
        public int Level { get; } = 61;

        /// <summary>
        /// ReadOnly: The thread number which wrote this message
        /// </summary>
        public string ThreadId { get; }

        /// <summary>
        /// The component to which this message belongs
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// ReadOnly: Method in which the message was logged
        /// </summary>
        public string MethodName { get; }

        /// <summary>
        /// ReadOnly: Method definition for which the message was logged
        /// </summary>
        public string MethodDefinition { get; }

        /// <summary>
        /// ReadOnly: Line number where the log message was written
        /// </summary>
        public int LineNumber { get; }

        /// <summary>
        /// ReadOnly: The source file in which the message was logged
        /// </summary>
        public string SourceFile { get; }

        /// <summary>
        /// The context of this message, used to bring a fractional part of information together
        /// </summary>
        public string Context { get; set; }

        /// <summary>
        /// An value to attribute this message
        /// </summary>
        public string Attribute1 { get; set; }

        [System.ComponentModel.Browsable(false)]
        /// <summary>
        /// An integer value specifying the indent level of the message.
        /// NOTE: Do not change this value
        /// </summary>
        public int IndentLevel { get; set; }

        #endregion

        #region Private variables

        #endregion

        #endregion

        #region Methods

        #region Constructor(-s) & Destructor

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="pText">The text of the message</param>
        public LogMessage(string pText) : 
            this(pText, MessageType.Info, 61, "", "", "", "")
        {
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="pText">The text of the message</param>
        /// <param name="pType">The type of the message</param>
        public LogMessage(string pText, MessageType pType) : 
            this(pText, pType, 61, "", "", "", "")
        {
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="pText">The text of the message</param>
        /// <param name="pType">The type of the message</param>
        /// <param name="pLevel">The level under which this message is logged => Default is 61</param>
        public LogMessage(string pText, MessageType pType, int pLevel) : 
            this(pText, pType, pLevel, "", "", "", "")
        {
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="pText">The text of the message</param>
        /// <param name="pType">The type of the message</param>
        /// <param name="pLevel">The level under which this message is logged => Default is 61</param>
        /// <param name="pThreadId">The thread id which logs the message => Default is empty</param>
        public LogMessage(string pText, MessageType pType, int pLevel, string pThread) : 
            this(pText, pType, pLevel, pThread, "", "", "")
        {
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="pText">The text of the message</param>
        /// <param name="pType">The type of the message</param>
        /// <param name="pLevel">The level under which this message is logged => Default is 61</param>
        /// <param name="pThreadId">The thread id which logs the message => Default is empty</param>
        /// <param name="pContext">The context to which the message belongs to => Default is empty</param>
        public LogMessage(string pText, MessageType pType, int pLevel, string pThread, string pContext) : 
            this(pText, pType, pLevel, pThread, pContext, "", "")
        {
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="pText">The text of the message</param>
        /// <param name="pType">The type of the message</param>
        /// <param name="pLevel">The level under which this message is logged => Default is 61</param>
        /// <param name="pThreadId">The thread id which logs the message => Default is empty</param>
        /// <param name="pContext">The context to which the message belongs to => Default is empty</param>
        /// <param name="pComponent">The component under which this message was written => Default is empty</param>
        public LogMessage(string pText, MessageType pType, int pLevel, string pThread, string pContext, string pAttribute) : 
            this(pText, pType, pLevel, pThread, pContext, pAttribute, "")
        {
        }

        /// <summary>
        /// Constructor of the class accepting all parameters. This is the default constructor, as all others call it
        /// </summary>
        /// <param name="pText">The text of the message</param>
        /// <param name="pType">The type of the message</param>
        /// <param name="pLevel">The level under which this message is logged => Default is 61</param>
        /// <param name="pThreadId">The thread id which logs the message => Default is empty</param>
        /// <param name="pContext">The context to which the message belongs to => Default is empty</param>
        /// <param name="pComponent">The component under which this message was written => Default is empty</param>
        /// <param name="pAttribute1">The value to attribute the message to => Default is empty</param>
        public LogMessage(string pText, MessageType pType = MessageType.Info, int pLevel = 61, string pThreadId = "", string pContext = "",
            string pAttribute1 = "", string pComponent = "",
            [System.Runtime.CompilerServices.CallerMemberName] string pMemberName = "",
            [System.Runtime.CompilerServices.CallerFilePath] string pSourceFilePath = "",
            [System.Runtime.CompilerServices.CallerLineNumber] int pSourceLineNumber = 0)
        {
            ParameterInfo[] parameters = new StackTrace().GetFrame(1).GetMethod().GetParameters();
            Text = pText;
            Type = pType;
            Level = pLevel;
            if (pThreadId != Thread.CurrentThread.ManagedThreadId.ToString())
                ThreadId = pThreadId;
            else
                ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();
            Context = pContext;
            Component = pComponent;
            Attribute1 = pAttribute1;
            LineNumber = pSourceLineNumber;
            MethodName = pMemberName;
            MethodDefinition = pMemberName +"(";

            SourceFile = pSourceFilePath;

            for(int i=0; i<parameters.Length;i++)
            {
                if(i==0)
                    MethodDefinition += "["+ parameters[i].ParameterType+" | "+ parameters[i].Name+"]";
                else
                    MethodDefinition += ", [" + parameters[i].ParameterType + " | " + parameters[i].Name + "]";
            }
            MethodDefinition += ")";
        }

        #endregion

        #region Private Methods

        #endregion

        #region Event handlers (methods)

        #endregion

        #region Public Methods

        #endregion

        #endregion

        #region Nested classes here (indentical structure here)

        #endregion
    }
}