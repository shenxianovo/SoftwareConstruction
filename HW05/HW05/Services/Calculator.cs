using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW05.Services;

class Calculator : ICalculator
{
    public decimal Calculate(decimal left, decimal right, string op)
    {
        switch (op)
        {
            case "+":
                return Add(left, right);
            case "-":
                return Subtract(left, right);
            case "×":
                return Multiply(left, right);
            case "÷":
                return Divide(left, right);
            default:
                break;
        }
        return 0;
    }

    public decimal Add(decimal left, decimal right)
    {
        return left + right;
    }

    public decimal Subtract(decimal left, decimal right)
    {
        return left - right;
    }

    public decimal Multiply(decimal left, decimal right)
    {
        return left * right;
    }

    public decimal Divide(decimal left, decimal right)
    {
        return left / right;
    }
}