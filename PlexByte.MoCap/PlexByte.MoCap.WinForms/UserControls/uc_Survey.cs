﻿//////////////////////////////////////////////////////////////
//                      Windows form uc_Task                              
//      Author: Christian B. Sax            Date:   2016/02/14
//      The task details form containing any task detail and is docked as document type in the main form
using WeifenLuo.WinFormsUI.Docking;

namespace PlexByte.MoCap.WinForms.UserControls
{
    public partial class uc_Survey : DockContent
    {
        public string Id { get; set; }
        public string InteractionId { get; set; }
        public string ProjectId { get; set; }

        private const string PanelTitle = "Survey Details";

        public uc_Survey()
        {
            InitializeComponent();
            this.TabText = PanelTitle;
        }

        public void AddVoteOptions(string pOption) { lv_Otions.Items.Add(pOption); }

        public void RegisterEvents(UIManager pEventHandler)
        {
            btn_CreateOptions.Click += pEventHandler.SurveyButtonClicked;
            btn_Edit.Click += pEventHandler.SurveyButtonClicked;
            btn_New.Click += pEventHandler.SurveyButtonClicked;
            btn_Vote.Click += pEventHandler.SurveyButtonClicked;
        }

        public void UnRegisterEvents(UIManager pEventHandler)
        {
            btn_CreateOptions.Click -= pEventHandler.SurveyButtonClicked;
            btn_Edit.Click -= pEventHandler.SurveyButtonClicked;
            btn_New.Click -= pEventHandler.SurveyButtonClicked;
            btn_Vote.Click -= pEventHandler.SurveyButtonClicked;
        }

        private void cbx_Project_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            ProjectId = cbx_Project.Text;
        }
    }
}
