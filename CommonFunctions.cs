using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using NCalc.Domain;
using System.Data.SQLite;
using NCalc;
using System.Reflection;
using System.IO;
using System.Net.NetworkInformation;
using System.Diagnostics.Eventing.Reader;

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

            string fullPath = @"C:\Users\Lolban\Projects\WinCalculator\db.db";
            using (SQLiteConnection connection = new SQLiteConnection($"Data Source={fullPath}; Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(queryString, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            double value = reader.GetDouble(0);
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

            Expression e = new Expression(expression.Replace(',','.').Replace("∞","(1/0)")
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

    }
}
