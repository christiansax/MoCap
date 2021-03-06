﻿//////////////////////////////////////////////////////////////
//                      Class UIManager
//      Author: Christian B. Sax            Date:   2016/03/16
//      This class manages the windows presented in the user interface. It further
//      registers to all available events and executes the corresponding action
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PlexByte.MoCap.Helpers;
using PlexByte.MoCap.Interactions;
using PlexByte.MoCap.Security;
using PlexByte.MoCap.WinForms.CustomForms;
using PlexByte.MoCap.WinForms.UserControls;
using WeifenLuo.WinFormsUI.Docking;
using PlexByte.MoCap.Managers;

namespace PlexByte.MoCap.WinForms
{
    public enum UiType
    {
        User,
        Task,
        Project,
        Account, 
        Survey,
        Expense,
        Timeslice
    }

    /// <summary>
    /// This class is responsible for any window that is being presented to the user 
    /// and manages its corresponding events
    /// </summary>
    public class UIManager : IDisposable
    {
        #region Properties

        public User UserContext => _userContext;

        #endregion

        #region Variables

        private readonly frm_MoCapMain _MainUI = null;
        private readonly ErrorProvider _errorProvider = null;
        private ObjectManager _objectManager = null;
        private uc_MenuBar _menuBar = null;
        private User _userContext = null;
        private uc_Overview _overviewPanel = null;
        private const string _longDateTimeFtm = "ddd dd MMM yyyy  HH:mm";
        private const string _DateTimeFtm = "yyyy.MM.dd HH:mm:ss";
        private const string _DateFtm = "yyyy.MM.dd";
        private const string _TimeFtm = "HH:mm:ss";
        private const string _dateTimeIdFmt = "yyyyMMddHHmmssfff";

        #endregion

        #region Ctor & Dtor

        /// <summary>
        /// Constructor of the class
        /// </summary>
        /// <param name="pMainUI">The pointer to the main form, which hosts any other window</param>
        public UIManager(frm_MoCapMain pMainUI)
        {
            _MainUI = pMainUI;
            _errorProvider = new ErrorProvider();
            _errorProvider.BlinkStyle = ErrorBlinkStyle.BlinkIfDifferentError;
            _objectManager = new ObjectManager();
            _objectManager.UserLoggedIn += ObjectManager_UserLoggedIn;
            _objectManager.UserLoggedOut += ObjectManager_UserLoggedOut;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (UserContext != null)
                _userContext = null;
            _objectManager.UserLoggedIn -= ObjectManager_UserLoggedIn;
            _objectManager.UserLoggedOut -= ObjectManager_UserLoggedOut;
            _objectManager?.Dispose();
            _objectManager = null;
        }

        #endregion

        #region Public Methods

        #region Content Panels

        /// <summary>
        /// Adds the user panel to the form and registers to its events
        /// </summary>
        public void AddUserPanel()
        {
            DockContent tmp = CreateContentPanel(UiType.User);
            tmp.TabText = "User Dialog";
            ((uc_User) tmp).RegisterEvents(this);
        }

        /// <summary>
        /// Adds the task panel to the form
        /// </summary>
        public void AddTaskPanel()
        {
            DockContent tmp = CreateContentPanel(UiType.Task);
            tmp.TabText = "Task Dialog";
            ((uc_Task) tmp).RegisterEvents(this);
            ((uc_Task)tmp).SetProjectCollection(_objectManager.ProjectList);
            ((uc_Task) tmp).TaskId = DateTime.Now.ToString(_dateTimeIdFmt);
            ((uc_Task)tmp).InteractionId = DateTime.Now.AddMilliseconds(100).ToString(_dateTimeIdFmt);
        }

        /// <summary>
        /// Adds the survey panel to the form
        /// </summary>
        public void AddSurveyPanel()
        {
            DockContent tmp = CreateContentPanel(UiType.Survey);
            tmp.TabText = "Survey Dialog";
            ((uc_Survey) tmp).RegisterEvents(this);
            foreach (var project in _objectManager.ProjectList)
            {
                GetControlByName<ComboBox>(tmp, "cbx_Project").Items.Add(project.Id);
            }
        }

        /// <summary>
        /// Adds the project panel to the form
        /// </summary>
        public void AddProjectPanel()
        {
            DockContent tmp = CreateContentPanel(UiType.Project);
            tmp.TabText = "Project Dialog";
            ((uc_Project) tmp).RegisterEvents(this);
        }

        /// <summary>
        /// Adds the account panel to the form
        /// </summary>
        public void AddAccoutPanel()
        {
            DockContent tmp = CreateContentPanel(UiType.Account);
            tmp.TabText = "Account Dialog";
            ((uc_Account)tmp).RegisterEvents(this);
        }

        /// <summary>
        /// Adds the expense panel to the form
        /// </summary>
        public void AddExpensePanel()
        {
            DockContent tmp = CreateContentPanel(UiType.Expense);
            tmp.TabText = "Expense Dialog";
        }

        /// <summary>
        /// Adds the timeslice panel to the form
        /// </summary>
        public void AddTimeslicePanel()
        {
            DockContent tmp = CreateContentPanel(UiType.Timeslice);
            tmp.TabText = "Timeslice Dialog";
        }

        /// <summary>
        /// Adds the menu bar and registers to its events
        /// </summary>
        public void AddMenuBar()
        {
            _menuBar = new uc_MenuBar();
            _menuBar.TabText = "Main Menu";
            _menuBar.CloseButton = false;
            _menuBar.Show(_MainUI.Panel, DockState.DockTop);
            ((uc_MenuBar)_menuBar).RegisterEvents(this);
            if (UserContext != null)
                _menuBar.EnableButtonControls();
            else
                _menuBar.DisableButtonControls();
        }

        /// <summary>
        /// Adds the overview panel and registers to its events
        /// </summary>
        public void AddOverview()
        {
            DockContent tmp = new uc_Overview(this);
            tmp.Show(_MainUI.Panel, DockState.DockLeft);
            _overviewPanel = (uc_Overview) tmp;
        }

        #endregion

        #region UIEvents

        /// <summary>
        /// This method is the event handler for the userLoggedIn event raised by objectManager
        /// </summary>
        /// <param name="sender">the user that logged in</param>
        /// <param name="e">The event arguments</param>
        private void ObjectManager_UserLoggedOut(object sender, EventArgs e)
        {
            _userContext = null;
            _overviewPanel.ClearDataGridViews();
            _menuBar.DisableButtonControls();
        }

        /// <summary>
        /// This method is the event handler for the userLoggedOut event raised by objectManager
        /// </summary>
        /// <param name="sender">The user raising the event</param>
        /// <param name="e">The event args</param>
        private void ObjectManager_UserLoggedIn(object sender, EventArgs e)
        {
            _userContext = (User)_objectManager.UserContext;
            _overviewPanel.UnRegisterEvents();
            GenerateOverviewPanel();
            _menuBar.EnableButtonControls();
        }

        /// <summary>
        /// Eventlistener for the menu form. Any button event is captured here and corresponding action is executed
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        public void MenuButtonClicked(object sender, EventArgs e)
        {
            switch (((Button) sender).Name)
            {
                case "btn_User":
                    AddUserPanel();
                    break;
                case "btn_Survey":
                    AddSurveyPanel();
                    break;
                case "btn_Task":
                    AddTaskPanel();
                    break;
                case "btn_Project":
                    AddProjectPanel();
                    break;
                case "btn_Account":
                    AddAccoutPanel();
                    break;
                case "btn_Expense":
                    AddExpensePanel();
                    break;
                case "btn_Timeslice":
                    AddTimeslicePanel();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Event handler method for the uc_User form button click events
        /// </summary>
        /// <param name="sender">The button raising the event</param>
        /// <param name="e">The event args</param>
        public void UserButtonClicked(object sender, EventArgs e)
        {
            _errorProvider.Clear();
            List<Control> ctrls = GetAllControls(((Button) sender).Parent);
            switch (((Button) sender).Name)
            {
                case "btn_Edit":
                    UserButtonEdit(ctrls);
                    break;
                case "btn_New":
                    if (GetControlByName<Button>(ctrls, "btn_New").Text.ToLower() == "new")
                        UserButtonNew(ctrls);
                    else
                        UserButtonSave(ctrls);
                    break;
                case "btn_Login":
                    if (GetControlByName<Button>(ctrls, "btn_Login").Text.ToLower() == "login")
                        UserButtonLogin(ctrls);
                    else
                        UserButtonLogout(ctrls);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Event handler method for the uc_Task for button click events
        /// </summary>
        /// <param name="sender">The button raising the event</param>
        /// <param name="e">The event args</param>
        public void TaskButtonClicked(object sender, EventArgs e)
        {
            _errorProvider.Clear();
            List<Control> ctrls = GetAllControls(((Button) sender).Parent);
            switch (((Button) sender).Name)
            {
                case "btn_New":
                    if (GetControlByName<Button>(ctrls, "btn_New").Text.ToLower() == "new")
                        TaskButtonNew(ctrls);
                    else
                        TaskButtonSave(ctrls);
                    break;
                case "btn_Update":
                    TaskButtonUpdate(ctrls);
                    break;
                case "btn_Edit":
                    TaskButtonEdit(ctrls);
                    break;
                case "btn_Subtask":
                    TaskButtonSubTask(ctrls);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// This method handles gridView items being doubleclicked
        /// </summary>
        /// <param name="sender">The object that was clicked</param>
        /// <param name="e">The dataGridView eventargs providing the row</param>
        public void OverviewGridviewDoubleClicked(object sender, DataGridViewCellEventArgs e)
        {
            DockContent tmp = null;
            string clickedId = ((DataGridView) sender).Rows[e.RowIndex].Cells[0].Value.ToString();
            if (_objectManager.ProjectList.Any(x => x.Id == clickedId))
            {
                tmp = _objectManager.CreateFormFromObject<IInteraction>(
                    (Project) _objectManager.ProjectList.First(x => x.Id == clickedId));
                ((uc_Project)tmp).RegisterEvents(this);
            }
            else if (_objectManager.TaskList.Any(x => x.Id == clickedId))
            {
                tmp = _objectManager.CreateFormFromObject<IInteraction>(
                    (Task) _objectManager.TaskList.First(x => x.Id == clickedId));
                ((uc_Task)tmp).RegisterEvents(this);
            }
            else if (_objectManager.SurveyList.Any(x => x.Id == clickedId))
            {
                tmp = _objectManager.CreateFormFromObject<IInteraction>(
                    (Survey) _objectManager.SurveyList.First(x => x.Id == clickedId));
                ((uc_Survey)tmp).RegisterEvents(this);
            }
            else
            {
                _MainUI.ShowErrorMessage($"The id {clickedId} referenced did not resolve in either a project, " +
                                         $"task or survey!");
            }
            tmp?.Show(_MainUI.Panel, DockState.Document);
        }

        /// <summary>
        /// Eventlistener for the Project form.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguements</param>
        public void ProjectButtonClicked(object sender, EventArgs e)
        {
            _errorProvider.Clear();
            List<Control> ctrls = GetAllControls(((Button) sender).Parent);
            switch (((Button) sender).Name)
            {
                case "btn_Create":
                    if (GetControlByName<Button>(ctrls, "btn_Create").Text.ToLower() == "save")
                        ProjectButtonCreate(ctrls);
                    else
                        ProjectButtonEdit(ctrls);
                    break;
                case "btn_Update":
                    ProjectButtonUpdatel(ctrls);
                    break;
                case "btn_InviteUser":
                    ProjectButtonInviteUser(ctrls);
                    break;
                case "btn_ChangeOwner":
                    ProjectButtonChangeOwner(ctrls);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Eventlistener for the Account form.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguements</param>
        public void AccountButtonClicked(object sender, EventArgs e)
        {
            _errorProvider.Clear();
            List<Control> ctrls = GetAllControls(((Button)sender).Parent);
            switch (((Button)sender).Name)
            {
                case "btn_AssignProject":
                    AccountButtonAssign(ctrls);
                    break;
                case "btn_Update":
                    AccountButtonUpdate(ctrls);
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// This method handles button event fired from the survey form
        /// </summary>
        /// <param name="sender">The button sending the event</param>
        /// <param name="e">the event arguments</param>
        public void SurveyButtonClicked(object sender, EventArgs e)
        {
            _errorProvider.Clear();
            List<Control> ctrls = GetAllControls(((Button) sender).FindForm());
            switch (((Button) sender).Name.ToLower())
            {
                case "btn_createoptions":
                    SurveyButtonOptions(ctrls);
                    break;
                case "btn_vote":
                    SurveyButtonVote((uc_Survey)((Button)sender).FindForm());
                    break;
                case "btn_edit":
                    SurveyButtonEdit(ctrls);
                    break;
                case "btn_new":
                    SurveyButtonNew(ctrls);
                    break;
            }
        }

        #endregion

        #endregion

        #region Private methods

        /// <summary>
        /// Creates a dock content panel and returns its instance
        /// </summary>
        /// <param name="pType">The enumeration of panel type to create</param>
        /// <returns>DockContent representing the instance created</returns>
        private DockContent CreateContentPanel(UiType pType)
        {
            DockContent panel = null;
            switch (pType)
            {
                case UiType.Account:
                    panel = new uc_Account();
                    break;
                case UiType.Project:
                    panel = new uc_Project();
                    break;
                case UiType.Survey:
                    panel = new uc_Survey();
                    break;
                case UiType.Task:
                    panel = new uc_Task();
                    break;
                case UiType.User:
                    panel = new uc_User();
                    break;
                case UiType.Expense:
                    panel = new uc_Expense();
                    break;
                case UiType.Timeslice:
                    panel = new uc_Timeslice();
                    break;
                default:
                    break;
            }
            panel.Show(_MainUI.Panel, DockState.Document);
            panel.MdiParent = _MainUI;
            return panel;
        }

        /// <summary>
        /// This method loops through all controls on a form and adds them to a list
        /// </summary>
        /// <param name="container">The control to loop through</param>
        /// <returns>List of Controls that were found on the form</returns>
        private List<Control> GetAllControls(Control container)
        {
            List<Control> ctrlList = new List<Control>();
            foreach (Control c in container.Controls)
            {
                if (c.HasChildren)
                {
                    ctrlList.Add(c);
                    ctrlList.AddRange(GetAllControls(c));
                }
                else
                    ctrlList.Add(c);
            }
            return ctrlList;
        }

        /// <summary>
        /// This generic method returns the control fo type specified, which is named as provided in the controlName
        /// </summary>
        /// <typeparam name="T">The type of control to use</typeparam>
        /// <param name="pContainer">The parent container (form), which hosts the control</param>
        /// <param name="pControlName">The name of the control</param>
        /// <returns>Control of type T specified</returns>
        private T GetControlByName<T>(Control pContainer, string pControlName)
        {
            object control = default(T);
            try
            {
                List<Control> ctrls = GetAllControls(pContainer);
                foreach (var variable in ctrls)
                {
                    if (variable.GetType() == typeof (T))
                    {
                        if (variable.Name.ToLower() == pControlName.ToLower())
                            return (T) (control = variable);
                    }
                }
                return (T) control;
            }
            catch (Exception exp)
            {
                throw new Exception($"Expection while trying to get control of type {typeof (T).Name} and name {pControlName}. Excption message= {exp.Message}");
            }
        }

        /// <summary>
        /// This generic method was optimized to improved performance, as you can input the list of controls contained on the form,
        /// rather than generating the list of contained controls at with every call
        /// </summary>
        /// <typeparam name="T">The type of control to use</typeparam>
        /// <param name="pControls">The list of controls to search</param>
        /// <param name="pControlName">The name of the control</param>
        /// <returns>Control of type T specified</returns>
        private T GetControlByName<T>(List<Control> pControls, string pControlName)
        {
            object control = default(T);
            try
            {
                foreach (var variable in pControls)
                {
                    if (variable.GetType() == typeof (T))
                    {
                        if (variable.Name.ToLower() == pControlName.ToLower())
                            return (T) (control = variable);
                    }
                }
                return (T) control;
            }
            catch (Exception exp)
            {
                throw new Exception($"Expection while trying to get control of type {typeof (T).Name} and name {pControlName}. Excption message= {exp.Message}");
            }
        }

        /// <summary>
        /// This method either en- or disables the contols found on the uc_User form except controls excluded
        /// </summary>
        /// <param name="pControlList">The list of controls contained by the form</param>
        /// <param name="pExceptionList">The list of control to set opposit state</param>
        /// <param name="pIsEnabled">Indicating whether the controls should be enabled or not</param>
        private void UserButtonSetControlsState(List<Control> pControlList, List<Control> pExceptionList, bool pIsEnabled)
        {
            foreach (var control in pControlList)
            {
                if (!pExceptionList.Contains(control))
                    control.Enabled = pIsEnabled;
                else
                    control.Enabled = !pIsEnabled;
            }
        }

        /// <summary>
        /// This method deals with the New / Save button on the uc_User form
        /// </summary>
        /// <param name="pControlList">The list of controld contained on the form</param>
        private void UserButtonSave(List<Control> pControlList)
        {
            _errorProvider.Clear();
            // Initialize default values for controls
            GetControlByName<DateTimePicker>(pControlList, "dtp_Created").Value = DateTime.Now;
            GetControlByName<DateTimePicker>(pControlList, "dtp_Modified").Value = DateTime.Now;

            // Disable control list
            List<Control> expCtrls = new List<Control>();
            expCtrls.Add(GetControlByName<Button>(pControlList, "btn_Login"));
            expCtrls.Add(GetControlByName<Button>(pControlList, "btn_New"));
            UserButtonSetControlsState(pControlList, expCtrls, false);

            if (GetControlByName<Button>(pControlList, "btn_New").Text.ToLower() == "save")
            {
                // Save command
                try
                {
                    bool bError = false;
                    _MainUI.Enabled = false;

                    // Check if all fields have values
                    if (GetControlByName<TextBox>(pControlList, "tbx_FirstName").Text.Length < 1)
                    {
                        _errorProvider.SetError(GetControlByName<TextBox>(pControlList, "tbx_FirstName"),
                            "First name is not specified");
                        bError = true;
                    }
                    if (GetControlByName<TextBox>(pControlList, "tbx_LastName").Text.Length < 1)
                    {
                        _errorProvider.SetError(GetControlByName<TextBox>(pControlList, "tbx_LastName"),
                            "Last name is not specified");
                        bError = true;
                    }
                    if (GetControlByName<TextBox>(pControlList, "tbx_Email").Text.Length < 1)
                    {
                        _errorProvider.SetError(GetControlByName<TextBox>(pControlList, "tbx_Email"),
                            "Email is not specified");
                        bError = true;
                    }
                    if (GetControlByName<DateTimePicker>(pControlList, "dtp_Birthdate").Value > DateTime.Now.AddYears(-18))
                    {
                        _errorProvider.SetError(GetControlByName<DateTimePicker>(pControlList, "dtp_Birthdate"),
                            "You must be 18+ to register with this service");
                        bError = true;
                    }
                    if (GetControlByName<TextBox>(pControlList, "tbx_UserName").Text.Length < 1)
                    {
                        _errorProvider.SetError(GetControlByName<TextBox>(pControlList, "tbx_UserName"),
                            "User name is not specified");
                        bError = true;
                    }
                    if (GetControlByName<MaskedTextBox>(pControlList, "tbx_Password").Text.Length < 1)
                    {
                        _errorProvider.SetError(GetControlByName<MaskedTextBox>(pControlList, "tbx_Password"),
                            "Password name is not specified");
                        bError = true;
                    }

                    if (!bError)
                    {
                        _objectManager.UpsertUserFromForm((uc_User) pControlList[0].Parent);
                        _userContext = (User) _objectManager.UserContext;
                    }
                    else
                    {
                        // Enable control list
                        expCtrls = new List<Control>();
                        expCtrls.Add(GetControlByName<DateTimePicker>(pControlList, "dtp_Created"));
                        expCtrls.Add(GetControlByName<DateTimePicker>(pControlList, "dtp_Modified"));
                        expCtrls.Add(GetControlByName<TextBox>(pControlList, "tbx_Id"));
                        UserButtonSetControlsState(pControlList, expCtrls, true);
                        GetControlByName<Button>(pControlList, "btn_Edit").Visible = false;
                    }

                    // Enable Main GUI again
                    _MainUI.Enabled = true;
                }
                catch (Exception exp)
                {
                    _MainUI.ShowErrorMessage($"Exception while trying to save user. Exception thrown: {exp.Message}");
                    UserButtonSetControlsState(pControlList, expCtrls, true);
                }
            }
            else
            {
                // New command
                UserButtonSetControlsState(pControlList, expCtrls, true);
                GetControlByName<Button>(pControlList, "btn_New").Text = "Save";
            }
        }

        /// <summary>
        /// This mehtod deals with the Login button on the uc_User form
        /// </summary>
        /// <param name="pControlList">The list of controld contained on the form</param>
        private void UserButtonLogin(List<Control> pControlList)
        {
            try
            {
                // Disable control list
                List<Control> expCtrls = new List<Control>();
                expCtrls.Add(GetControlByName<Button>(pControlList, "btn_Login"));
                expCtrls.Add(GetControlByName<Button>(pControlList, "btn_New"));

                // There is no user context now, thus we attempt to login
                string sUserName = GetControlByName<TextBox>(pControlList, "tbx_username").Text;
                string sPassword = GetControlByName<MaskedTextBox>(pControlList, "tbx_password").Text;
                if (!string.IsNullOrEmpty(sUserName) && !string.IsNullOrEmpty(sPassword))
                {
                    _MainUI.Enabled = false;
                    _objectManager.LoginUser(sUserName, sPassword);
                    _MainUI.Enabled = true;
                    GetControlByName<Button>(pControlList, "btn_Login").Text = "Logout";
                    GetControlByName<Button>(pControlList, "btn_Edit").Visible = true;

                    // Set Control values
                    GetControlByName<TextBox>(pControlList, "tbx_Id").Text = UserContext.Id;
                    GetControlByName<TextBox>(pControlList, "tbx_FirstName").Text = UserContext.FirstName;
                    GetControlByName<TextBox>(pControlList, "tbx_MiddleName").Text = UserContext.MiddleName;
                    GetControlByName<TextBox>(pControlList, "tbx_LastName").Text = UserContext.LastName;
                    GetControlByName<TextBox>(pControlList, "tbx_UserName").Text = UserContext.Username;
                    GetControlByName<MaskedTextBox>(pControlList, "tbx_Password").Text = UserContext.Password;
                    GetControlByName<TextBox>(pControlList, "tbx_Email").Text = UserContext.EmailAddress;
                    GetControlByName<DateTimePicker>(pControlList, "dtp_Birthdate").Value = UserContext.Birthdate;
                    GetControlByName<DateTimePicker>(pControlList, "dtp_Created").Value = UserContext.CreatedDateTime;
                    GetControlByName<DateTimePicker>(pControlList, "dtp_Modified").Value = UserContext.ModifiedDateTime;

                    DockContent tmp = (DockContent) pControlList[0].FindForm();
                    tmp.TabText = $"User Details ({UserContext.Username})";

                    _MainUI.Enabled = true;

                    GetControlByName<Button>(pControlList, "btn_Login").Text = "Logout";
                    GetControlByName<Button>(pControlList, "btn_New").Text = "New";
                    GetControlByName<Button>(pControlList, "btn_Edit").Visible = true;

                    expCtrls.Add(GetControlByName<Button>(pControlList, "btn_Edit"));
                }
                else
                {
                    expCtrls.Add(GetControlByName<TextBox>(pControlList, "tbx_UserName"));
                    expCtrls.Add(GetControlByName<MaskedTextBox>(pControlList, "tbx_Password"));
                    if (string.IsNullOrEmpty(sUserName))
                        _errorProvider.SetError(GetControlByName<TextBox>(pControlList, "tbx_username"), "UserName is not specified");
                    if (string.IsNullOrEmpty(sPassword))
                        _errorProvider.SetError(GetControlByName<MaskedTextBox>(pControlList, "tbx_password"), "Password is not specified");
                }

                UserButtonSetControlsState(pControlList, expCtrls, false);
            }
            catch (Exception exp)
            {
                _MainUI.ShowErrorMessage($"Exception while trying process login/logout. Exception thrown: {exp.Message}");
            }
        }

        /// <summary>
        /// This method logsout a user
        /// </summary>
        /// <param name="pControlList">The controls contained on the uc_User form</param>
        private void UserButtonLogout(List<Control> pControlList)
        {
            _objectManager.LogoutUser();
            _userContext = (User) _objectManager.UserContext;
            uc_User tmp = (uc_User) pControlList[0].Parent;
            tmp.Close();
        }

        /// <summary>
        /// This method is called when the New button is clicked on the uc_User form
        /// </summary>
        /// <param name="pControlList">The controls contained on the uc_User form</param>
        private void UserButtonNew(List<Control> pControlList)
        {
            // Initialize default values for controls
            GetControlByName<DateTimePicker>(pControlList, "dtp_Created").Value = DateTime.Now;
            GetControlByName<DateTimePicker>(pControlList, "dtp_Modified").Value = DateTime.Now;
            GetControlByName<TextBox>(pControlList, "tbx_Id").Text = DateTime.Now.ToString(_dateTimeIdFmt);
            GetControlByName<Button>(pControlList, "btn_New").Text = "Save";

            // Disabled control list
            List<Control> expCtrls = new List<Control>();
            expCtrls.Add(GetControlByName<DateTimePicker>(pControlList, "dtp_Created"));
            expCtrls.Add(GetControlByName<DateTimePicker>(pControlList, "dtp_Modified"));
            expCtrls.Add(GetControlByName<Button>(pControlList, "btn_Login"));
            expCtrls.Add(GetControlByName<TextBox>(pControlList, "tbx_Id"));
            UserButtonSetControlsState(pControlList, expCtrls, true);
        }

        /// <summary>
        /// This method deals with the edit button on the uc_User form
        /// </summary>
        /// <param name="pControlList">The list of controld contained on the form</param>
        private void UserButtonEdit(List<Control> pControlList)
        {
            List<Control> expCtrls = new List<Control>();
            expCtrls.Add(GetControlByName<TextBox>(pControlList, "tbx_Id"));
            expCtrls.Add(GetControlByName<DateTimePicker>(pControlList, "dtp_Created"));
            expCtrls.Add(GetControlByName<DateTimePicker>(pControlList, "dtp_Modified"));
            expCtrls.Add(GetControlByName<Button>(pControlList, "btn_Login"));
            UserButtonSetControlsState(pControlList, expCtrls, true);
            GetControlByName<Button>(pControlList, "btn_New").Text = "Save";
            GetControlByName<Button>(pControlList, "btn_Edit").Visible = false;
        }

        private void TaskButtonNew(List<Control> pControlList)
        {
            // Get the form
            uc_Task tmp = (uc_Task) pControlList[0].Parent;
            tmp.TaskId = tmp.TaskId;
            tmp.MainTaskId = DateTime.Now.ToString(_dateTimeIdFmt);
            // Initialize button state
            List<Control> expCtrls = new List<Control>();
            expCtrls.Add(GetControlByName<TextBox>(pControlList, "tbx_CreatedBy"));
            expCtrls.Add(GetControlByName<TextBox>(pControlList, "tbx_ModifiedBy"));
            expCtrls.Add(GetControlByName<DateTimePicker>(pControlList, "dtp_Created"));
            expCtrls.Add(GetControlByName<DateTimePicker>(pControlList, "dtp_Modified"));
            UserButtonSetControlsState(pControlList, expCtrls, true);
            GetControlByName<Button>(pControlList, "btn_New").Text = "Save";
            GetControlByName<TextBox>(pControlList, "tbx_ModifiedBy").Text = _userContext.Username;
            GetControlByName<TextBox>(pControlList, "tbx_CreatedBy").Text = _userContext.Username;
            GetControlByName<TextBox>(pControlList, "tbx_Owner").Text = _userContext.Username;
            GetControlByName<DateTimePicker>(pControlList, "dtp_Created").Value = DateTime.Now;
            GetControlByName<DateTimePicker>(pControlList, "dtp_Modified").Value = DateTime.Now;
        }

        private void TaskButtonSave(List<Control> pControlList)
        {
            _errorProvider.Clear();
            bool isError = false;
            try
            {
                if (_userContext != null)
                {
                    if (GetControlByName<TextBox>(pControlList, "tbx_Description").Text.Length < 1)
                    {
                        isError = true;
                        _errorProvider.SetError(GetControlByName<TextBox>(pControlList, "tbx_Description"), "You must specify a description");
                    }
                    if (GetControlByName<DateTimePicker>(pControlList, "dtp_End").Value < GetControlByName<DateTimePicker>(pControlList, "dtp_Start").Value.AddDays(1))
                    {
                        isError = true;
                        _errorProvider.SetError(GetControlByName<DateTimePicker>(pControlList, "dtp_End"), "End date must be at least one day ahead of start date");
                    }

                    if (!isError)
                    {
                        // Initialize button state
                        List<Control> expCtrls = new List<Control>();
                        expCtrls.Add(GetControlByName<Button>(pControlList, "btn_Update"));
                        expCtrls.Add(GetControlByName<Button>(pControlList, "btn_New"));
                        expCtrls.Add(GetControlByName<Button>(pControlList, "btn_Edit"));
                        //expCtrls.Add(GetControlByName<DataGridView>(pControlList, "dgw_Subtasks"));
                        UserButtonSetControlsState(pControlList, expCtrls, false);
                        GetControlByName<Button>(pControlList, "btn_New").Text = "New";

                        // Get the form
                        uc_Task tmp = (uc_Task) pControlList[0].Parent;

                        ITask task = _objectManager.UpsertTaskFromForm((uc_Task) pControlList[0].Parent);
                        tmp.TabText = $"Task Details ({task.Id})";
                        _overviewPanel.AddRecentlyChangedInteraction((Task)task);
                    }
                    else
                        return;
                }
                else
                {
                    _MainUI.ShowErrorMessage("You must login first!");
                }
            }
            catch (Exception exp)
            {
                _MainUI.ShowErrorMessage($"Exception caught: {exp.Message}");
            }
        }

        private void TaskButtonUpdate(List<Control> pControlList)
        {
            // Is task loaded?
            uc_Task tmp = (uc_Task) pControlList[0].Parent;
            if (!string.IsNullOrEmpty(tmp.TaskId))
            {
                tmp.TabText = $"Task Details ({tmp.TaskId})";
                Task task = (Task)_objectManager.TaskList.First(x => x.Id == tmp.TaskId);
                frm_TaskUpdateProgress progressForm = new frm_TaskUpdateProgress();
                progressForm.SetProjectName(((Project)_objectManager.ProjectList.First(x=>x.Id==task.ProjectId)).Name);
                progressForm.SetTaskName(task.Title);
                if (progressForm.ShowDialog() == DialogResult.OK)
                {
                    // Get settings...
                    switch (progressForm.UpdateType)
                    {
                        case 1:
                            task.UdateProgress(progressForm.UpdateValue);
                            if(task.Progress>=100)
                                task.ChangeState(InteractionState.Finished);
                            break;
                        case 2:
                            task.TimesliceItems.Add((Timeslice)_objectManager.UpsertTimesliceFromForm(progressForm));
                            break;
                        case 3:
                            task.ExpenseItems.Add((Expense)_objectManager.UpsertExpenseFromForm(progressForm));
                            break;
                        default:
                            break;
                    }
                    // Update current task with settings
                }
            }
        }

        private void TaskButtonEdit(List<Control> pControlList)
        {
            foreach (Control control in pControlList)
            {
                control.Enabled = true;
            }
            GetControlByName<Button>(pControlList, "btn_Edit").Visible = false;
            GetControlByName<Button>(pControlList, "btn_New").Text= "Save";

        }

        private void TaskButtonSubTask(List<Control> pControlList)
        {
            uc_Task tmp = (uc_Task) pControlList[0].Parent;
            if (!string.IsNullOrEmpty(tmp.TaskId))
            {
                uc_Task subTask = new uc_Task();
                subTask.MainTaskId = tmp.TaskId;
                subTask.TaskId = DateTime.Now.ToString(_dateTimeIdFmt);
                subTask.TabText = $"Task Details ({subTask.TaskId})";
                subTask.Show(_MainUI.Panel, DockState.Document);
                List<Control> subTaskCrtls = GetAllControls(subTask);
                GetControlByName<Button>(subTaskCrtls, "btn_Subtask").Visible = false;
                GetControlByName<Button>(subTaskCrtls, "btn_New").Text = "Save";
                GetControlByName<TextBox>(subTaskCrtls, "tbx_ProjectName").Text = GetControlByName<TextBox>(pControlList, "tbx_ProjectName").Text;
                GetControlByName<TextBox>(subTaskCrtls, "tbx_Owner").Text = UserContext.Username;
                GetControlByName<TextBox>(subTaskCrtls, "tbx_CreatedBy").Text = UserContext.Username;
                GetControlByName<TextBox>(subTaskCrtls, "tbx_ModifiedBy").Text = UserContext.Username;
                GetControlByName<DateTimePicker>(subTaskCrtls, "dtp_Created").Value = DateTime.Now;
                GetControlByName<DateTimePicker>(subTaskCrtls, "dtp_Modified").Value = DateTime.Now;
            }
        }

        /// <summary>
        /// This mehtod deals with the Create button on the uc_Project form
        /// </summary>
        /// <param name="pControlList">The list of controld contained on the form</param>
        private void ProjectButtonCreate(List<Control> pControlList)
        {

            //Disable control list
            List<Control> expCtrls = new List<Control>();
            expCtrls.Add(GetControlByName<Button>(pControlList, "btn_Update"));
            expCtrls.Add(GetControlByName<Button>(pControlList, "btn_InviteUser"));
            expCtrls.Add(GetControlByName<Button>(pControlList, "btn_AcceptInvite"));
            expCtrls.Add(GetControlByName<Button>(pControlList, "btn_Create"));

            _MainUI.Enabled = false;
            bool bError = false;
            try
            {
                if (GetControlByName<TextBox>(pControlList, "tbx_Title").Text.Contains(";"))
                {
                    _errorProvider.SetError(GetControlByName<TextBox>(pControlList, "tbx_Title"), "The symbol ';' is not allowed in Title");
                    bError = true;
                }
                if (GetControlByName<TextBox>(pControlList, "tbx_Title").Text.Length < 1)
                {
                    _errorProvider.SetError(GetControlByName<TextBox>(pControlList, "tbx_Title"), "Title is not specified");
                    bError = true;
                }
                if (GetControlByName<DateTimePicker>(pControlList, "dtp_EndDate").Value < GetControlByName<DateTimePicker>(pControlList, "dtp_StartDate").Value)
                {
                    _errorProvider.SetError(GetControlByName<DateTimePicker>(pControlList, "dtp_EndDate"), "The project needs to end after it starts");
                    bError = true;
                }
                if (GetControlByName<DateTimePicker>(pControlList, "dtp_EndDate").Value < DateTime.Now)
                {
                    _errorProvider.SetError(GetControlByName<DateTimePicker>(pControlList, "dtp_EndDate"), "The project end needs to be in the future");
                    bError = true;
                }

                if (!bError)
                {
                    // Get the form
                    uc_Project tmp = (uc_Project) pControlList[0].Parent;

                    TimeSpan _Countdown;

                    if (GetControlByName<DateTimePicker>(pControlList, "dtp_EndDate").Value < DateTime.Now)
                    {
                        _Countdown = GetControlByName<DateTimePicker>(pControlList, "dtp_EndDate").Value.Subtract(DateTime.Now);
                    }
                    else
                    {
                        _Countdown = GetControlByName<DateTimePicker>(pControlList, "dtp_EndDate").Value.Subtract(GetControlByName<DateTimePicker>(pControlList, "dtp_StartDate").Value);
                    }



                    //Create project
                    IProject project = _objectManager.UpsertProjectFromForm((uc_Project) pControlList[0].Parent);

                    //Disable setting controls after project is created
                    GetControlByName<Button>(pControlList, "btn_Update").Enabled = true;
                    GetControlByName<Button>(pControlList, "btn_InviteUser").Enabled = true;
                    GetControlByName<CheckBox>(pControlList, "cbx_EnableBalance").Enabled = false;
                    GetControlByName<CheckBox>(pControlList, "cbx_EnableSurvey").Enabled = false;
                    GetControlByName<DateTimePicker>(pControlList, "dtp_StartDate").Enabled = false;
                    GetControlByName<DateTimePicker>(pControlList, "dtp_EndDate").Enabled = false;
                    GetControlByName<TextBox>(pControlList, "tbx_Title").Enabled = false;
                    GetControlByName<TextBox>(pControlList, "tbx_Description").Enabled = false;
                    GetControlByName<Label>(pControlList, "lbl_Countdown").Text = String.Format($"{Convert.ToInt32(_Countdown.TotalDays)}d:{_Countdown.Hours}h:{_Countdown.Minutes}min");
                    GetControlByName<Button>(pControlList, "btn_Create").Text = "Edit";
                }
                _MainUI.Enabled = true;
            }
            catch (Exception exp)
            {
                _MainUI.ShowErrorMessage($"Exception caught: {exp.Message}");
            }
        }

        /// <summary>
        /// This mehtod deals with the Edit button on the uc_Project form
        /// </summary>
        /// <param name="pControlList">The list of controld contained on the form</param>
        private void ProjectButtonEdit(List<Control> pControlList)
        {
            try
            {

                //Disable setting controls for editing project
                GetControlByName<Button>(pControlList, "btn_Update").Enabled = false;
                GetControlByName<Button>(pControlList, "btn_InviteUser").Enabled = false;
                GetControlByName<Button>(pControlList, "btn_ChangeOwner").Enabled = true;
                GetControlByName<CheckBox>(pControlList, "cbx_EnableBalance").Enabled = true;
                GetControlByName<CheckBox>(pControlList, "cbx_EnableSurvey").Enabled = true;
                GetControlByName<DateTimePicker>(pControlList, "dtp_StartDate").Enabled = true;
                GetControlByName<DateTimePicker>(pControlList, "dtp_EndDate").Enabled = true;
                GetControlByName<TextBox>(pControlList, "tbx_Title").Enabled = true;
                GetControlByName<TextBox>(pControlList, "tbx_Description").Enabled = true;
                GetControlByName<TextBox>(pControlList, "tbx_Owner").Text = UserContext.Username;
                GetControlByName<TextBox>(pControlList, "tbx_CreatedBy").Text = UserContext.Username;
                GetControlByName<TextBox>(pControlList, "tbx_ModifiedBy").Text = UserContext.Username;
                GetControlByName<DateTimePicker>(pControlList, "dtp_Created").Value = DateTime.Now;
                GetControlByName<DateTimePicker>(pControlList, "dtp_Modified").Value = DateTime.Now;
                GetControlByName<Button>(pControlList, "btn_Create").Text = "Save";
            }
            catch (Exception exp)
            {
                _MainUI.ShowErrorMessage($"Exception caught: {exp.Message}");
            }
        }

        /// <summary>
        /// This method opens a form to select a new owner for the project
        /// </summary>
        /// <param name="pControlList"></param>
        private void ProjectButtonChangeOwner(List<Control> pControlList)
        {
            List<IProject> _Project = _objectManager.ProjectList;
            List<IUser> _MemberList = null;
            string _NewOwnerId = null;

            foreach (Project pProject in _Project)
            {
                if (pProject.Name == GetControlByName<TextBox>(pControlList, "tbx_Title").Text)
                {
                    _MemberList = pProject.MemberList;
                }
            }


            // Create a new instance of the UserSelectionList class
            frm_OwnerChangeList _OwnerSelectionList = new frm_OwnerChangeList(_MemberList, _userContext.Id);

            // Show the UserSelectionList form
            _OwnerSelectionList.Show();

            if (_OwnerSelectionList.NewOwnerId != _userContext.Id)
                _NewOwnerId = _OwnerSelectionList.NewOwnerId;
        }

        /// <summary>
        /// This method opens a form to add an expense or a timeslice for the project
        /// </summary>
        /// <param name="pControlList"></param>
        private void ProjectButtonUpdatel(List<Control> pControlList)
        {
            // Is project loaded?
            uc_Project tmp = (uc_Project) pControlList[0].Parent;
            tmp.ProjectId = GetControlByName<Label>(pControlList, "lbl_Id").Text;
            if (!string.IsNullOrEmpty(tmp.ProjectId))
            {
                tmp.TabText = $"Project Details ({tmp.ProjectId})";
                frm_TaskUpdateProgress progressForm = new frm_TaskUpdateProgress();
                if (progressForm.ShowDialog() == DialogResult.OK)
                {
                    // Get settings...

                    // Update current task with settings
                }
            }
        }

        /// <summary>
        /// This method opens a form to invite users to the project
        /// </summary>
        /// <param name="pControlList"></param>
        private void ProjectButtonInviteUser(List<Control> pControlList)
        {

            List<IProject> _Project = _objectManager.ProjectList;
            string _ProjectId = null;

            foreach (Project pProject in _Project)
            {
                if (pProject.Name == GetControlByName<TextBox>(pControlList, "tbx_Title").Text)
                {
                    _ProjectId = pProject.Id;

                }
            }

            // Create a new instance of the UserSelectionList class
            frm_UserSelectionList _UserSelectionList = new frm_UserSelectionList(_ProjectId, _userContext.Id);

            // Show the UserSelectionList form
            _UserSelectionList.Show();
        }

        /// <summary>
        /// This method builds the overview for this user. It fills the dataGridView with the 
        /// objects in collection and registers dgw double click to call objectManager
        /// </summary>
        private void GenerateOverviewPanel()
        {
            _objectManager.CreateUserOverview();

            foreach (var project in _objectManager.ProjectList)
            {
                _overviewPanel.AddAssignedProjects(project);
                if (project.ModifiedDateTime > DateTime.Now.AddDays(-5))
                    _overviewPanel.AddRecentlyChangedInteraction((IInteraction) project);
            }
            foreach (var task in _objectManager.TaskList)
            {
                if (((Task) task).ModifiedDateTime > DateTime.Now.AddDays(-5))
                    _overviewPanel.AddRecentlyChangedInteraction((IInteraction) task);
            }
            foreach (var survey in _objectManager.SurveyList)
            {
                if (((Survey) survey).ModifiedDateTime > DateTime.Now.AddDays(-5))
                    _overviewPanel.AddRecentlyChangedInteraction((IInteraction) survey);
            }

            _overviewPanel.RegisterEvents();
        }

        /// <summary>
        /// This method handles button surveyOptions click
        /// </summary>
        /// <param name="pControlList">The form controls to manipulate</param>
        private void SurveyButtonOptions(List<Control> pControlList)
        {
            if ((uc_Survey) pControlList[0].FindForm() != null)
            {
                List<string> options = ((uc_Survey) pControlList[0].FindForm()).SurveyOptions.
                    Select(so => so.Text).ToList();
                frm_CreateOptions tmp = new frm_CreateOptions(options);
                if (tmp.ShowDialog() == DialogResult.OK)
                {
                    ((uc_Survey)pControlList[0].FindForm()).SurveyOptions.Clear();
                    foreach (var option in tmp.SurveyOptions)
                    {
                        if (!options.Contains(option))
                        {
                            ((uc_Survey) pControlList[0].FindForm()).SurveyOptions.Add(_objectManager.CreateSurveyOption(option));
                        }
                    }
                }
                tmp = null;
            }
        }

        /// <summary>
        /// This method handles button edit click
        /// </summary>
        /// <param name="pForm">The form to manipulate</param>
        private void SurveyButtonVote(uc_Survey pForm)
        {
            _errorProvider.Clear();
            try
            {
                // find survey
                if (_objectManager.SurveyList.Any(x => x.Id == pForm.Id))
                {
                    frm_Vote vote = new frm_Vote(((ISurvey) _objectManager.SurveyList.First(x => x.Id == pForm.Id)),
                        UserContext,
                        new ObjectFactory());
                    vote.SurveyId = _objectManager.SurveyList.First(x => x.Id == pForm.Id).Id;
                    vote.SetSurveyOptions(((ISurvey) _objectManager.SurveyList.First(x => x.Id == pForm.Id)).OptionList);
                    if (vote.ShowDialog() == DialogResult.OK)
                    {
                        _objectManager.UpsertObject(vote.Vote);
                        _objectManager.SurveyList.First(x => x.Id == pForm.Id).AddVote(vote.Vote);
                        vote = null;
                    }
                }
                else
                {
                    _errorProvider.SetError(pForm.ActiveControl, "The survey was not found in object manager");
                }
            }
            catch (Exception exp)
            {
                _MainUI.ShowErrorMessage(exp.Message);
            }
        }

        /// <summary>
        /// This method handles button new click
        /// </summary>
        /// <param name="pControlList">The form control to manipulate</param>
        private void SurveyButtonNew(List<Control> pControlList)
        {
            if (GetControlByName<Button>(pControlList, "btn_New").Text.ToLower() == "new")
            {
                ((uc_Survey) pControlList[0].Parent).Id = DateTime.Now.ToString(_dateTimeIdFmt);
                ((uc_Survey)pControlList[0].Parent).InteractionId = DateTime.Now.AddMilliseconds(10).ToString(_dateTimeIdFmt);
                GetControlByName<TextBox>(pControlList, "tbx_CreatedBy").Text = UserContext.Username;
                GetControlByName<TextBox>(pControlList, "tbx_ModifiedBy").Text = UserContext.Username;
                GetControlByName<DateTimePicker>(pControlList, "dtp_CreatedAt").Value = DateTime.Now;
                GetControlByName<DateTimePicker>(pControlList, "dtp_ModifiedAt").Value = DateTime.Now;
                GetControlByName<TextBox>(pControlList, "tbx_SurveyVoteCount").Text = "0";
                GetControlByName<NumericUpDown>(pControlList, "num_VotesPerUser").Value = 1;
                GetControlByName<GroupBox>(pControlList, "groupBox2").Enabled = true;
                GetControlByName<Button>(pControlList, "btn_New").Text = "Save";
                GetControlByName<Button>(pControlList, "btn_Edit").Visible = false;
            }
            else
            {
                GetControlByName<Button>(pControlList, "btn_New").Text = "New";
                GetControlByName<Button>(pControlList, "btn_Edit").Visible = true;
                _objectManager.UpsertSurveyFromForm((uc_Survey) pControlList[0].Parent);
                ISurvey tmp = _objectManager.SurveyList.First(x => x.Id == ((uc_Survey) pControlList[0].Parent).Id);
                foreach (var option in tmp.OptionList)
                {
                    ((uc_Survey)pControlList[0].Parent).AddVoteOptions(option);
                }
                GetControlByName<GroupBox>(pControlList, "groupBox2").Enabled = false;
                GetControlByName<Button>(pControlList, "btn_Edit").Visible = true;
                _overviewPanel.AddRecentlyChangedInteraction((Survey)tmp);
            }
        }

        /// <summary>
        /// This method handles button edit click
        /// </summary>
        /// <param name="pControlList">The form control to manipulate</param>
        private void SurveyButtonEdit(List<Control> pControlList)
        {
            GetControlByName<GroupBox>(pControlList, "groupBox2").Enabled = true;
            GetControlByName<Button>(pControlList, "btn_New").Text = "Save";
            GetControlByName<Button>(pControlList, "btn_Edit").Visible = false;
        }

        private void AccountButtonAssign(List<Control> pControlList)
        {
            frm_ProjectSelectionList progressForm = new frm_ProjectSelectionList();

            foreach (var project in _objectManager.ProjectList)
            {
                progressForm.AddAssignedProjects(project);
            }
            if (progressForm.ShowDialog() == DialogResult.OK)
            {
                string projectId = (progressForm.SelectedProjectId);
            }
            
        }

        private void AccountButtonUpdate(List<Control> ctrls)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
