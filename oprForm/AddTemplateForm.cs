﻿using Data;
using Data.Entity;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Drawing;

namespace oprForm
{
    public partial class AddTemplateForm : Form
    {
        private DBManager db = new DBManager();
        private int user = 1;
        private int valueCol = 2;
        private int descCol = 1;

        /* Begin - Серая подсказка для TextBox, когда пустое TextBox.Text*/

        TextBox[] txtBxMas = new TextBox[3]; //= { txtBxTemplate, txtBxRes, evNameTB, descTB };
        string[] placeholderMas = { "Пошук по ресурсам", "Введіть назву шаблону заходу", "Введіть опис шаблону заходу" };

        private void PlaceholderTxtBx(TextBox txtBxName, string placeholder)
        {
            txtBxName.ForeColor = SystemColors.GrayText;
            txtBxName.Text = placeholder;
            txtBxName.Leave += TxtBx_Leave;
            txtBxName.Enter += TxtBx_Enter;
        }

        private void TxtBx_Enter(object sender, EventArgs e)
        {
            // throw new NotImplementedException();

            TextBox txtBx = sender as TextBox;
            bool check = false;
            for (int i = 0; i < placeholderMas.Length; i++) if (txtBx.Text == placeholderMas[i]) check = true;
            if (check)
            {
                txtBx.Text = "";
                txtBx.ForeColor = SystemColors.WindowText;
            }
        }

        private void TxtBx_Leave(object sender, EventArgs e)
        {
            //throw new NotImplementedException();

            TextBox txtBx = sender as TextBox;
            string placeholder = "";

            if (txtBx.Text.Length == 0)
            {
                for (int i = 0; i < txtBxMas.Length; i++) if (txtBx.Name == txtBxMas[i].Name) placeholder = placeholderMas[i];
                txtBx.Text = placeholder;
                txtBx.ForeColor = SystemColors.GrayText;
            }

        }
        /* End - Серая подсказка для TextBox, когда пустое TextBox.Text*/

        public AddTemplateForm()
        {
            InitializeComponent();

            /* Begin - Серая подсказка для TextBox, когда пустое TextBox.Text*/
            
            txtBxMas[0] = txtBxRes;
            txtBxMas[1] = nameTB;
            txtBxMas[2] = descTB;
            for (int i = 0; i < txtBxMas.Length; i++) PlaceholderTxtBx(txtBxMas[i], placeholderMas[i]);

            /* End - Серая подсказка для TextBox, когда пустое TextBox.Text*/
        }

        private void AddResourceToGrid()
        {
            Resource res = resourcesLB.SelectedItem as Resource;

            if (res == null)
            {
                return;
            }

            foreach (DataGridViewRow row in materialListGrid.Rows)
            {
                if (row.Cells[0].Value == res)
                    return;
            }

            materialListGrid.Rows.Add(res, res.Description);
        }
        
        private void RemoveResourceFromGrid()
        {
            if (materialListGrid.CurrentRow == null)
            {
                return;
            }

            materialListGrid.Rows.Remove(materialListGrid.CurrentRow);
        }

        private void AddTemplateForm_Load(object sender, EventArgs e)
        {
            db.Connect();
            var obj = db.GetRows("resource", "*", "");
            var resources = new List<Resource>();
            foreach (var row in obj)
            {
                resources.Add(ResourceMapper.Map(row));
            }

            resourcesLB.Items.AddRange(resources.ToArray());
            db.Disconnect();
        }

        private void commitValue(object sender, DataGridViewCellEventArgs e)
        {
            Resource res = materialListGrid.Rows[e.RowIndex].Cells[0].Value as Resource;
            if (e.RowIndex == valueCol)
                res.Value = Int32.Parse(materialListGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            if (e.RowIndex == descCol)
                res.Description = materialListGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            db.Connect();
            string temName = DBUtil.AddQuotes(nameTB.Text);
            string temDesc = DBUtil.AddQuotes(descTB.Text);

            string[] evFields = new string[] { "name", "description", "expert_id" };
            string[] evValues = new string[] { temName, temDesc, user.ToString() };

            int evId = db.InsertToBD("event_template", evFields, evValues);

            foreach (DataGridViewRow row in materialListGrid.Rows)
            {
                Resource res = row.Cells[0].Value as Resource;
                if (res != null)
                {
                    string[] fields = { "template_id", "resource_id" };
                    string[] values = { evId.ToString(), res.Id.ToString() };

                    db.InsertToBD("template_resource", fields, values);
                }
            }
            db.Disconnect();

            MessageBox.Show("Шаблон " + nameTB.Text + " додано.");
        }

        private void resourcesLB_DoubleClick(object sender, EventArgs e)
        {
            AddResourceToGrid();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            AddResourceToGrid();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            RemoveResourceFromGrid();
        }


    }
}