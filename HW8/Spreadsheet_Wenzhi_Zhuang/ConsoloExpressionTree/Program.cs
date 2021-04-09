using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CptS321;
namespace HW5
{
    /// <summary>
    /// The main class to run the expression tree algorithm in HW5.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The main function to run the program for HW5.
        /// </summary>
        static void Main()
        {
            // Initilize a exoresstree, which input a initial string as "Hello+World"
            ExpressionTree root = new ExpressionTree("Hello+World");

            // print out the current evaluation result inside ExpressionTree
            Console.WriteLine("result: " + root.Evaluate().ToString());

            // log into a while loop as a menu
            while (true)
            {
                Console.WriteLine("Menu()");
                Console.WriteLine("1. Enter a new exprssion");
                Console.WriteLine("2. Set a variable value");
                Console.WriteLine("3. Evaluate tree");
                Console.WriteLine("4. Quit");
                string option = Console.ReadLine();
                int choice;

                // stack overflow checking - filter the invalid choice
                try
                {
                    choice = int.Parse(option);
                }
                catch (FormatException)
                {
                    choice = 5;
                }

                // switch to certain option in current meun
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Option 1");
                        string expression = Console.ReadLine();
                        if (expression != string.Empty)
                        {
                            root = new ExpressionTree(expression);
                        }

                        break;

                    case 2:
                        Console.WriteLine("Option 2");
                        Console.Write("Variable: ");
                        string variable = Console.ReadLine();
                        Console.Write("Value: ");
                        string value = Console.ReadLine();
                        root.SetVariable(variable, value);
                        break;
                    case 3:
                        Console.WriteLine("Option 3");
                        Console.WriteLine("result: " + root.Evaluate().ToString());
                        break;
                    case 4:
                        System.Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("The option unknown.");
                        break;
                }
            }
        }
    }
}
