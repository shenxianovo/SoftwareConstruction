using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW05.Services;

public interface ICalculator
{
    public decimal Calculate(string expression);
}
