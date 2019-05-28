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
            firstZero = false;
            resultLabelExp.Content = "";
            lastNumberString = "";
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
            resultLabel.Content = MathBodmas.EvalExpression(resultLabelExp.Content.ToString().ToCharArray()).ToString();
            Init();
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
            resultLabelExp.Content = $"{resultLabelExp.Content}{operatorBtn.Content.ToString()}";
            lastInputType = lastInputType.Operator;
            lastNumber = 0;
            lastNumberString = "";
            firstZero = false;
        }

        private void pointButton_Click(object sender, RoutedEventArgs e)
        {
            if (resultLabel.Content.ToString().Contains("."))
            {
                // Do nothing
            }
            else
            {
                resultLabel.Content = $"{resultLabel.Content}.";
            }
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            var numberBtn = sender as Button;

            var selectedValue = numberBtn.Content.ToString();

            switch (lastInputType)
            {
                case lastInputType.Operator:
                case lastInputType.Zero:
                    if(lastNumberString == "0")
                    {

                        // Special checking for the first lastNumberString
                        if(firstZero)
                        {
                            // Replace
                            lastNumberString = selectedValue;

                            // Extract whole string without last char using substring.
                            resultLabelExp.Content = resultLabelExp.Content.ToString().Substring(0, resultLabelExp.Content.ToString().Length - 1);

                            resultLabelExp.Content += lastNumberString;
                            
                            
                            firstZero = false;
                        }
                        else
                        {
                            // Append
                            resultLabelExp.Content += lastNumberString;
                        }
                        
                    }
                    else
                    {
                        // Append
                        lastNumberString += selectedValue;
                        resultLabelExp.Content += selectedValue;
                    }

                    break;
                case lastInputType.Number:
                    // Append
                    lastNumber += Convert.ToDouble(selectedValue);
                    resultLabelExp.Content = $"{resultLabelExp.Content}{selectedValue}";
                    lastInputType = lastInputType.Number;
                    break;
                
                
                default:
                    break;
            }

        }

        private void ZeroButton_Click(object sender, RoutedEventArgs e)
        {
            var zeroButton = sender as Button;
            var selectedValue = zeroButton.Content.ToString();
            
            // If lastNumber is firstZero, do nothing
            if (lastNumberString == "")
            {
                lastNumberString = "0";
                resultLabelExp.Content += 0.ToString();
                firstZero = true;
                // Do nothing
            }
            else if (lastNumberString.Length == 1 && lastNumberString == "0"){
                // Do nothing
            }
            else
            {
                switch (lastInputType)
                {
                    case lastInputType.Zero:
                        //// Append
                        resultLabelExp.Content += 0.ToString();
                        //firstZero = true;
                        break;
                    case lastInputType.Number:
                        lastNumberString += "0";
                        // Append
                        resultLabelExp.Content += selectedValue.ToString();
                        break;
                    case lastInputType.Operator:
                        // Append
                        resultLabelExp.Content += selectedValue.ToString();
                        break;
                    default:
                        break;
                }
                
            }

            lastInputType = lastInputType.Zero;
        }
    }

    public enum lastInputType
    {
        Zero, Number, Operator
    }
}