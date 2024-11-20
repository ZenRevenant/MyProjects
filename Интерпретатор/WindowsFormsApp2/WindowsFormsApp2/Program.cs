using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
    public class Var
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public dynamic ConvertTo()
        {
            if (Type == "int")
                return Convert.ToInt32(Value);
            else
                return Convert.ToDouble(Value);

        }
    }
    public class Operation
    {
        public string leftVarName;
        public string rightVarName1;
        public string rightVarName2;
        public string operation;
        public bool isLogic = false;
        public string ifvar { get; set; } = null;
        public string elsevar { get; set; } = null;
        public Var DoOperation()
        {
            Var result = new Var();
            Var leftVar = Form1.Variable[leftVarName];
            Var rightVar1 = Form1.Variable[rightVarName1];
            Var rightVar2;
            if (operation != "|" && operation != "=")
            {
                rightVar2 = Form1.Variable[rightVarName2];
            }
            else
            {
                rightVar2 = Form1.Variable[rightVarName1];
            }
            dynamic left;
            dynamic right1;
            dynamic right2;
            var operations = new Dictionary<string, Func<dynamic, dynamic, dynamic>>
        {
            { "=", (x, y) => x },
            { "+", (x, y) => x + y },
            { "-", (x, y) => x - y },
            { "*", (x, y) => x * y },
            { "/", (x, y) => x / y },
            { "==", (x, y) => x == y },
            { "!=", (x, y) => x != y },
            { "<", (x, y) => x < y },
            { ">", (x, y) => x > y },
            { "<=", (x, y) => x <= y },
            { ">=", (x, y) => x >= y },
            {"|",(x,y)=>Math.Abs(x) },
            {"^",(x,y)=>Math.Pow(x, y)},
            {"%",(x,y)=>x%y}
        };
            if (rightVar1.Type == rightVar2.Type && rightVar2.Type!="real")
            {
                right1 = rightVar1.ConvertTo();
                right2 = rightVar2.ConvertTo();
                result.Type = rightVar1.Type;
            }
            else
            {

                right1 = Convert.ToDouble(Form1.Variable[rightVarName1].Value.Replace(".",","));
                right2 = Convert.ToDouble(Form1.Variable[rightVarName2].Value.Replace(".",","));
                result.Type = "real";
            }
            if (isLogic == false)
            {
                result.Value = Convert.ToString(operations[operation](right1, right2)).Replace(",", ".");
            }
            else
            {
                if (operations[operation](right1, right2))
                {
                    result = Form1.Variable[ifvar];
                }
                else
                {
                    result = Form1.Variable[elsevar];
                }
            }
            return result;
        }
    }
}
