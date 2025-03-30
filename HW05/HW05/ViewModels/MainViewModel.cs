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

    [ObservableProperty]
    public partial string ExpressionText { get; set; }

    [ObservableProperty]
    public partial string NumberText { get; set; }

    [ObservableProperty]
    public partial string Left { get; set; } = "";
    [ObservableProperty]
    public partial string Op { get; set; } = "";
    [ObservableProperty]
    public partial string Right { get; set; } = "";

    public Main Model { get; }

    private bool isLeftFinished = false;

    private bool isCalcFinished = false;

    public void Clear()
    {
        ClearText();
        ClearNumber();
    }

    private void ClearText() => ExpressionText = NumberText = "";
    private void ClearNumber() => Left = Right = Op = "";

    public void InputNumber(string content)
    {
        if (!isLeftFinished)
        {
            if (Left == "0" && content != ".") Left = content;
            else if (content == "%")
            {
                decimal.TryParse(Left, out var n);
                Left = (n / 100).ToString();
            }
            else Left += content;

            NumberText = Left;
        }
        else
        {
            if (Right == "0" && content != ".") Right = content;
            else if (content == "%")
            {
                decimal.TryParse(Right, out var n);
                Right = (n / 100).ToString();
            }
            else Right += content;

            NumberText = Right;
        }
    }
    public void InputOp(string content)
    {
        Op = content;

        ExpressionText = Left + Op;
    }

    public void Calculate()
    {
        try
        {
            isCalcFinished = true;
            isLeftFinished = false;

            ClearText();

            ExpressionText = Left + Op + Right + "=";
            var result = Model.Calculate(Left, Right, Op).ToString();

            ClearNumber();

            NumberText = result;
        }
        catch(Exception ex)
        {
            if (ex is DivideByZeroException)
                NumberText = "除数不能为零";

            ClearNumber();
        }
    }

    private bool IsOperator(string content) => content == "+" || content == "-" || content == "×" || content == "÷";

    private bool IsNumber(string content) => int.TryParse(content, out var num) || content == "." || content == "%";

    public void AppendDigit(string content)
    {
        if (isCalcFinished)
        {
            ExpressionText = "";
            isCalcFinished = false;
        }

        if (IsNumber(content))
        {
            InputNumber(content);
            return;
        }

        if (IsOperator(content) && Left.Length > 0)
        {
            isLeftFinished = true;
            InputOp(content);
            return;
        }
    }

    public void RemoveDigit()
    {
        if (!isLeftFinished)
        {
            if (Left.Length > 1)
                Left = Left.Substring(0, Left.Length - 1);
            else
                Left = "0";

            NumberText = Left;
        }
        else if (isLeftFinished)
        {
            if (Right.Length > 1)
                Right = Right.Substring(0, Right.Length - 1);
            else
                Right = "0";

            NumberText = Right;
        }
    }

    public MainViewModel(Main model)
    {
        Model = model;
    }
}
