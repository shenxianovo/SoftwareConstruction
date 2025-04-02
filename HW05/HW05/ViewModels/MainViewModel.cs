using CommunityToolkit.Mvvm.ComponentModel;
using HW05.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW05;

public partial class MainViewModel : ObservableObject
{
    private string left = "0";
    private string op = "";
    private string right = "0";

    private CalcState state = CalcState.INIT;

    [ObservableProperty]
    public partial string ExpressionText { get; set; }

    [ObservableProperty]
    public partial string NumberText { get; set; }

    public Main Model { get; }

    private enum CalcState
    {
        INIT,
        LEFT,
        OP,
        RIGHT
    }

    public MainViewModel(Main model)
    {
        Model = model;
    }

    public void Clear()
    {
        ClearText();
        ClearNumber();
        state = CalcState.INIT;
    }

    public void AppendDigit(string content)
    {
        switch (state)
        {
            case CalcState.INIT:
                if (IsNumber(content))
                {
                    Clear();
                    AppendNumber(content, ref left);
                    state = CalcState.LEFT;
                }
                else if (IsOperator(content))
                {
                    AppendOperator(content);
                    state = CalcState.OP;
                }
                break;
            case CalcState.LEFT:
                if (IsNumber(content))
                {
                    AppendNumber(content, ref left);
                    state = CalcState.LEFT;
                }
                else if (IsOperator(content) && left.Length > 0)
                {
                    AppendOperator(content);
                    state = CalcState.OP;
                }
                else if (content == "=")
                {
                    AppendOperator(content);
                    state = CalcState.INIT;
                }
                break;

            case CalcState.OP:
                if (IsNumber(content))
                {
                    AppendNumber(content, ref right);
                    state = CalcState.RIGHT;
                }
                else if (IsOperator(content))
                {
                    AppendOperator(content);
                }
                break;

            case CalcState.RIGHT:
                if (IsNumber(content))
                {
                    AppendNumber(content, ref right);
                }
                else if (IsOperator(content))
                {
                    Stage();
                    op = content;
                    ExpressionText = left + op;
                    state = CalcState.OP;
                }
                else if (content == "=")
                {
                    ExpressionText = left + op + right + "=";
                    Stage();
                    state = CalcState.INIT;
                }
                break;
        }
    }

    public void RemoveDigit()
    {
        switch (state)
        {
            case CalcState.LEFT:
                RemoveLastDigit(ref left);
                break;

            case CalcState.RIGHT:
                RemoveLastDigit(ref right);
                break;
        }
    }

    private void ClearText() => ExpressionText = NumberText = "";
    private void ClearNumber() => left = right = op = "";

    private bool IsNumber(string content) => decimal.TryParse(content, out _) || content == "." || content == "%";
    private bool IsOperator(string content) => content is "+" or "-" or "×" or "÷";

    private void AppendNumber(string content, ref string target)
    {
        if (target.Contains(".") && content == ".")
            return;

        if (content == "%")
        {
            decimal.TryParse(target, out var result);
            target = (result / 100).ToString();
            NumberText = target;
            return;
        }

        switch (target)
        {
            case "0" when content != ".":
                target = content;
                break;
            case "" when content == ".":
                target = "0.";
                break;
            default:
                target += content;
                break;
        }

        NumberText = target;
    }
    private void AppendOperator(string content)
    {
        op = content;
        ExpressionText = left + op;
    }

    private decimal Calculate()
    {
        return Model.Calculate(left, right, op);
    }

    private void Stage()
    {
        try
        {
            left = Calculate().ToString();
            NumberText = left;
            right = "";
        }
        catch (Exception ex)
        {
            if (ex is DivideByZeroException)
            {
                Clear();
                NumberText = "除数不能为零";
            }
        }
    }

    private void RemoveLastDigit(ref string target)
    {
        if (target.Length > 1)
            target = target.Substring(0, target.Length - 1);
        else
            target = "0";
        NumberText = target;
    }
}
