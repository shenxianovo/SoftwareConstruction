using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW05.Services;

class Calculator : ICalculator
{
    public decimal Calculate(string expression)
    {
        decimal.TryParse(expression, out var result);
        return result;
    }
}
