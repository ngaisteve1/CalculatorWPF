using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    class MathBodmas
    {
        //  Implementation
        //  string exp = "2*2+1";
        //  Console.WriteLine(MathParser.EvalExpression(exp.ToCharArray()).ToString());
        public static double EvalExpression(char[] expr)
        {
            return parseSummands(expr, 0);
        }

        private static double parseSummands(char[] expr, int index)
        {
            // index is passed as reference to maintain one copy throughout
            double x = parseFactors(expr, ref index);
            while (true)
            {
                char op = expr[index];
                if (op != '+' && op != '-')
                    return x;

                index++;
                double y = parseFactors(expr, ref index);
                if (op == '+')
                    x += y;
                else
                    x -= y;
            }
        }

        private static double parseFactors(char[] expr, ref int index)
        {
            double x = GetDouble(expr, ref index);
            while (true)
            {
                char op = expr[index];
                if (op != '/' && op != '*')
                    return x;

                index++;
                double y = GetDouble(expr, ref index);
                if (op == '/')
                    x /= y;
                else
                    x *= y;
            }
        }

        private static double GetDouble(char[] expr, ref int index)
        {
            string dbl = "";

            //ASCII table:
            //46 . 2E
            //48 0 30
            //49 1 31
            //50 2 32
            //51 3 33
            //52 4 34
            //53 5 35
            //54 6 36
            //55 7 37
            //56 8 38
            //57 9 39
            while (((int)expr[index] >= 48 && (int)expr[index] <= 57) || expr[index] == 46)
            {
                dbl = dbl + expr[index].ToString();
                index++;
                if (index == expr.Length)
                {
                    index--;
                    break;
                }
            }
            return double.Parse(dbl);
        }
    }
}
