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
        LastInputType lastInputType;
        bool firstZero;
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
            lastInputType = LastInputType.Operator;
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
            if (resultLabelExp.Content.ToString().Trim() != "" && lastInputType != LastInputType.Operator)
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

            if (lastInputType == LastInputType.Operator)
            {
                if (resultLabelExp.Content.ToString() == "")
                {
                    // Do nothing
                }
                else
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

            if (lastNumberString.Contains("."))
            {
                // Do nothing
            }
            else
            {
                if (lastNumberString == "")
                {
                    lastNumberString += "0.".ToString();
                    resultLabelExp.Content += "0.".ToString();
                }
                else
                {
                    // Append
                    AppendExp(selectedValue);                    
                }
            }
            firstZero = false;
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            selectedBtn = sender as Button;
            selectedValue = selectedBtn.Content.ToString();

            switch (lastInputType)
            {
                case LastInputType.Operator:
                case LastInputType.Zero:
                    // firstZero value is assigned at Zero button click handler 
                    if (firstZero)
                    {                     
                        ReplaceLastChar(selectedValue);
                        firstZero = false;
                    }
                    else
                    {
                        // Append
                        AppendExp(selectedValue);
                    }

                    break;
                case LastInputType.Number:
                    // Append
                    AppendExp(selectedValue);

                    break;
                default:
                    break;
            }
            lastInputType = LastInputType.Number;
        }

        private void ZeroButton_Click(object sender, RoutedEventArgs e)
        {
            selectedBtn = sender as Button;
            selectedValue = selectedBtn.Content.ToString();

            if (lastNumberString == "")
            {
                // First zero assigned                
                AppendExp(selectedValue);

                firstZero = true;
                // Do nothing
            }
            else if (lastNumberString.Length == 1 && lastNumberString == "0")
            {
                firstZero = true;
                // Do nothing
                // To block 00
            }
            else
            {
                // Append. i.e. 100
                AppendExp(selectedValue);
            }

            lastInputType = LastInputType.Zero;
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

            // Extract whole string without last char using substring.
            resultLabelExp.Content = resultLabelExp.Content.ToString().Substring(0, resultLabelExp.Content.ToString().Length - 1);

            resultLabelExp.Content += lastNumberString;
        }
    }

    public enum LastInputType
    {
        Zero, Number, Operator
    }
}