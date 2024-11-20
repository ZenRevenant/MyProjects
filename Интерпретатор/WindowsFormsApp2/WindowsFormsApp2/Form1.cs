using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        List<Operation> codeU = new List<Operation>();
        List<string> output = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }
        public static Dictionary<string, Var> Variable = new Dictionary<string, Var>();
        public static Var CreaNum(string var)
        {
            Var result = new Var();
            if (var.Contains("."))
            {
                result.Type = "real";
            }
            else
            {
                result.Type = "int";
            }
            result.Value = var;
            return result;
        }
        private string[] deleter(string[] line)
        {
            for (int ind = 0; ind < line.Length; ind++)
            {
                int index = line[ind].IndexOf(':');
                if (index != -1)
                {
                    line[ind] = line[ind].Substring(index + 1);
                }
            }
            return line;
        }
        private void ReadOperation(string line, int i)
        {
            // Обновленное регулярное выражение
            string operationPattern = @"
        (?<leftVar>\w+)\s*=\s*                             # Переменная слева от '='
        (?<rightVar1>\w+|\d+(\.\d+)?)\s*                   # Первый операнд
        (?<operation>[+\-*/%]|[><=]+|\|\||\|)?\s*          # Операция (математическая, логическая или модуль) - необязательная
        (?<rightVar2>\w+|\d+(\.\d+)?)?\s*                  # Второй операнд (необязательный)
        (?:\?\s*(?<ifVar>\w+|\(\w+\))\s*:\s*(?<elseVar>\w+|\(\w+\)))? # Тернарный оператор
        |                                                  # Или
        (?<leftVar>\w+)\s*=\s*(?<rightVar1>\w+|\d+(\.\d+)?)\s*=\s*(?<operation>=)"; // Простое присваивание // Простое присваивание  // Условие if
            Regex regexop = new Regex(operationPattern, RegexOptions.IgnorePatternWhitespace);
            MatchCollection matches = regexop.Matches(line);

            foreach (Match match in matches)
            {
                string leftVar = match.Groups["leftVar"].Value;
                string rightVar1 = match.Groups["rightVar1"].Value;
                string operationSymbol = match.Groups["operation"].Value; // Исправлено на правильную группу
                string rightVar2 = match.Groups["rightVar2"].Value;
                string ifVar = match.Groups["ifVar"].Value;
                string elseVar = match.Groups["elseVar"].Value;

                codeU.Add(new Operation());
                codeU[i].operation = string.IsNullOrEmpty(operationSymbol) ? "=" : operationSymbol;

                // Обработка переменных
                if (!Variable.ContainsKey(leftVar))
                {
                    Variable[leftVar] = new Var();
                }
                if (!Variable.ContainsKey(rightVar1))
                {
                    Variable[rightVar1] = CreaNum(rightVar1);
                }
                if (!string.IsNullOrEmpty(rightVar2) && !Variable.ContainsKey(rightVar2)) // Проверка на пустоту
                {
                    Variable[rightVar2] = CreaNum(rightVar2);
                }

                if (!string.IsNullOrEmpty(ifVar) && !string.IsNullOrEmpty(elseVar))
                {
                    if (!Variable.ContainsKey(ifVar))
                    {
                        Variable[ifVar] = CreaNum(ifVar);
                    }
                    if (!Variable.ContainsKey(elseVar))
                    {
                        Variable[elseVar] = CreaNum(elseVar);
                    }
                    codeU[i].ifvar = ifVar;
                    codeU[i].elsevar = elseVar;
                    codeU[i].isLogic = true;
                }

                codeU[i].leftVarName = leftVar;
                codeU[i].rightVarName1 = rightVar1;
                codeU[i].rightVarName2 = rightVar2;
            }
        }
        private void ReadCode(string[] code)
        {
            string functionPattern = @"net \s*\(\s*in:(?<inParams>[^;]*);out\((?<outParams>\d+)\)\)\s*{(?<body>[^}]+)}";
            Regex regex = new Regex(functionPattern);
            int i = 0;
            foreach (string line in code)
            {
                ReadOperation(line, i);
                i++;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Variable.Clear();
            codeU.Clear();
            string pattern = @"\b(int|real)\s+([a-zA-Z_]\w*)\b";
            string[] lines = textBox1.Lines;
            foreach (string line in lines)
            {
                MatchCollection matches = Regex.Matches(line, pattern);
                foreach (Match match in matches)
                {
                    Variable[match.Groups[2].Value] = new Var { Type = match.Groups[1].Value };
                }
            }
            string[] code = richTextBox1.Lines;
            code = deleter(code);
            ReadCode(code);
            lines = textBox2.Lines;
            string pattern1 = @"(?<varName>[a-zA-Z_][a-zA-Z0-9_]*)";
            foreach (string line in lines)
            {
                MatchCollection matches = Regex.Matches(line, pattern1);
                foreach (Match match in matches)
                {
                    string variableName = match.Groups["varName"].Value;
                    if (!output.Contains(variableName))
                        output.Add(variableName);
                }
            }
            textBox3.Visible = true;
        }

        private void Compute()
        {
            textBox4.Text = "";
            string input = textBox3.Text;
            string pattern = @"(\w+)\s*=\s*(-?\d+(\.\d+)?)";
            Regex regex = new Regex(pattern);
            MatchCollection matches = regex.Matches(input);

            // Обновление переменных на основе входных данных
            foreach (Match match in matches)
            {
                string variableName = match.Groups[1].Value;
                string variableValue = match.Groups[2].Value;
                Variable[variableName].Value = variableValue;
            }

            // Создаем список для хранения результатов
            List<int> complete = new List<int>();

            // Параллельная обработка операций
            bool changed = true; // Инициализируем как true, чтобы войти в цикл

            while (changed) // Цикл продолжается, пока есть изменения
            {
                changed = false; // Сброс флага изменения для каждой итерации

                // Параллельная обработка операций
                Parallel.For(0, codeU.Count, i =>
                {
                    // Синхронизация доступа к общей коллекции Variable
                    lock (Variable)
                    {
                        if ((codeU[i].operation == "|" || codeU[i].operation == "=") && !complete.Contains(i))
                        {
                            if (Variable[codeU[i].rightVarName1].Value != null)
                            {
                                Variable[codeU[i].leftVarName] = codeU[i].DoOperation();
                                changed = true; // Устанавливаем, если произошло изменение
                                complete.Add(i);
                            }
                        }
                        else if (!complete.Contains(i))
                        {
                            if (Variable[codeU[i].rightVarName1].Value != null &&
                            (Variable[codeU[i].rightVarName2].Value != null) &&
                            !complete.Contains(i))
                            {
                                if (!codeU[i].isLogic)
                                {
                                    Variable[codeU[i].leftVarName] = codeU[i].DoOperation();
                                    changed = true; // Устанавливаем, если произошло изменение
                                    complete.Add(i);
                                }
                                else if (Variable[codeU[i].ifvar].Value != null && Variable[codeU[i].elsevar].Value != null)
                                {
                                    Variable[codeU[i].leftVarName] = codeU[i].DoOperation();
                                    changed = true; // Устанавливаем, если произошло изменение
                                    complete.Add(i);
                                }
                            }
                        }
                    }
                });
            }

            // Обновление пользовательского интерфейса должно выполняться в основном потоке
            this.Invoke((MethodInvoker)delegate
            {
                foreach (string ouT in output)
                {
                    lock (Variable) // Синхронизация доступа к общей коллекции Variable
                    {
                        if (Variable[ouT].Value != null)
                        {
                            textBox4.Text += ouT + "=" + Variable[ouT].Value + " ";
                        }
                    }
                }
            });
        }
        private void AddLineNumbers()
        {
            // Calculate the number of lines to display
            // Разделяем текст на строки
            string[] lines = richTextBox1.Lines;
            lines = deleter(lines);
            // Перебираем строки и добавляем нумерацию
            string[] numberedLines = new string[lines.Length];
            int savedSelectionStart = richTextBox1.SelectionStart;
            int savedSelectionLength = richTextBox1.SelectionLength;
            // Получаем текущий номер строки, на которой находится курсор
            int currentLineNumber = richTextBox1.GetLineFromCharIndex(savedSelectionStart);
            // Перебираем строки и добавляем нумерацию
            for (int i = 0; i < lines.Length; i++)
            {
                numberedLines[i] = $"{i}:{lines[i]}"; // Добавляем +1, чтобы нумерация начиналась с 1, а не с 0
            }
            // Устанавливаем новый массив строк с номерами в RichTextBox
            richTextBox1.Lines = numberedLines;
            richTextBox1.Select(savedSelectionStart + richTextBox1.Lines[currentLineNumber].Length, savedSelectionLength);
            richTextBox1.ScrollToCaret();
        }
        private void textBox3_Leave(object sender, EventArgs e)
        {
            Compute();
        }
        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            AddLineNumbers();
            textBox3.Visible = false;
        }
        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete)
            {
                int savedSelectionStart = richTextBox1.SelectionStart;
                int savedSelectionLength = richTextBox1.SelectionLength;
                string[] lines = richTextBox1.Lines;
                int currentLineNumber = richTextBox1.GetLineFromCharIndex(savedSelectionStart);
                if (Regex.IsMatch(lines[currentLineNumber].Trim(), @"^\d+:$") && lines.Length != 1)
                {
                    lines = lines.Where((line, index) => index != currentLineNumber).ToArray();
                    richTextBox1.Lines = lines;
                    e.Handled = true;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox3.Visible = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox3.Visible = false;
        }
    }
}
