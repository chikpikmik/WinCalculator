using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using NCalc.Domain;
using System.Data.OleDb;
using NCalc;
using System.Reflection;
using System.IO;
using System.Net.NetworkInformation;
using System.Diagnostics.Eventing.Reader;
using System.Windows;

namespace WinCalculator
{
    public class QueryElement
    {
        public double Value { get; set; }
        public string Name { get; set; }

        public QueryElement(double value, string name)
        {
            Value = value;
            Name = name;
        }

        // Что бы отображалось при выводе списка
        public override string ToString()
        {
            return Name;
        }
    }


    class CommonFunctions
    {

        static public List<QueryElement> GetListFromDB(string Table)
        {
            List<QueryElement> QueryElements = new List<QueryElement>();

            string queryString = $"SELECT Value, Name FROM {Table}";

            string fullPath = @"..\..\db.mdb";
            string connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source= {fullPath};";

            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {
                connection.Open();

                using (OleDbCommand command = new OleDbCommand(queryString, connection))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            double value = Convert.ToDouble(reader.GetString(0));
                            string name = reader.GetString(1);

                            QueryElements.Add(new QueryElement(value, name));
                        }
                    }
                }
            }

            return QueryElements;
        }

        static public double ComputeExpression(string expression)
        {

            if (expression == "")
                return 0;

            NCalc.Expression e = new NCalc.Expression(expression.Replace(',','.').Replace("∞","(1/0)")
                .Replace("e", "(2.71828)").Replace("π", "(3.14159)"));

            if (e.HasErrors())
                return double.NaN;

            return Convert.ToDouble(e.Evaluate());


        }


        static public void TextBox_TextChanged(TextBox ChangedTextBox, TextBox SecondTextBox=null, double A=1, double B=1)
        {
            int oldLength = ChangedTextBox.Text.Length;
            int cursor = ChangedTextBox.SelectionStart;
            ChangedTextBox.Text = string.Concat(ChangedTextBox.Text.Where(c => char.IsDigit(c) ||
            // c == '!' || c == 'i' || c == '^' ||
            c == '∞' || c == 'e' || c == 'π' ||
            c == '+' || c == '-' ||
            c == '(' || c == ' ' || c == ')' ||
            c == '/' || c == '*' || c == '%' ||
            c == '.' || c == ','));
            int newLength = ChangedTextBox.Text.Length;

            if (newLength == 0)
                ChangedTextBox.Text = "0";

            if (oldLength > newLength)
            {
                // Либо курсор за пределами обрезанной области, либо введенный символ не был отображен и курсор нужно сместить обратно
                ChangedTextBox.SelectionStart = newLength < cursor ? newLength: cursor - 1;
                ChangedTextBox.Focus();

            }
            else if (oldLength == newLength)
            {
                // Либо значение было заменено, значит курсор на нуле, либо все впорядке
                ChangedTextBox.SelectionStart = cursor==0 ? newLength : cursor;
                ChangedTextBox.Focus();
            }


            if (SecondTextBox != null)
            {
                double ComputedExpression = ComputeExpression(ChangedTextBox.Text) * B / A;

                if (double.IsInfinity(ComputedExpression))
                    SecondTextBox.Text = "∞";

                else if (!ComputedExpression.Equals(double.NaN))
                    SecondTextBox.Text = ComputedExpression.ToString();
            }


        }

        static public void ComboBox_SelectionChanged(
            ComboBox ChangedComboBox, ComboBox SecondComboBox,
            TextBox ChangedTextBox, TextBox SecondTextBox,
            double A=1, double B=1)
        {

            if (ChangedTextBox.IsFocused)
            {
                double result = ComputeExpression(ChangedTextBox.Text) * B / A;
                SecondTextBox.Text = result.Equals(Double.NaN) ? "" : result.ToString();
            }
            else
            {
                double result = ComputeExpression(SecondTextBox.Text) * A / B;
                ChangedTextBox.Text = result.Equals(Double.NaN) ? "" : result.ToString();
            }

        }


        static public void Button_click(TextBox Calculator_TextBox, Button b)
        {

            string content = b.Content.ToString();
            string name = b.Name;

            string selectedtext = Calculator_TextBox.SelectedText;
            int selectionStart = Calculator_TextBox.SelectionStart;
            int selectionLength = Calculator_TextBox.SelectionLength;

            int oldpos = Calculator_TextBox.SelectionStart;
            string oldstr = Calculator_TextBox.Text;

            if (content == "=")
            {
                double ComputedExpression = ComputeExpression(Calculator_TextBox.Text);
                if (double.IsInfinity(ComputedExpression))
                {
                    Calculator_TextBox.Text = "∞";
                }
                else if (!ComputedExpression.Equals(double.NaN))
                {
                    Calculator_TextBox.Text = ComputedExpression.ToString();
                }
            }
            else if (name == "Backspace")
            {
                if (oldstr.Length == 1)
                    Calculator_TextBox.Text = "0";
                else if (!string.IsNullOrEmpty(selectedtext))
                {
                    Calculator_TextBox.Text = Calculator_TextBox.Text.Remove(selectionStart, selectionLength);
                    Calculator_TextBox.SelectionStart = selectionStart;
                }
                else
                {
                    Calculator_TextBox.Text = oldstr.Remove(oldpos - 1, 1);
                    Calculator_TextBox.SelectionStart = oldpos - 1;
                }
            }
            else if (content == "CE" || content == "C")
                Calculator_TextBox.Text = "0";

            else if (content == "1/x")
                Calculator_TextBox.Text = $"1/({oldstr})";

            else if (content == "+/-")
            {
                Calculator_TextBox.Text = oldstr[0] == '-' ? oldstr.Remove(0, 1) : oldstr.Insert(0, "-");
                Calculator_TextBox.SelectionStart = oldpos + (Calculator_TextBox.Text.Length - oldstr.Length);
            }

            else if (!string.IsNullOrEmpty(selectedtext))
            {
                Calculator_TextBox.Text = Calculator_TextBox.Text.Remove(selectionStart, selectionLength).Insert(selectionStart, content);
                Calculator_TextBox.SelectionStart = selectionStart + content.Length;
            }

            else
            {
                Calculator_TextBox.Text = oldstr.Insert(oldpos, content); ;
                Calculator_TextBox.SelectionStart = oldpos + 1;
            }



        }

    }
}
