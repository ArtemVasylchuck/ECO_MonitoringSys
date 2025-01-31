﻿
namespace UserMap.UserControls
{
    partial class MedStatUserControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            if (dbManager != null)
            {
                dbManager.Dispose();
            }



            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.MedStatParamLable = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.MedStatParamDataGridView = new System.Windows.Forms.DataGridView();
            this.MedStatDataGridView = new System.Windows.Forms.DataGridView();
            this.MedStatIndicatorsTreeView = new System.Windows.Forms.TreeView();
            this.FiltrationGroupBox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SelectedIndicatorsListBox = new System.Windows.Forms.ListBox();
            this.AcceptFiltrarionButton = new System.Windows.Forms.Button();
            this.MedStatYearsTreeView = new System.Windows.Forms.TreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.MedStatParamDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MedStatDataGridView)).BeginInit();
            this.FiltrationGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MedStatParamLable
            // 
            this.MedStatParamLable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MedStatParamLable.AutoSize = true;
            this.MedStatParamLable.Location = new System.Drawing.Point(367, 9);
            this.MedStatParamLable.Name = "MedStatParamLable";
            this.MedStatParamLable.Size = new System.Drawing.Size(193, 13);
            this.MedStatParamLable.TabIndex = 47;
            this.MedStatParamLable.Text = "Параметри по вибраному показнику";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(111, 13);
            this.label9.TabIndex = 46;
            this.label9.Text = "Медична статистика";
            // 
            // MedStatParamDataGridView
            // 
            this.MedStatParamDataGridView.AllowUserToAddRows = false;
            this.MedStatParamDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MedStatParamDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MedStatParamDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.MedStatParamDataGridView.Location = new System.Drawing.Point(370, 25);
            this.MedStatParamDataGridView.MinimumSize = new System.Drawing.Size(350, 150);
            this.MedStatParamDataGridView.Name = "MedStatParamDataGridView";
            this.MedStatParamDataGridView.ReadOnly = true;
            this.MedStatParamDataGridView.Size = new System.Drawing.Size(350, 319);
            this.MedStatParamDataGridView.TabIndex = 45;
            this.MedStatParamDataGridView.TabStop = false;
            this.MedStatParamDataGridView.Tag = "2";
            this.MedStatParamDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.MedStatDataGridView_CellFormatting);
            this.MedStatParamDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.MedStatDataGridView_DataError);
            // 
            // MedStatDataGridView
            // 
            this.MedStatDataGridView.AllowUserToAddRows = false;
            this.MedStatDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MedStatDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.MedStatDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MedStatDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.MedStatDataGridView.Location = new System.Drawing.Point(14, 26);
            this.MedStatDataGridView.MinimumSize = new System.Drawing.Size(350, 150);
            this.MedStatDataGridView.Name = "MedStatDataGridView";
            this.MedStatDataGridView.ReadOnly = true;
            this.MedStatDataGridView.Size = new System.Drawing.Size(350, 318);
            this.MedStatDataGridView.TabIndex = 44;
            this.MedStatDataGridView.TabStop = false;
            this.MedStatDataGridView.Tag = "1";
            this.MedStatDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.MedStatDataGridView_CellFormatting);
            this.MedStatDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.MedStatDataGridView_DataError);
            this.MedStatDataGridView.SelectionChanged += new System.EventHandler(this.MedStatDataGridView_SelectionChanged);
            // 
            // MedStatIndicatorsTreeView
            // 
            this.MedStatIndicatorsTreeView.CheckBoxes = true;
            this.MedStatIndicatorsTreeView.Location = new System.Drawing.Point(6, 33);
            this.MedStatIndicatorsTreeView.Name = "MedStatIndicatorsTreeView";
            this.MedStatIndicatorsTreeView.PathSeparator = " ";
            this.MedStatIndicatorsTreeView.Size = new System.Drawing.Size(325, 123);
            this.MedStatIndicatorsTreeView.TabIndex = 48;
            this.MedStatIndicatorsTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.ResetFilterButtonDelay_AfterCheck);
            // 
            // FiltrationGroupBox
            // 
            this.FiltrationGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FiltrationGroupBox.Controls.Add(this.label3);
            this.FiltrationGroupBox.Controls.Add(this.SelectedIndicatorsListBox);
            this.FiltrationGroupBox.Controls.Add(this.MedStatYearsTreeView);
            this.FiltrationGroupBox.Controls.Add(this.label2);
            this.FiltrationGroupBox.Controls.Add(this.label1);
            this.FiltrationGroupBox.Controls.Add(this.MedStatIndicatorsTreeView);
            this.FiltrationGroupBox.Location = new System.Drawing.Point(726, 9);
            this.FiltrationGroupBox.Name = "FiltrationGroupBox";
            this.FiltrationGroupBox.Size = new System.Drawing.Size(517, 303);
            this.FiltrationGroupBox.TabIndex = 50;
            this.FiltrationGroupBox.TabStop = false;
            this.FiltrationGroupBox.Text = "Налаштування фільтрації";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 13);
            this.label3.TabIndex = 54;
            this.label3.Text = "Вибрані показники";
            // 
            // SelectedIndicatorsListBox
            // 
            this.SelectedIndicatorsListBox.FormattingEnabled = true;
            this.SelectedIndicatorsListBox.HorizontalScrollbar = true;
            this.SelectedIndicatorsListBox.Location = new System.Drawing.Point(9, 189);
            this.SelectedIndicatorsListBox.Name = "SelectedIndicatorsListBox";
            this.SelectedIndicatorsListBox.Size = new System.Drawing.Size(502, 108);
            this.SelectedIndicatorsListBox.TabIndex = 53;
            // 
            // AcceptFiltrarionButton
            // 
            this.AcceptFiltrarionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AcceptFiltrarionButton.Location = new System.Drawing.Point(726, 321);
            this.AcceptFiltrarionButton.Name = "AcceptFiltrarionButton";
            this.AcceptFiltrarionButton.Size = new System.Drawing.Size(517, 23);
            this.AcceptFiltrarionButton.TabIndex = 52;
            this.AcceptFiltrarionButton.Text = "Фільтрувати";
            this.AcceptFiltrarionButton.UseVisualStyleBackColor = true;
            this.AcceptFiltrarionButton.Click += new System.EventHandler(this.AcceptFiltrarionButton_Click);
            // 
            // MedStatYearsTreeView
            // 
            this.MedStatYearsTreeView.CheckBoxes = true;
            this.MedStatYearsTreeView.Location = new System.Drawing.Point(352, 33);
            this.MedStatYearsTreeView.Name = "MedStatYearsTreeView";
            this.MedStatYearsTreeView.Size = new System.Drawing.Size(159, 123);
            this.MedStatYearsTreeView.TabIndex = 51;
            this.MedStatYearsTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.ResetFilterButtonDelay_AfterCheck);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(349, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 50;
            this.label2.Text = "Роки";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 49;
            this.label1.Text = "Показники";
            // 
            // MedStatUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FiltrationGroupBox);
            this.Controls.Add(this.MedStatParamLable);
            this.Controls.Add(this.AcceptFiltrarionButton);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.MedStatParamDataGridView);
            this.Controls.Add(this.MedStatDataGridView);
            this.MinimumSize = new System.Drawing.Size(1250, 320);
            this.Name = "MedStatUserControl";
            this.Size = new System.Drawing.Size(1250, 352);
            this.Load += new System.EventHandler(this.MedStatUserControl_Load);
            this.Resize += new System.EventHandler(this.MedStatUserControlTabControl_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.MedStatParamDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MedStatDataGridView)).EndInit();
            this.FiltrationGroupBox.ResumeLayout(false);
            this.FiltrationGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MedStatParamLable;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView MedStatParamDataGridView;
        private System.Windows.Forms.DataGridView MedStatDataGridView;
        private System.Windows.Forms.TreeView MedStatIndicatorsTreeView;
        private System.Windows.Forms.GroupBox FiltrationGroupBox;
        private System.Windows.Forms.TreeView MedStatYearsTreeView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AcceptFiltrarionButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox SelectedIndicatorsListBox;
    }
}
