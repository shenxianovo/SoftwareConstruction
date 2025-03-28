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

    public Main(ICalculator calculator)
    {
        this.calculator = calculator;
    }
}
