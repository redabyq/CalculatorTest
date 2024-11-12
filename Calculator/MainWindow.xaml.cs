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
using System;


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
            Console.WriteLine("Программа запущена");

        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            Button? bequals = null;
            if (e.Key >= Key.D0 && e.Key <= Key.D9 && e.Key != Key.E|| e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 && e.Key != Key.E)
            {
                char digit = (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                    ? (char)(e.Key - Key.NumPad0 + '0')
                    : (char)(e.Key - Key.D0 + '0');
                //сложно но можно
                bequals = new Button { Content = digit.ToString() };
            }
            else if (e.Key == Key.E)
            {
                bequals = new Button
                {
                    Content = "="
                };
            }
            else if (e.Key == Key.C)
            {
                bequals = new Button
                {
                    Content = "C"
                };
            }
            else if (e.Key == Key.Divide)
            {
                bequals = new Button
                {
                    Content = "/"
                };
            }
            else if (e.Key == Key.Multiply)
            {
                bequals = new Button
                {
                    Content = "*"
                };
            }
            else if (e.Key == Key.Add)
            {
                bequals = new Button
                {
                    Content = "+"
                };
            }
            else if (e.Key == Key.Subtract)
            {
                bequals = new Button
                {
                    Content = "-"
                };
            }
            else { return; }
            Button_Click(bequals, new RoutedEventArgs());
            Console.WriteLine("Нажато"+ bequals.Content.ToString());
        }


        private void Button_Click(object sender, RoutedEventArgs e){
            Button button = (Button)sender;
            string? symbol = button.Content.ToString();
            TextBlock textBlock = (TextBlock)Screen;
            TextBlock func = (TextBlock)Funct;
            if (textBlock.Text == "Введите..."){
                textBlock.Text = "";
                textBlock.Foreground = new SolidColorBrush(Colors.White);
            }
            if (symbol == "="){
                if (!textBlock.Text.Equals("Введите...") || !textBlock.Text.Equals("∞")){
                    func.Text = textBlock.Text;
                    string processedExpression = SqareReplacer(textBlock.Text);
                    string result = SolveWithDatatables(processedExpression);

                    textBlock.Text = result;
                }
                //else if (textBlock.Text == "Введите...") { }
            }
            //else if (lastsymbol == "r" && opers.Contains(symbol)) { }
            else{
                if (symbol.Equals("C")){
                    textBlock.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA4A4A4"));
                    textBlock.Text = "Введите...";
                    lastsymbol = "r";
                    func.Text = "";
                }
                else{
                    if (!opers.Contains(lastsymbol) || !opers.Contains(symbol)){
                        textBlock.Text += symbol;
                        lastsymbol = symbol;
                    }
                }
            }
        }
        static string SqareReplacer(string ex){
            string output = "";
            int length = ex.Length;
            for (int i = 0; i < length; i++){
                // посля √
                if (ex[i] == '√'){
                    string addn = "";
                    if (i != 0){
                        if (char.IsDigit(ex[i - 1])){
                            int prevend = i - 1; 
                            // начало
                            int prevstart = prevend;
                            // кончало
                            while (prevstart > 0 && char.IsDigit(ex[prevstart])){
                                prevstart = prevstart - 1;
                            }
                            // выкончало
                            addn = ex.Substring(prevstart, prevend - prevstart) + "*";
                        }
                    }
                    int start = i + 1; 
                    // начало
                    int end = start;
                    // кончало
                    while (end < length && char.IsDigit(ex[end])){
                        end = end + 1;
                    }
                    // выкончало
                    string numberStr = ex.Substring(start, end - start);
                    //start начало второе длинна
                    if (double.TryParse(numberStr, out double number)){
                        double root = Math.Sqrt(number);
                        output += addn;
                        output += root.ToString();
                        i = end - 1;
                    }
            }
                else{ 
                    output += ex[i];
                }
            }
            return output;
        }
        static string SolveWithDatatables(string expression){
            if (!expression.Contains("∞")){
                var dataTable = new DataTable();
                var value = dataTable.Compute(expression, string.Empty);
                return Convert.ToString(value);
                
            }
            else {
                return "Ошибка";
            }
        }
    }
}
