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
        Stack<string> expression = new Stack<string>();        
        bool firstZero, lastInputIsOperator;
        Button selectedBtn;

        private void Init(bool isEqualPressed = false, bool isOperatorPressed = false)
        {

            if (!isEqualPressed)
                resultLabel.Content = "0";

            if (!isOperatorPressed)
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
            if (resultLabelExp.Content.ToString().Trim() != "" && lastInputIsOperator == false)
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
                if (resultLabelExp.Content.ToString().Any()) // resultLabelExp.Content.ToString() == ""
                {                    
                    ReplaceLastChar(selectedValue);
                }
            }
            else
            {
                AppendExp(selectedValue);
            }
            
            Init(false, true);
        }

        private void DecimalButton_Click(object sender, RoutedEventArgs e)
        {
            selectedBtn = sender as Button;
            selectedValue = selectedBtn.Content.ToString();

            if (!lastNumberString.Contains("."))           
            {
                if (!lastNumberString.Any()) //  lastNumberString == ""
                {
                    lastNumberString += "0.";
                    resultLabelExp.Content += "0.";
                }
                else
                {
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
                ReplaceLastChar(selectedValue);
                firstZero = false;
            }
            else
            {
                AppendExp(selectedValue);
            }

            lastInputIsOperator = false;
            //lastInputType = LastInputType.Number;
        }

        private void ZeroButton_Click(object sender, RoutedEventArgs e)
        {
            selectedBtn = sender as Button;
            selectedValue = selectedBtn.Content.ToString();

            if (lastInputIsOperator)
            {
                // First zero assigned here              
                AppendExp(selectedValue);

                firstZero = true;
                // Do nothing
            }
            else if (!firstZero)
            { 
                AppendExp(selectedValue);
            }

            lastInputIsOperator = false;         
        }

        private void AppendExp(string _selectedValue)
        {
            lastNumberString += _selectedValue.ToString();
            resultLabelExp.Content += _selectedValue.ToString();
        }

        private void ReplaceLastChar(string _selectedValue)
        {
            // Replace
            lastNumberString = _selectedValue;

            // Extract whole string minus last char using substring.
            resultLabelExp.Content = resultLabelExp.Content.ToString().Substring(0, resultLabelExp.Content.ToString().Length - 1);

            resultLabelExp.Content += lastNumberString;
        }
    }
}