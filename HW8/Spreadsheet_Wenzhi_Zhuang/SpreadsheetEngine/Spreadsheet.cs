// <copyright file="Spreadsheet.cs" company="Wenzhi Zhuang">
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
    /// Constuct a Spreadsheet, which contains total columes and rows.
    /// </summary>
    public class Spreadsheet : INotifyPropertyChanged
    {
        /// <summary>
        /// Global define a cell in Spreadsheet as a 2D array, specific in position(colume/row) infomation.
        /// </summary>
        public SpreadsheetCell[,] Cells;

        /// <summary>
        /// The total colume in current spreadsheet.
        /// </summary>
        public int ColumnCount;

        /// <summary>
        /// The total row in a colume in current spreedsheet.
        /// </summary>
        public int RowCount;

        /// <summary>
        /// Gets the position of modification in Colume, for onPropertyChange use and CellPropertyChangedEventHandler in main file.
        /// </summary>
        public int ColIndex { get; private set; }

        /// <summary>
        /// Gets the position of modification in rows for certain colume, for onPropertyChange use and CellPropertyChangedEventHandler in main file.
        /// </summary>
        public int RowIndex { get; private set; }

        /// <summary>
        /// Set a proertychanged event to detected whether there is a inside property changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Handle undo and redo opinion.
        /// </summary>
        private PropertyChangedEventHandler propertyChangedEventHandler;

        /// <summary>
        /// Updating the current property.
        /// </summary>
        /// <param name="changedCell">property that was changed.</param>
        /// <param name="v">String of property infomation in SpreadsheetCell.</param>
        private void RaiseCellPropertyChanged(SpreadsheetCell changedCell, string v)
        {
            this.PropertyChanged?.Invoke(changedCell, new PropertyChangedEventArgs(v));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// Construct a spreadsheet in as a matrix.
        /// </summary>
        /// <param name="numRow">Number of Rows supposed to be in the matrix.</param>
        /// <param name="numCol">Number of Columes supposed to be in the matrix.</param>
        public Spreadsheet(int numRow, int numCol)
        {
            this.RowCount = numRow;
            this.ColumnCount = numCol;
            this.Cells = new SpreadsheetCell[numRow, numCol];
            this.propertyChangedEventHandler = this.CellPropertyChanged;
            this.unDoStack = new Stack<UndoRedo>();
            this.reDoStack = new Stack<UndoRedo>();

            for (int row = 0; row < this.RowCount; row++)
            {
                for (int col = 0; col < this.ColumnCount; col++)
                {
                    this.Cells[row, col] = new SpreadsheetCell(row, col);
                    this.Cells[row, col].PropertyChanged += this.CellPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Get the current position that indicated in paramter.
        /// </summary>
        /// <param name="row">The cell's row.</param>
        /// <param name="col">The cell's colume.</param>
        /// <returns>Cell in SpreadsheetCell type.</returns>
        public SpreadsheetCell GetCell(int row, int col)
        {
            if (col >= 0 && col < this.ColumnCount)
            {
                if (row >= 0 && row < this.RowCount)
                {
                    return this.Cells[row, col];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the modification cell position, which will be used in main file.
        /// </summary>
        /// <param name="sender">object SpreadsheetCell.</param>
        /// <param name="e">Property name(Text or Value).</param>
        private void CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SpreadsheetCell changeCell;

            if (sender is SpreadsheetCell)
            {
                changeCell = this.GetCell(((SpreadsheetCell)sender).RowIndex, ((SpreadsheetCell)sender).ColumeIndex);
                if (e.PropertyName.ToString().CompareTo("BGColor") == 0)
                {
                    this.RaiseCellPropertyChanged(changeCell, "BGColor");
                }
                else
                {
                    if (changeCell != null)
                    {
                        string text = changeCell.Text;
                        StringBuilder sb = new StringBuilder(text);

                        // text = text.Replace(" ", string.Empty);
                        for (int i = 0; i < text.Length; i++)
                        {
                            if (text[i] == '=')
                            {
                                sb = sb.Replace(" ", string.Empty, 0, i);
                                text = sb.ToString();
                                break;
                            }
                        }

                        // changeCell.Text = changeCell.Text.Replace(" ", string.Empty);
                        if (text[0] == '=')
                        {
                            List<string> token = this.GetVaraiable(text.Substring(1));
                            bool isValid = true;
                            for (int i = 0; i < token.Count; i++)
                            {
                                try
                                {
                                    if (!double.TryParse(token[i], out var value))
                                    {
                                        double test = 0;
                                        int row = 0;
                                        int colume = 0;
                                        var myNumbers = token[i].Where(x => char.IsDigit(x)).ToArray();
                                        var myNewString = new string(myNumbers);
                                        row = int.Parse((string)myNewString) - 1;
                                        char columeChar = token[i][0];
                                        colume = Convert.ToInt32(columeChar) - 65;
                                        var cell = this.GetCell(row, colume);
                                        if (isValid == true)
                                        {
                                            isValid = double.TryParse(cell.Value, out test);
                                        }

                                        if (changeCell == cell)
                                        {
                                            changeCell.Value = "0";
                                            changeCell.Text = "0";
                                            return;
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    isValid = false;
                                    break;
                                }
                            }

                            if (token.Count == 1)
                            {
                                try
                                {
                                    if (!double.TryParse(token[0], out var value))
                                    {
                                        int row = 0;
                                        int colume = 0;
                                        var myNumbers = token[0].Where(x => char.IsDigit(x)).ToArray();
                                        var myNewString = new string(myNumbers);
                                        row = int.Parse((string)myNewString) - 1;
                                        char columeChar = changeCell.Text[1];
                                        colume = Convert.ToInt32(columeChar) - 65;
                                        var cell = this.GetCell(row, colume);
                                        cell.PropertyChanged -= changeCell.CellPropertyChanged;
                                        changeCell.Value = this.Cells[row, colume].Value;
                                        cell.PropertyChanged += changeCell.CellPropertyChanged;
                                    }
                                    else
                                    {
                                        changeCell.Value = token[0];
                                    }
                                }
                                catch (Exception)
                                {
                                    changeCell.Text = text.Substring(1);
                                    changeCell.Value = text.Substring(1);
                                }
                            }
                            else if (isValid)
                            {
                                if (this.CheckOperator(text))
                                {
                                    changeCell.expressionTree = new ExpressionTree(text.Substring(1));
                                    List<SpreadsheetCell> cellList = new List<SpreadsheetCell>();
                                    for (int i = 0; i < token.Count; i++)
                                    {
                                        if (!double.TryParse(token[i], out var value))
                                        {
                                            int row = 0;
                                            int colume = 0;
                                            var myNumbers = token[i].Where(x => char.IsDigit(x)).ToArray();
                                            var myNewString = new string(myNumbers);
                                            row = int.Parse((string)myNewString) - 1;
                                            char columeChar = token[i][0];
                                            colume = Convert.ToInt32(columeChar) - 65;
                                            var cell = this.GetCell(row, colume);
                                            changeCell.expressionTree.SetVariable(token[i], cell.Value);
                                            cellList.Add(cell);
                                        }
                                    }

                                    for (int i = 0; i < cellList.Count; i++)
                                    {
                                        cellList[i].PropertyChanged -= changeCell.CellPropertyChanged;
                                    }

                                    changeCell.Value = changeCell.expressionTree.Evaluate().ToString();
                                    for (int i = 0; i < cellList.Count; i++)
                                    {
                                        cellList[i].PropertyChanged += changeCell.CellPropertyChanged;
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    changeCell.expressionTree = new ExpressionTree(text.Substring(1));
                                    List<SpreadsheetCell> cellList = new List<SpreadsheetCell>();
                                    for (int i = 0; i < token.Count; i++)
                                    {
                                        if (!double.TryParse(token[i], out var value))
                                        {
                                            int row = 0;
                                            int colume = 0;
                                            var myNumbers = token[i].Where(x => char.IsDigit(x)).ToArray();
                                            var myNewString = new string(myNumbers);
                                            row = int.Parse((string)myNewString) - 1;
                                            char columeChar = token[i][0];
                                            colume = Convert.ToInt32(columeChar) - 65;
                                            var cell = this.GetCell(row, colume);
                                            if (double.TryParse(cell.Value, out var value1))
                                            {
                                                changeCell.expressionTree.SetVariable(token[i], cell.Value);
                                            }
                                            else
                                            {
                                                changeCell.expressionTree.SetVariable(token[i], "0");
                                            }

                                            cellList.Add(cell);
                                        }
                                    }

                                    for (int i = 0; i < cellList.Count; i++)
                                    {
                                        cellList[i].PropertyChanged -= changeCell.CellPropertyChanged;
                                    }

                                    changeCell.Value = changeCell.expressionTree.Evaluate().ToString();
                                    for (int i = 0; i < cellList.Count; i++)
                                    {
                                        cellList[i].PropertyChanged += changeCell.CellPropertyChanged;
                                    }
                                }
                                catch
                                {
                                    changeCell.Text = text.Substring(1);
                                    changeCell.Value = text.Substring(1);
                                }
                            }
                        }
                        else
                        {
                            changeCell.Text = text;
                            changeCell.Value = text;
                        }

                        this.ColIndex = changeCell.ColumeIndex;
                        this.RowIndex = changeCell.RowIndex;

                        // this.OnPropertyChanged(e.PropertyName);
                        this.RaiseCellPropertyChanged(changeCell, "Text");
                    }
                }
            }
        }

        public bool CheckOperator(string expression)
        {
            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] == '+' || expression[i] == '-' || expression[i] == '*' || expression[i] == '/' || expression[i] == '^' || expression[i] == '(')
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsOperator(char c)
        {
            if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^' || c == ')' || c == '(')
            {
                return true;
            }

            return false;
        }

        public List<string> GetVaraiable(string expression)
        {
            List<string> item = new List<string>();
            expression = expression.Replace(" ", string.Empty);
            Console.WriteLine(expression);
            string expressionNode = null;

            // turn string into list as a single token as words and operators
            for (int i = 0; i < expression.Length; i++)
            {
                if (this.IsOperator(expression[i]))
                {
                    if (expressionNode != null)
                    {
                        item.Add(expressionNode);
                    }

                    expressionNode = null;
                }
                else if (expression[i] == '(' || expression[i] == ')')
                {
                    if (expressionNode != null)
                    {
                        if (expressionNode != null)
                        {
                            item.Add(expressionNode);
                        }

                        expressionNode = null;
                    }

                    if (expression[i].ToString() != null)
                    {
                        item.Add(expression[i].ToString());
                    }
                }
                else
                {
                    expressionNode += expression[i];
                }
            }

            if (expressionNode != null)
            {
                item.Add(expressionNode);
            }

            return item;
        }

        /// <summary>
        /// Push the prev changed property into stack.
        /// </summary>
        /// <param name="cell">target property.</param>
        /// <param name="text_info">info of target property.</param>
        public void PushUndoEvent(SpreadsheetCell cell, string text_info)
        {
            this.unDoStack.Push(new UndoRedo(cell, text_info));
        }

        public void PushRedoEvent(UndoRedo undoRedoCell)
        {
            this.reDoStack.Push(undoRedoCell);
        }

        public UndoRedo Redo()
        {
            if (this.reDoStack.Count > 0)
            {
                var redoCell = this.reDoStack.Pop();
                var pos = redoCell.GetCoordinates();
                SpreadsheetCell copy = this.Cells[pos.Item1, pos.Item2].CreateCopy();
                var afterObj = new UndoRedo(copy, redoCell.changedPropertyName);
                redoCell.Evaluate(ref this.Cells[pos.Item1, pos.Item2]);
                return afterObj;
            }
            return null;
        }

        public UndoRedo Undo()
        {
            if (this.unDoStack.Count > 0)
            {
                var undoCell = this.unDoStack.Pop();
                var pos = undoCell.GetCoordinates();
                SpreadsheetCell copy = this.Cells[pos.Item1, pos.Item2].CreateCopy();
                var prevObj = new UndoRedo(copy, undoCell.changedPropertyName);
                undoCell.Evaluate(ref this.Cells[pos.Item1, pos.Item2]);
                return prevObj;
            }

            return null;
        }

        /// <summary>
        /// peek the stack's top property info.
        /// </summary>
        /// <returns>Redo "property" string. </returns>
        public string PeekRedo()
        {
            if (this.reDoStack.Count > 0)
            {
                return this.reDoStack.Peek().changedPropertyName;
            }

            return string.Empty;
        }

        /// <summary>
        /// peek the stack's top property info.
        /// </summary>
        /// <returns>Undo "property" string. </returns>
        public string PeekUedo()
        {
            if (this.unDoStack.Count != 0)
            {
                return this.unDoStack.Peek().changedPropertyName;
            }

            return string.Empty;
        }

        public Stack<UndoRedo> unDoStack;

        public Stack<UndoRedo> reDoStack;
    }
    
}
