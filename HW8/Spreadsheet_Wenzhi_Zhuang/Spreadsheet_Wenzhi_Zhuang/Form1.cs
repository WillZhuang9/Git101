// <copyright file="Form1.cs" company="Wenzhi Zhuang">
// Copyright (c) Wenzhi Zhuang. All rights reserved.
//  Programmer: Wenzhi Zhuang, ID: 11632272
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CptS321;

namespace Spreadsheet_Wenzhi_Zhuang
{
    /// <summary>
    /// Intilizie a form to construct a spreadsheet.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// initilize the inside component and construct a spreadsheet.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.ConstructSpreadsheet();
        }

        /// <summary>
        /// Construct a spreadsheet.
        /// Set the number of columns and rows for dataGridView firstly.
        /// Then construct a variable type of spreadsheet to update the current modification in dataGridView.
        /// </summary>
        public void ConstructSpreadsheet()
        {
            this.CreateColumns();
            this.CreateRows();
            this.MySpreadsheet = new Spreadsheet(50, 26);
            this.MySpreadsheet.PropertyChanged += this.CellPropertyChangedEventHandler;
            this.undoToolStripMenuItem.Enabled = false;
            this.redoToolStripMenuItem.Enabled = false;
        }

        /// <summary>
        /// Handle the propertyChanged event inside the spreadsheet class varaiable.
        /// Once the property's change was detected, the the spreadsheet class will automactically drop into current function to do a inside updating in dataGridView.
        /// </summary>
        /// <param name="sender"> The varaible in Spreadsheet class type. </param>
        /// <param name="e"> The variable of string that mention what is the property changed.</param>
        private void CellPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.CompareTo("Text") == 0)
            {
                this.dataGridView1.Rows[((SpreadsheetCell)sender).RowIndex].Cells[((SpreadsheetCell)sender).ColumeIndex].Value = this.MySpreadsheet.Cells[((SpreadsheetCell)sender).RowIndex, ((SpreadsheetCell)sender).ColumeIndex].Value;
            }
            else
            {
                this.dataGridView1.Rows[((SpreadsheetCell)sender).RowIndex].Cells[((SpreadsheetCell)sender).ColumeIndex].Style.BackColor = Color.FromArgb((int)this.MySpreadsheet.Cells[((SpreadsheetCell)sender).RowIndex, ((SpreadsheetCell)sender).ColumeIndex].BGColor);
            }
        }

        /// <summary>
        /// Set and construct a number of columns for dataGridView, which was defined in From1.Designer.cs.
        /// </summary>
        private void CreateColumns()
        {
            this.dataGridView1.ColumnCount = 26;
            char columnChar = 'A';
            for (int i = 0; i < 26; i++)
            {
                this.dataGridView1.Columns[i].Name = ((char)(columnChar + i)).ToString();
            }
        }

        /// <summary>
        /// Set and construct a number of rows for dataGridView, which was defined in From1.Designer.cs.
        /// </summary>
        private void CreateRows()
        {
            this.dataGridView1.RowCount = 50;
            for (int i = 0; i < 50; i++)
            {
                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
        }

        /// <summary>
        /// Button of perform demo in the form.
        /// </summary>
        /// <param name="sender"> String information: "Perform Demo".</param>
        /// <param name="e"> Position of button1 icon in the form.</param>
        private void Button1_Click(object sender, System.EventArgs e)
        {
            this.Text = "Running demo";

            // Input the value without a '=' at the beginning of the string.
            for (int i = 0; i < 50; i++)
            {
                this.MySpreadsheet.Cells[i, 0].Text = "I love C#";
            }

            // Input a value of string "=A#" to copy the information from colume A.
            for (int i = 0; i < 50; i++)
            {
                this.MySpreadsheet.Cells[i, 5].Text = "=A" + (i + 1).ToString();
            }
        }

        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.MySpreadsheet.Cells[e.RowIndex, e.ColumnIndex].Text;
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            SpreadsheetCell prevCell = this.MySpreadsheet.GetCell(e.RowIndex, e.ColumnIndex).CreateCopy();
            this.MySpreadsheet.PushUndoEvent(prevCell, "Text change");

            this.undoToolStripMenuItem.Enabled = true;
            this.undoToolStripMenuItem.Text = "Undo " + this.MySpreadsheet.PeekUedo();

            this.MySpreadsheet.Cells[e.RowIndex, e.ColumnIndex].Text = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.MySpreadsheet.Cells[e.RowIndex, e.ColumnIndex].Value;
        }

        private void changeColorOfSelectedCellsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.dataGridView1.SelectedCells.Count != 0)
            {
                ColorDialog myDialog = new ColorDialog();
                myDialog.AllowFullOpen = false;
                myDialog.ShowHelp = true;
                myDialog.Color = this.dataGridView1.SelectedCells[0].Style.BackColor;
                myDialog.ShowDialog();
                for (int i = 0; i < this.dataGridView1.SelectedCells.Count; i++)
                {
                    var prevCell = this.MySpreadsheet.GetCell(this.dataGridView1.SelectedCells[i].RowIndex, this.dataGridView1.SelectedCells[i].ColumnIndex).CreateCopy();
                    this.MySpreadsheet.PushUndoEvent(prevCell, "Color change");

                    this.undoToolStripMenuItem.Enabled = true;
                    this.MySpreadsheet.Cells[this.dataGridView1.SelectedCells[i].RowIndex, this.dataGridView1.SelectedCells[i].ColumnIndex].BGColor =
                        (uint)((myDialog.Color.A << 24)
                            | (myDialog.Color.R << 16)
                            | (myDialog.Color.G << 8)
                            | (myDialog.Color.B << 0));
                }
            }
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var undoneCell = this.MySpreadsheet.Undo();
            if (undoneCell != null)
            {
                this.MySpreadsheet.PushRedoEvent(undoneCell);
                this.redoToolStripMenuItem.Enabled = true;
                this.redoToolStripMenuItem.Text = "Redo " + this.MySpreadsheet.PeekRedo();
            }

            if (this.MySpreadsheet.unDoStack.Count == 0)
            {
                this.undoToolStripMenuItem.Enabled = false;

            }
            else
            {
                this.undoToolStripMenuItem.Enabled = true;
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var redoneCell = this.MySpreadsheet.Redo();
            if (redoneCell != null)
            {
                this.MySpreadsheet.PushUndoEvent(redoneCell.oldCell, redoneCell.changedPropertyName);
                this.undoToolStripMenuItem.Enabled = true;
                this.undoToolStripMenuItem.Text = "Undo " + this.MySpreadsheet.PeekUedo();
            }

            if (this.MySpreadsheet.unDoStack.Count == 0)
            {
                this.redoToolStripMenuItem.Enabled = false;
            }
            else
            {
                this.redoToolStripMenuItem.Enabled = true;
            }
        }
    }
}
