// <copyright file="TestClass.cs" company="Wenzhi Zhuang">
// Copyright (c) Wenzhi Zhuang. All rights reserved.
//  Programmer: Wenzhi Zhuang, ID: 11632272
// </copyright>
// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using CptS321;
using System.ComponentModel;
using System;

namespace NUnit.TestsSpreadshell
{
    /// <summary>
    /// Where to run the test inside.
    /// </summary>
    [TestFixture]
    public class TestClass
    {
        /// <summary>
        /// Construct a Spreadsheet for testing.
        /// </summary>
        private readonly Spreadsheet myspreadsheet = new Spreadsheet(50, 26);

        /// <summary>
        /// initilize a string the accept the testing's value inside Spreadsheet.
        /// </summary>
        private string test;

        /// <summary>
        /// Here is to run a test inside
        /// There are three case to test.
        /// 1. Assign a value inside the Spreadsheet.
        /// 2. Utilize the "=(Colume Character)#" for a copy in other cell.
        /// 3. Stackover testing.
        /// 4. Null copy testing.
        /// </summary>
        [Test]
        public void TestMethod()
        {
            this.myspreadsheet.PropertyChanged += this.CellPropertyChangedEventHandler;
            //Assert.AreEqual(1, 1);
            // 1. Assign a value inside the Spreadsheet.
            this.myspreadsheet.Cells[1, 0].Text = "This is a test";

            // 2. Utilize the "=(Colume Character)#" for a copy in other cell.
            this.myspreadsheet.Cells[2, 5].Text = "=A2";

            // test whether value and copy was achieve succeed.
            Assert.AreEqual("This is a test", this.test);

            // 3. Stackover testing.
            // test whether spreadsheey can be accessed out of range.
            try
            {
                this.myspreadsheet.Cells[55, 5].Text = "1";
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("An Exception has occurred : {0}", e.Message);
                Assert.AreEqual("Index was outside the bounds of the array.", e.Message);
            }

            // 4. Null copy testing.
            this.myspreadsheet.Cells[2, 5].Text = "=A3";

            // testing whether initial variable is correct in SpreadsheetCell
            Assert.AreEqual(" ", this.test);

            // 5. Expression tree calculating 

            this.myspreadsheet.Cells[0, 0].Text = "1"; 
            this.myspreadsheet.Cells[1, 0].Text = "2";
            this.myspreadsheet.Cells[2, 0].Text = "=A1+A2";
            Assert.AreEqual("3", this.test);

            this.myspreadsheet.Cells[0, 0].Text = "4";
            this.myspreadsheet.Cells[2, 0].Text = "=A1+A2";
            Assert.AreEqual("6", this.test);

            this.myspreadsheet.Cells[2, 0].Text = "=A1*A2 + 4";
            Assert.AreEqual("12", this.test);
        }

        /// <summary>
        /// Get the modified value once the property was changed inside spreadsheet.
        /// </summary>
        /// <param name="sender"> The varaible in Spreadsheet class type. </param>
        /// <param name="e"> The variable of string that mention what is the property changed.</param>
        private void CellPropertyChangedEventHandler(object sender, PropertyChangedEventArgs e)
        {
            this.test = this.myspreadsheet.Cells[((SpreadsheetCell)sender).RowIndex, ((SpreadsheetCell)sender).ColumeIndex].Value;
        }
        //private void CellPropertyChangedEventHandler(object sender, System.EventArgs e)
        //{
        //    Spreadsheet sendersheet;

        //    if (sender is Spreadsheet)
        //    {
        //        sendersheet = (Spreadsheet)sender;

        //        SpreadsheetCell senderCell = sendersheet.GetCell(sendersheet.RowIndex, sendersheet.ColIndex);
        //        int row = senderCell.RowIndex;
        //        int column = senderCell.ColumeIndex;

        //        string str = this.myspreadsheet.Cells[row, column].Text;
        //        this.test = str;
        //    }
        //}
    }
}
