// <copyright file="ExpressionTree.cs" company="Wenzhi Zhuang">
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
    /// Expression Tree construction inside this class.
    /// </summary>
    public class ExpressionTree
    {
        /// <summary>
        /// Check calculation priority.
        /// </summary>
        /// <param name="operator">The checking operator object.</param>
        /// <returns>operator level.</returns>
        public int CheckOperatorLevel(string @operator)
        {
            if (@operator == "^")
            {
                return 3;
            }

            if (@operator == "/")
            {
                return 2;
            }

            if (@operator == "*")
            {
                return 2;
            }

            if (@operator == "+")
            {
                return 1;
            }

            if (@operator == "-")
            {
                return 1;
            }

            if (@operator == ")")
            {
                return 0;
            }

            return -1;
        }

        // Construct a expression tree node as root
        private ExpressionTreeNode eT = new ExpressionTreeNode();

        /// <summary>
        /// Print the expression tree in order sequence.
        /// </summary>
        /// <param name="root">The expression tree node as root.</param>
        public void PostInOrder(ExpressionTreeNode root)
        {
            if (root == null)
            {
                return;
            }

            this.PostInOrder(root.Left);
            Console.Write(root.Text + " ");
            this.PostInOrder(root.Right);
        }

        /// <summary>
        /// Check whether the current character is a opeartion.
        /// </summary>
        /// <param name="c">single character inside string.</param>
        /// <returns>return the checking result.</returns>
        public bool IsOperator(char c)
        {
            if (c == '+' || c == '-' || c == '*' || c == '/' || c == '^' || c == ')' || c == '(')
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check whether the current string only contains single/unique operator.
        /// </summary>
        /// <param name="expression">the expression string from main file.</param>
        /// <returns>return the checking result.</returns>
        public bool CheckOperator(string expression)
        {
            Hashtable opeartorList = new Hashtable();
            for (int i = 0; i < expression.Length; i++)
            {
                if (this.IsOperator(expression[i]))
                {
                    opeartorList[expression[i]] = 1;
                }
            }

            if (opeartorList.Count > 1)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Construct a epression tree.
        /// </summary>
        /// <param name="expression">expression info string from main file.</param>
        /// <returns>the expression tree that has been constructed. </returns>
        public ExpressionTreeNode ConstructTree(string expression)
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

                    if (expression[i].ToString() != null)
                    {
                        item.Add(expression[i].ToString());
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

            // initilize the current list variable for double stacks to complie
            item.Insert(0, "(");
            item.Add(")");

            Stack<ExpressionTreeNode> stackNode = new Stack<ExpressionTreeNode>();
            Stack<string> stackString = new Stack<string>();
            ExpressionTreeNode t, t1, t2;
            for (int i = 0; i < item.Count; i++)
            {
                // push "(" in stackString
                if (item[i] == "(")
                {
                    stackString.Push(item[i]);
                }

                // push the variable in Stacknode
                else if (!this.IsOperator(item[i][0]))
                {
                    ExpressionTreeNode newNode = new ExpressionTreeNode(item[i]);
                    stackNode.Push(newNode);
                }
                else if (this.IsOperator(item[i][0]) && this.CheckOperatorLevel(item[i]) > 0)
                {

                    // if operator is lower or same level as current operator in current stacks
                    while (stackString.Count != 0 && stackString.Peek() != "(" &&
                        ((item[i] != "^" && this.CheckOperatorLevel(stackString.Peek()) >= this.CheckOperatorLevel(item[i])) ||
                        (item[i] == "^" && this.CheckOperatorLevel(stackString.Peek()) > this.CheckOperatorLevel(item[i]))))
                    {
                        // get and remove the top variable in stackString
                        t = new ExpressionTreeNode(stackString.Peek());
                        stackString.Pop();

                        // get and remove the top variable in stackNode
                        t1 = stackNode.Peek();
                        stackNode.Pop();

                        // get and remove the top variable in stackNode
                        t2 = stackNode.Peek();
                        stackNode.Pop();

                        // update the expression tree
                        t.Left = t2;
                        t.Right = t1;

                        // push to the stackNode
                        stackNode.Push(t);
                    }

                    stackString.Push(item[i]);
                }
                else if (item[i] == ")")
                {
                    while (stackString.Count != 0 && stackString.Peek() != "(")
                    {
                        // get and remove the top variable in stackString
                        t = new ExpressionTreeNode(stackString.Peek());
                        stackString.Pop();

                        // get and remove the top variable in stackNode
                        t1 = stackNode.Peek();
                        stackNode.Pop();

                        // get and remove the top variable in stackNode
                        t2 = stackNode.Peek();
                        stackNode.Pop();

                        // update the expression tree
                        t.Left = t2;
                        t.Right = t1;

                        // push to the stackNode
                        stackNode.Push(t);
                    }

                    stackString.Pop();
                }
            }

            // if there is no operator in current expression string
            if (item.Count == 3)
            {
                return new ExpressionTreeNode(item[1]);
            }

            t = stackNode.Pop();
            return t;
        }

        /// <summary>
        /// Search node to set the node's variable's value.
        /// </summary>
        /// <param name="root">Node type as a root pointer.</param>
        /// <param name="variableName">The varaible name that need to be set the value in the expression tree.</param>
        /// <param name="varaibleValue">The value that is going to set in target node.</param>
        public void SearchNode(ExpressionTreeNode root, string variableName, double varaibleValue)
        {
            if (root == null)
            {
                return;
            }
            else
            {
                if (variableName == root.Text)
                {
                    root.Value = varaibleValue;
                    return;
                }
                else
                {
                    this.SearchNode(root.Left, variableName, varaibleValue);
                    this.SearchNode(root.Right, variableName, varaibleValue);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// ExpressionTree constructor - to contruct a expression tree by giving expression string.
        /// </summary>
        /// <param name="expression">expression input string.</param>
        public ExpressionTree(string expression)
        {
            this.eT = this.ConstructTree(expression);
            Console.Write("In order sequence: ");
            this.PostInOrder(this.eT);
            Console.WriteLine();
            Console.Write("Infix sequence: " + expression);
            Console.WriteLine();
        }

        /// <summary>
        /// set the certain variable's value.
        /// </summary>
        /// <param name="variableName">The target variable in expression tree.</param>
        /// <param name="varaibleValue">The value is going to be set in the target node.</param>
        public void SetVariable(string variableName, string varaibleValue)
        {
            if (double.TryParse(varaibleValue, out var value))
            {
                this.SearchNode(this.eT, variableName, value);
            }
            else
            {
                Console.WriteLine("Please enter valid value");
            }
        }

        /// <summary>
        /// use recursion to calucalate the expression tree.
        /// </summary>
        /// <param name="root">root of expression tree.</param>
        /// <returns>calculate result from expression tree.</returns>
        public double CalculationExpression(ExpressionTreeNode root)
        {
            if (root == null)
            {
                return 0;
            }

            if (root.Left == null && root.Right == null)
            {
                return root.Value;
            }

            double leftValue = this.CalculationExpression(root.Left);
            double rightValue = this.CalculationExpression(root.Right);
            if (root.Text == "+")
            {
                return leftValue + rightValue;
            }

            if (root.Text == "-")
            {
                return leftValue - rightValue;
            }

            if (root.Text == "*")
            {
                return leftValue * rightValue;
            }

            if (root.Text == "/")
            {
                return leftValue / rightValue;
            }

            if (root.Text == "^")
            {
                return Math.Pow(leftValue, rightValue);
            }

            return root.Value;
        }

        /// <summary>
        /// Get the calucalate result back from current expression tree.
        /// </summary>
        /// <returns>calculating result from expression tree.</returns>
        public double Evaluate()
        {
            double sum = 0;
            sum = this.CalculationExpression(this.eT);
            return sum;
        }
    }
}
