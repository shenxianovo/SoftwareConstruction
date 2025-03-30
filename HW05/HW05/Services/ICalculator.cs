using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW05.Services;

public interface ICalculator
{
    public decimal Calculate(decimal left, decimal right, string op);
    public decimal Add(decimal left, decimal right);
    public decimal Subtract(decimal left, decimal right);
    public decimal Multiply(decimal left, decimal right);
    public decimal Divide(decimal left, decimal right);
}
