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
        string lastNumberString;
        lastInputType lastInputType;
        bool firstZero;

        private void Init()
        {
            resultLabelExp.Content = "";

            // This reset is similar with after key in operator reset but without the above line
            firstZero = false;
            lastNumberString = "";
            lastNumber = 0;
            lastInputType = lastInputType.Operator;
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
            if (resultLabelExp.Content.ToString().Trim() != "" && lastInputType != lastInputType.Operator)
            {
                resultLabel.Content = MathBodmas.EvalExpression(resultLabelExp.Content.ToString().ToCharArray()).ToString();
                Init();
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
            resultLabelExp.Content = "";
            resultLabel.Content = "0";
            Init();
        }

        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            var operatorBtn = sender as Button;
            var selectedValue = operatorBtn.Content.ToString();

            if (lastInputType == lastInputType.Operator)
            {
                if (resultLabelExp.Content.ToString() == "")
                {
                    // Do nothing
                }
                else
                {
                    //// Extract whole string without last char using substring.
                    //resultLabelExp.Content = resultLabelExp.Content.ToString().Substring(0, resultLabelExp.Content.ToString().Length - 1);

                    //// Replace
                    //resultLabelExp.Content += selectedValue;

                    ReplaceLastChar(selectedValue);
                }
            }
            else
            {
                AppendExp(selectedValue);
                //resultLabelExp.Content = $"{resultLabelExp.Content}{operatorBtn.Content.ToString()}";                                
            }

            lastInputType = lastInputType.Operator;
            lastNumber = 0;
            lastNumberString = "";
            firstZero = false;
        }

        private void pointButton_Click(object sender, RoutedEventArgs e)
        {
            var numberBtn = sender as Button;
            var selectedValue = numberBtn.Content.ToString();

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
            var numberBtn = sender as Button;
            var selectedValue = numberBtn.Content.ToString();

            switch (lastInputType)
            {
                case lastInputType.Operator:
                case lastInputType.Zero:
                    // firstZero value is assigned at Zero button click handler 
                    if (firstZero)
                    {
                        // Replace
                        //lastNumberString = selectedValue;

                        //// Extract whole string without last char using substring.
                        //resultLabelExp.Content = resultLabelExp.Content.ToString().Substring(0, resultLabelExp.Content.ToString().Length - 1);

                        //resultLabelExp.Content += lastNumberString;

                        ReplaceLastChar(selectedValue);

                        firstZero = false;
                    }
                    else
                    {
                        // Append
                        AppendExp(selectedValue);
                    }

                    break;
                case lastInputType.Number:
                    // Append
                    AppendExp(selectedValue);

                    break;
                default:
                    break;
            }
            lastInputType = lastInputType.Number;
        }

        private void ZeroButton_Click(object sender, RoutedEventArgs e)
        {
            var zeroButton = sender as Button;
            var selectedValue = zeroButton.Content.ToString();

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

            lastInputType = lastInputType.Zero;
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


    public enum lastInputType
    {
        Zero, Number, Operator
    }
}