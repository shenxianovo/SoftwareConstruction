using HW05.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW05.Models;

public class Main
{
    private readonly ICalculator calculator;

    public decimal? Calculate(string left, string right, string op)
    {
        decimal.TryParse(left, out var numLeft);
        decimal.TryParse(right, out var numRight);

        return calculator.Calculate(numLeft, numRight, op);
    }

    public Main(ICalculator calculator)
    {
        this.calculator = calculator;
    }
}
