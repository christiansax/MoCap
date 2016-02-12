﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using PlexByte.MoCap.Logging;

namespace PlexByte.MoCap.WinForms
{
    public partial class frmTraceViewer : Form
    {
        public delegate void UpdateStatusLabelHandler(string pValue);
        public delegate void UpdateProgressbarHandler(int pValue);
        public delegate void AddFormatedMessageHandler(TraceMessageAdapter pMessage);
        public delegate void ReadLogFileHandler(string pFullPath);

        private ILogObject _log = null;
        private int _curRow = -1;
        private bool _isTimerActive = false;
        private System.Timers.Timer _refreshTimer = null;

        public frmTraceViewer()
        {
            InitializeComponent();
            Thread.CurrentThread.Name = "LogViewer_Main";
            InitializeToolTips();
            toolStripStatusLabel1.Text = "Ready";
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.DefaultExt = "Trace Files|*.trc;*.mocapLog;";
            openFileDialog1.Title = "Select log file to open";
            openFileDialog1.FileName = "MoCap.trc";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                new Thread(delegate () {
                    ReadLogFile(openFileDialog1.FileName);
                }).Start();
            }
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // Open file
            openToolStripMenuItem_Click(sender, e);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // Refresh opened file
            if (_log != null)
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                openToolStripMenuItem_Click(sender, e);
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            // Auto refresh
            toolStripButton3.CheckOnClick = !toolStripButton3.CheckOnClick;
            toolStripButton3.Checked = !toolStripButton3.Checked;
            if (!_isTimerActive && toolStripButton3.Checked && _log!=null)
            {
                _isTimerActive = true;
                _refreshTimer=new System.Timers.Timer(10000);
                _refreshTimer.Elapsed += Timer_Elapsed;
                _refreshTimer.Start();
            }
            else
            {
                if (_refreshTimer != null)
                {
                    _isTimerActive = false;
                    _refreshTimer.Stop();
                    _refreshTimer = null;
                }
            }
        }

        void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_log != null)
            {
                int numMsg = 0;
                ReadLogFile(_log.LogPath);
            }
        }

        private void InitializeToolTips()
        {
            toolStripButton1.ToolTipText = "Open";
            toolStripButton2.ToolTipText = "Refresh";
            toolStripButton3.ToolTipText = "Auto Refresh";
            toolStripButton4.ToolTipText = "Bookmark";
            toolStripButton5.ToolTipText = "Jump to date time";
            toolStripButton6.ToolTipText = "Filter";
            toolStripButton7.ToolTipText = "Clear Bookmark";
            toolStripButton8.ToolTipText = "Clear date time selector";
            toolStripButton9.ToolTipText = "Clear Filter";
            toolStripButton10.ToolTipText = "Find String";
        }

        private void ReadLogFile(string pFullFilePath)
        {
            if (this.InvokeRequired)
            {
                var d = new ReadLogFileHandler(ReadLogFile);
                this.Invoke(d, new object[] { pFullFilePath });
            }
            else
            {
                try
                {
                 _log=new BinaryLogFile();   
                    string tracePrefix = "   ";
                    int numMsg = 0;
                    long currNum = 0;
                    List<ITraceObject> messages= _log.ReadLogFileRaw(out numMsg, pFullFilePath);

                    TimeSpan duration = DateTime.Now.TimeOfDay;
                    foreach (ITraceObject msg in messages)
                    {
                        currNum++;
                        UpdateStatusLabel($"Reading {currNum} out of {numMsg} Messages...".ToString());
                        AddFormatedMessage(new TraceMessageAdapter(msg, tracePrefix));
                        UpdateProgressBar(Convert.ToInt32(Convert.ToDouble(currNum)/Convert.ToDouble(numMsg)*100));
                    }
                    duration = DateTime.Now.TimeOfDay - duration;
                    UpdateStatusLabel(
                        $"All messages read in {duration.Seconds}.{((duration.Milliseconds < 100) ? "0" + duration.Milliseconds.ToString() : duration.Milliseconds.ToString())} seconds [TotalMessage={numMsg}]");
                    UpdateProgressBar(0);
                }
                catch
                {
                }
            }
        }

        private void AddFormatedMessage(TraceMessageAdapter pMessage)
        {
            if (this.InvokeRequired)
            {
                var d = new AddFormatedMessageHandler(AddFormatedMessage);
                this.Invoke(d, new object[] { pMessage });
            }
            else
                traceMessageAdapterBindingSource.Add(pMessage);
        }

        public void UpdateStatusLabel(string pValue)
        {
            if (this.InvokeRequired)
            {
                UpdateStatusLabelHandler d = new UpdateStatusLabelHandler(UpdateStatusLabel);
                this.Invoke(d, new object[] { pValue });
            }
            else
                toolStripStatusLabel1.Text = pValue;
        }

        private void UpdateProgressBar(int pValue)
        {
            if (InvokeRequired)
            {
                var d = new UpdateProgressbarHandler(UpdateProgressBar);
                this.Invoke(d, new object[] { pValue });
            }
            else
                toolStripProgressBar1.Value = pValue;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (((TraceMessageAdapter)row.DataBoundItem).MessageColor != System.Drawing.Color.Transparent)
                    row.DefaultCellStyle.BackColor = ((TraceMessageAdapter)row.DataBoundItem).MessageColor;
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != _curRow)
            {
                TraceMessageAdapter fmt = (TraceMessageAdapter)dataGridView1.CurrentRow.DataBoundItem;
                StringBuilder sb = new StringBuilder();
                sb.Append(fmt.Source);
                sb.Append(Environment.NewLine);
                sb.Append(fmt.Component + "." + fmt.ScopedMethod);
                sb.Append($" on thread {fmt.ThreadId}");
                sb.Append(Environment.NewLine);
                sb.Append("Line " + fmt.LineNumber);
                sb.Append("\t\t\t");
                sb.Append("Level " + fmt.Level);
                sb.Append("\t\t\t");
                sb.Append("At " + fmt.Time);
                sb.Append(Environment.NewLine);
                sb.Append(Environment.NewLine);
                sb.Append(fmt.Message);
                tbx_MessageDetail.Text = sb.ToString();
                sb = null;
                fmt = null;
            }
            _curRow = dataGridView1.CurrentRow.Index;
        }
    }
}