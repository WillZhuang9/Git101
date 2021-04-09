// <copyright file="SpreadsheetCell.cs" company="Wenzhi Zhuang">
// Copyright (c) Wenzhi Zhuang. All rights reserved.
//  Programmer: Wenzhi Zhuang, ID: 11632272
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// Construct a single cell by inheritancing INotifyPropertyChanged.
    /// </summary>
    public class SpreadsheetCell : INotifyPropertyChanged
    {
        /// <summary>
        /// Color setting.
        /// </summary>
        public uint bgColor;
        /// <summary>
        /// Gets or sets the Row position for current cell.
        /// </summary>
        public int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets the Colume position for current cell.
        /// </summary>
        public int ColumeIndex { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadsheetCell"/> class.
        /// Initilize all nececcary compoments inside SpreadsheetCell.
        /// </summary>
        /// <param name="row">Set the row for current cell.</param>
        /// <param name="column">Set the colume for current cell.</param>
        public SpreadsheetCell(int row, int column)
        {
            this.RowIndex = row;
            this.ColumeIndex = column;
            this.textcontent = " ";
            this.valuecontent = " ";
            this.bgColor = 0xFFFFFFFF;
        }

        public SpreadsheetCell CreateCopy()
        {
            SpreadsheetCell newCell = new SpreadsheetCell(this.RowIndex, this.ColumeIndex);
            newCell.Text = this.Text;
            newCell.BGColor = this.BGColor;

            return newCell;
        }
        /// <summary>
        /// The content inside string Text.
        /// </summary>
        protected string textcontent;

        /// <summary>
        /// Gets or sets the Text's value.
        /// </summary>
        public string Text
        {
            get
            {
                return this.textcontent;
            }

            set
            {
                if (this.textcontent != value)
                {
                    this.textcontent = value;
                    this.OnPropertyChanged("Text");
                }
            }
        }

        /// <summary>
        /// The content inside string Value.
        /// </summary>
        protected string valuecontent;

        /// <summary>
        /// Gets or sets the Value's value.
        /// </summary>
        public string Value
        {
            get
            {
                if (this.textcontent.Length != 0)
                {
                    if (this.textcontent[0] == '=')
                    {
                        return this.valuecontent;
                    }
                }

                return this.textcontent;
            }

            set
            {
                if (Environment.StackTrace.Contains("Spreadsheet"))
                {
                    if (this.valuecontent != value)
                    {
                        this.valuecontent = value;

                        //this.OnPropertyChanged("Value");
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets set the color component.
        /// </summary>
        public uint BGColor
        {
            get
            {
                return this.bgColor;
            }

            set
            {
                if (this.bgColor != value)
                {
                    this.bgColor = value;
                    this.OnPropertyChanged("BGColor");
                }
            }
        }

        /// <summary>
        /// event raised when a property is changed on a component.
        /// </summary>
        /// <param name="v">Property string name(Text or Value).</param>
        private void OnPropertyChanged(string v)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        /// <summary>
        /// cell property change's response.
        /// </summary>
        /// <param name="sender">spreedsheetcell.</param>
        /// <param name="e">property name.</param>
        public void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(e.PropertyName));
        }

        /// <summary>
        /// expression tree in current node.
        /// </summary>
        public ExpressionTree expressionTree;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
