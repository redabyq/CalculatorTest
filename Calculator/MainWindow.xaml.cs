using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string lastsymbol="r";
        public string opers = "=*/+-^.";
        public MainWindow()
        {

            InitializeComponent();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            Button button = (Button)sender;
#pragma warning disable CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
            string symbol = button.Content.ToString();
#pragma warning restore CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
            TextBlock textBlock = (TextBlock)Screen;
            TextBlock func = (TextBlock)Funct;
            if (textBlock.Text == "Введите...")
            {
                textBlock.Text = "";
                textBlock.Foreground = new SolidColorBrush(Colors.White);
            }
                if (symbol == "=")
                {
                    if (!textBlock.Text.Equals("Введите...") && !textBlock.Text.Equals("∞"))
                    {
                        func.Text = textBlock.Text;
                        string processedExpression = SqareReplacer(textBlock.Text);
                        double result = SolveWithDatatables(processedExpression);

                        textBlock.Text = result.ToString();
                    }
                }
            else if (lastsymbol == "r" && opers.Contains(symbol)) { }
            else
            {
                if (symbol.Equals("C"))
                {
                    textBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA4A4A4"));
                    textBlock.Text = "Введите...";
                    lastsymbol = "r";
                    func.Text = "";
                }
                else
                {
                    if (opers.Contains(lastsymbol) && opers.Contains(symbol))
                    {
                    }
                    else
                    {
                        textBlock.Text += symbol;
                        lastsymbol = symbol;
                    }
                }
            }

           
        }
        static string SqareReplacer(string ex)
        {
            string output = "";
            int length = ex.Length;
            for (int i = 0; i < length; i++)
            {

                // посля √
                if (ex[i] == '√')
                {

                    string addn = "";
                    if (i != 0)
                    {
                        if (char.IsDigit(ex[i - 1]))
                        {
                            int prevend = i - 1; // начало
                            int prevstart = prevend;

                            // кончало
                            while (prevstart > 0 && char.IsDigit(ex[prevstart]))
                            {
                                prevstart = prevstart - 1;
                            }

                            // выкончало
                            addn = ex.Substring(prevstart, prevend - prevstart) + "*";//
                        }
                    }
                    int start = i + 1; // начало
                    int end = start;

                    // кончало
                    while (end < length && char.IsDigit(ex[end]))
                    {
                        end = end + 1;
                    }

                    // выкончало
                    string numberStr = ex.Substring(start, end - start);//start начало второе длинна
                    if (double.TryParse(numberStr, out double number))
                    {
                        double root = Math.Sqrt(number);
                        output += addn;
                        output += root.ToString();
                        i = end - 1;
                    }
                
                else
                {
                }
            }

                else
                {

                    output += ex[i];
                }
            }
            return output;
        }
        static double SolveWithDatatables(string expression)
        {
            var dataTable = new DataTable();
            var value = dataTable.Compute(expression, string.Empty);
            return Convert.ToDouble(value);
        }

        //textBlock.Foreground = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFDFD991"));



    }
}
