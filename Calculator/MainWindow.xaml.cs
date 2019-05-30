using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        double lastNumber;
        string lastNumberString, selectedValue;
        LinkedList<string> expression = new LinkedList<string>();        
        bool firstZero, lastInputIsOperator;
        Button selectedBtn;

        private void Init(bool isEqualPressed = false)
        {
            if (!isEqualPressed)
                resultLabel.Content = "0";

            if (!lastInputIsOperator)
                resultLabelExp.Content = "";


            // This reset is similar with after key in operator reset but without the above line
            firstZero = false;
            lastNumberString = "";
            lastNumber = 0;
            lastInputIsOperator = true;
        }

        public MainWindow()
        {
            InitializeComponent();

            acButton.Click += AcButton_Click;
            negativeButton.Click += NegativeButton_Click;
            percentageButton.Click += PercentageButton_Click;
            equalButton.Click += EqualButton_Click;

            Init();
        }

        private void EqualButton_Click(object sender, RoutedEventArgs e)
        {
            if (resultLabelExp.Content.ToString().Any() && lastInputIsOperator == false)
            {
                resultLabel.Content = MathBodmas.EvalExpression(resultLabelExp.Content.ToString().ToCharArray()).ToString();
                Init(true);
            }
        }

        private void PercentageButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(resultLabel.Content.ToString(), out lastNumber))
            {
                lastNumber = lastNumber / 100;
                resultLabel.Content = lastNumber.ToString();
            }
        }

        private void NegativeButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(resultLabel.Content.ToString(), out lastNumber))
            {
                lastNumber = lastNumber * -1;
                resultLabel.Content = lastNumber.ToString();
            }
        }

        private void AcButton_Click(object sender, RoutedEventArgs e)
        {            
            Init();
        }

        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            selectedBtn = sender as Button;
            selectedValue = selectedBtn.Content.ToString();

            if (lastInputIsOperator)
            {
                if (resultLabelExp.Content.ToString().Any()) 
                {                    
                    // 1+ -> 1-
                    ReplaceLastChar(selectedValue);
                }
            }
            else
            {
                // 1 -> 1+
                AppendExp(selectedValue);
            }

            lastInputIsOperator = true;
            Init(false);
        }

        private void DecimalButton_Click(object sender, RoutedEventArgs e)
        {
            selectedBtn = sender as Button;
            selectedValue = selectedBtn.Content.ToString();

            if (!lastNumberString.Contains("."))           
            {
                if (string.IsNullOrEmpty(lastNumberString))
                {
                    // '' -> 0.
                    AppendExp("0.");
                }
                else
                {
                    // 1 -> 1.
                    AppendExp(selectedValue);                    
                }
            }
            firstZero = false;
            lastInputIsOperator = false;
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            selectedBtn = sender as Button;
            selectedValue = selectedBtn.Content.ToString();

            if (firstZero)
            {
                // 0 -> 1
                ReplaceLastChar(selectedValue);
                firstZero = false;
            }
            else
            {
                // 10 -> 101
                AppendExp(selectedValue);
            }

            lastInputIsOperator = false;
        }

        private void ZeroButton_Click(object sender, RoutedEventArgs e)
        {
            selectedBtn = sender as Button;
            selectedValue = selectedBtn.Content.ToString();

            if (lastInputIsOperator)
            {
                // First zero assigned here    
                // '' -> 0
                AppendExp(selectedValue);

                firstZero = true;
            }
            else if (!firstZero)
            { 
                // 1 -> 10
                AppendExp(selectedValue);
            }

            lastInputIsOperator = false;         
        }

        private void AppendExp(string _selectedValue)
        {
            lastNumberString += _selectedValue;
            expression.AddLast(_selectedValue);
            //resultLabelExp.Content += _selectedValue;
            //expression.Reverse();
            resultLabelExp.Content = string.Join("", expression.ToArray());
        }

        private void ReplaceLastChar(string _selectedValue)
        {
            //lastNumberString = _selectedValue;

            // Extract whole string minus last char using substring.
            //resultLabelExp.Content = resultLabelExp.Content.ToString().Substring(0, resultLabelExp.Content.ToString().Length - 1);
            // resultLabelExp.Content += lastNumberString;

            expression.RemoveLast();
            AppendExp(_selectedValue);
        }
    }
}