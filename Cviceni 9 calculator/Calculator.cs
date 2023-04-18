using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Cviceni_09_calculator
{
    public class Calculator
    {
        public string Memory;
        private string firstNumber;
        private string secondNumber;

        private string operand;
        // integer overflow error - too small or too large number
        // use double to to avoid the error
        private int result;
        private double result_for_sum;
        private double numInMemory;
        private enum State
        {
            FirstNumber,
            Operation,
            SecondNumber,
            Result,
            Result_for_sum
        };
        private State _state;
        public Calculator()
        {
            Memory = "";
            firstNumber = "";
            secondNumber = "";
            operand = "";
            result = 0;
            result_for_sum = 0;
            numInMemory = 0;
            _state = State.FirstNumber;
        }
        public string Display
        {
            get
            {
                switch (_state)
                {
                    case State.FirstNumber:
                        return firstNumber;
                    case State.Operation:
                        return firstNumber + operand;
                    case State.SecondNumber:
                        return firstNumber + operand + secondNumber;
                    case State.Result:
                        return result.ToString();
                    case State.Result_for_sum:
                        return result_for_sum.ToString();
                    default:
                        return "";
                }
            }
        }
        public void Button(string buttonContent)
        {
            switch (buttonContent)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                case "f":
                    processNumberPress(buttonContent);
                    break;
                case "MC":
                case "MR":
                case "MS":
                case "M+":
                case "M-":
                    processMemoryOperation(buttonContent);
                    break;
                case "<=":
                case "CE":
                case "C":
                    processClearOperation(buttonContent);
                    break;
                case "+/-":
                case "+":
                case "-":
                case "/":
                case "*":
                case " KC ":
                case ",":
                    processOperand(buttonContent);
                    break;
                case "=":
                    //if (_state == State.SecondNumber) calculateResult();
                    calculateResult();
                    break;
                case "!":
                    operand = "!";
                    calculateResult();
                    break;
                case "KC":
                    operand = "KC";
                    calculateResult();
                    break;
                case "SUM":
                    operand = "SUM";
                    calculateResult();
                    break;
            }
        }
        private void processNumberPress(string number)
        {
            switch (_state)
            {
                case State.FirstNumber:
                    firstNumber += number;
                    break;
                case State.SecondNumber:
                    secondNumber += number;
                    break;
                case State.Operation:
                    secondNumber += number;
                    _state = State.SecondNumber;
                    break;
            }
        }
        private void processMemoryOperation(string operationButton)
        {
            switch (operationButton)
            {
                case "MC":
                    numInMemory = 0;
                    Memory = "";
                    break;
                case "MR":
                    if (Memory == "") break;
                    switch (_state)
                    {
                        case State.FirstNumber:
                            firstNumber = numInMemory.ToString();
                            break;
                        case State.Operation:
                            secondNumber = numInMemory.ToString();
                            _state = State.SecondNumber;
                            break;
                        case State.SecondNumber:
                            secondNumber = numInMemory.ToString();
                            break;
                    }
                    break;
                case "MS":
                    switch (_state)
                    {
                        case State.FirstNumber:
                            if (firstNumber != "") numInMemory = Double.Parse(firstNumber);
                            break;
                        case State.SecondNumber:
                            if (secondNumber != "") numInMemory = Double.Parse(secondNumber);
                            break;
                    }
                    Memory = "M";
                    break;
                case "M+":
                    if (Memory == "") break;
                    switch (_state)
                    {
                        case State.FirstNumber:
                            if (firstNumber != "") numInMemory += Double.Parse(firstNumber);
                            break;
                        case State.Operation:
                            break;
                        case State.SecondNumber:
                            if (secondNumber != "") numInMemory += Double.Parse(secondNumber);
                            break;
                    }
                    break;
                case "M-":
                    if (Memory == "") break;
                    switch (_state)
                    {
                        case State.FirstNumber:
                            if (firstNumber != "") numInMemory -= Double.Parse(firstNumber);
                            break;
                        case State.Operation:
                            break;
                        case State.SecondNumber:
                            if (secondNumber != "") numInMemory -= Double.Parse(secondNumber);
                            break;
                    }
                    break;
            }
        }
        private void processClearOperation(string clearButton)
        {
            switch (clearButton)
            {
                case "<=":
                    switch (_state)
                    {
                        case State.FirstNumber:
                            if (firstNumber.Length > 0) firstNumber = firstNumber.Substring(0, firstNumber.Length - 1);
                            if (firstNumber == "-") firstNumber = "";
                            break;
                        case State.Operation:
                            _state = State.FirstNumber;
                            break;
                        case State.SecondNumber:
                            if (secondNumber.Length > 0)
                            {
                                secondNumber = secondNumber.Substring(0, secondNumber.Length - 1);
                                if (secondNumber == "-") secondNumber = "";
                            }
                            else _state = State.FirstNumber;
                            break;
                    }
                    break;
                case "C":
                    clearDisplay();
                    break;
                case "CE":
                    switch (_state)
                    {
                        case State.FirstNumber:
                            firstNumber = "";
                            break;
                        case State.SecondNumber:
                            secondNumber = "";
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }
        private void processOperand(string operandButton)
        {
            switch (operandButton)
            {
                case "+/-":
                    switch (_state)
                    {
                        case State.FirstNumber:
                            negateString(ref firstNumber);
                            break;
                        case State.Operation:
                            secondNumber = negateString(firstNumber);
                            _state = State.SecondNumber;
                            break;
                        case State.SecondNumber:
                            negateString(ref secondNumber);
                            break;
                    }
                    break;
                case "+":
                case "-":
                case "!":
                case "SUM":
                case "*":
                case "/":
                    switch (_state)
                    {
                        case State.FirstNumber:
                            if (firstNumber == "") firstNumber = "0";
                            break;
                        case State.SecondNumber:
                            calculateResult();
                            firstNumber = result.ToString();
                            break;
                        default:
                            break;
                    }
                    operand = operandButton;
                    _state = State.Operation;
                    break;
                case ",":
                    switch (_state)
                    {
                        case State.FirstNumber:
                            if (firstNumber == "") firstNumber = "0";
                            firstNumber += ",";
                            break;
                        case State.SecondNumber:
                            if (secondNumber == "") secondNumber = "0";
                            secondNumber += ",";
                            break;
                        case State.Operation:
                            secondNumber = "0,";
                            _state = State.SecondNumber;
                            break;
                    }
                    break;

            }
        }
        private void calculateResult()
        {
            if (firstNumber == "") firstNumber = "0";
            if (secondNumber == "") secondNumber = "0";
            switch (operand)
            {
                //string format error 
                //int is only whole number without decimal point
                case "+":
                    checked
                    {
                        try
                        {
                            result = int.Parse(firstNumber) + int.Parse(secondNumber);
                        }
                        catch (OverflowException)
                        {
                            MessageBox.Show("Nice you find Integer Overflow Exception");
                        }
                        catch (FormatException)
                        {
                            MessageBox.Show("Well done you find String Format Exception");
                        }
                    }
                    break;
                case "-":
                    result = int.Parse(firstNumber) - int.Parse(secondNumber);
                    break;
                case "*":
                    result = int.Parse(firstNumber) * int.Parse(secondNumber);
                    //string format error 
                    //int is only whole number without decimal point
                    //counting with whole number
                    break;
                case "/":
                    result = int.Parse(firstNumber) / int.Parse(secondNumber);
                    break;
                case "KC":
                    result = (int)(int.Parse(firstNumber) * 0.265);
                    break;
                case "!":
                    try
                    {
                        result = factorialFunction(int.Parse(firstNumber));
                    }
                    catch (StackOverflowException)
                    {
                        MessageBox.Show("Congrats you find Stuck Overflow Exception");
                    }
                    break;
                case "SUM":
                    result_for_sum = sumFunction(Double.Parse(firstNumber));
                    break;
                default:
                    break;
            }
            firstNumber = "";
            secondNumber = "";
            if (operand != "SUM")
            {
                _state = State.Result;
            }
            else
            {
                _state = State.Result_for_sum;
            }
        }

        //function for coounting factorial
        //stuck overflow error (recursion)
        //use cycle (for) to avoid this error
        //31 ok, 32-33 int overwlow, 34-16000 = 0, 17000 stack overwlow

        static public int factorialFunction(int Fnumber)
        {      
                //if (firstNumber == "") firstNumber = "0";            
            
                if (Fnumber == 0)
                {
                    return 1;
                }
                else
                {
                    try
                    {
                        return Fnumber * factorialFunction(Fnumber - 1);
                    }
                    catch (StackOverflowException)
                    {
                        MessageBox.Show("Congrats you find Stuck Overflow Exception");
                        return 1;
                    }
                }
      
        }

        //function for counting sum from one number
        //heap error to much variables in arry
        //2 100 000 000 ok, 2 500 000 000 heap overflow
        private double sumFunction(double a)
        {
            try
            {
                double[] arr = new double[(int)a + 1];
                double sum = 0;
                for (double i = a; i >= 0; i--)
                {
                    // if (i > sum) // heap limit check
                    //{
                    arr[(int)i] = i;
                    //}
                    sum += arr[(int)i];
                }
                return sum;
            }
            catch (OverflowException)
            {
                MessageBox.Show("Congrats you find Heap Overflow Exception");
                return 0;
            }
        }
        private void clearDisplay()
        {
            _state = State.FirstNumber;
            firstNumber = "";
            secondNumber = "";
        }
        private void negateString(ref string input)
        {
            if (input.Length == 0 || input == "0") return;
            if (input[0] == '-') input = input.Substring(1);
            else input = "-" + input;
        }
        private string negateString(string input)
        {
            if (input.Length == 0 || input == "0") return "";
            if (input[0] == '-') return input.Substring(1);
            else return "-" + input;
        }
    }
}
