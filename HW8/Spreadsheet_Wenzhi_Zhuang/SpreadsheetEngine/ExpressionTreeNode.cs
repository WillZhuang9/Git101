// <copyright file="ExpressionTreeNode.cs" company="Wenzhi Zhuang">
// Copyright (c) Wenzhi Zhuang. All rights reserved.
//  Programmer: Wenzhi Zhuang, ID: 11632272
// </copyright>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// Expression Tree node construction - based on Binary Tree.
    /// </summary>
    public class ExpressionTreeNode
    {
        /// <summary>
        /// The Node's variable string.
        /// </summary>
        public string Text;

        /// <summary>
        /// The Node's variables's value in double type.
        /// </summary>
        public double Value = 0;

        /// <summary>
        /// Node's left child.
        /// </summary>
        public ExpressionTreeNode Left;

        /// <summary>
        /// Node's right child.
        /// </summary>
        public ExpressionTreeNode Right;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTreeNode"/> class.
        /// Node's constructor to accept a name string to set current node's text.
        /// </summary>
        /// <param name="item">The variable's name that be set in current node.</param>
        public ExpressionTreeNode(string item)
        {
            this.Text = item;
            this.Left = null;
            this.Right = null;
            if (double.TryParse(item, out var parsedNumber))
            {
                this.Value = parsedNumber;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTreeNode"/> class.
        /// Node's constructor to initilize a null Node.
        /// </summary>
        public ExpressionTreeNode()
        {
            this.Left = null;
            this.Right = null;
        }
    }
}
