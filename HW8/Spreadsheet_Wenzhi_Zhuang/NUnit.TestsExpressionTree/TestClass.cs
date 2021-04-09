// <copyright file="TestClass.cs" company="Wenzhi Zhuang">
// Copyright (c) Wenzhi Zhuang. All rights reserved.
//  Programmer: Wenzhi Zhuang, ID: 11632272
// </copyright>

using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using CptS321;
using System.ComponentModel;
using System;

namespace NUnit.TestsExpressionTree
{
    /// <summary>
    /// There will be 5 test inside.
    /// 1. Default testing.
    /// 2. Set varaible testing.
    /// 3. Invaild set testing.
    /// 4. Multiply operator testing.
    /// 5. evaluation testing.
    /// 6. parentheses expression calculation, mentioned in operator "+", "-", "*", "/", and "^" in natural numbers
    /// 7. single vairable input
    /// 8. ignore string space in expression
    /// </summary>
    [TestFixture]
    public class TestClass
    {
        /// <summary>
        /// run the unit testing inside.
        /// </summary>
        [Test]
        public void TestMethod()
        {
            // 1. Default testing - all string should be set in zero value
            ExpressionTree root = new ExpressionTree("Hello+World");
            Assert.AreEqual(0, root.Evaluate());

            // 2. Set varaible testing - set a value for certain variable
            root.SetVariable("Hello", "20");

            // testing pass - current result are equal to 20
            Assert.AreEqual(20, root.Evaluate());

            // 3. Invaild set testing - set a invaild string value for certain variable
            root.SetVariable("Hello", " ");

            // expression tree will be remained as "Hello+World" where Hello = 20
            Assert.AreEqual(20, root.Evaluate());

            // 4.Multiply operator testing - input a multiply operator string to check
            Assert.AreEqual(false, root.CheckOperator("Hello+World-1"));

            // 5. evaluation testing - set a current node's value for calucaluting result
            root = new ExpressionTree("1+2+3");
            Assert.AreEqual(6, root.Evaluate());

            // 6. parentheses expression calculation, mentioned in operator "+", "-", "*", "/", and "^" in natural numbers
            root = new ExpressionTree("((1+2)*3/2)^2");
            Assert.AreEqual(20.25, root.Evaluate());

            // 7. single vairable input
            root = new ExpressionTree("100");
            Assert.AreEqual(100, root.Evaluate());

            /// 8. ignore string space in expression
            root = new ExpressionTree("( ( 1 + 2 ) * 3 / 2 ) ^ 2 ");
            Assert.AreEqual(20.25, root.Evaluate());
        }
    }
}
