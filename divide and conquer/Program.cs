using System.Numerics;
using divide_and_conquer;
using BigInteger = divide_and_conquer.BigInteger;

bool IsOperator(char s)
{
    if (s is '+' or '-' or '*' or '/' or '^')
    {
        return true;
    }

    return false;
}

string MakeNumber(Queue b)
{
    string result = "";
    for (int i = 0; i < b.GetLength(); i++)
    {
        result += b.GetValue(i);
    }

    return result;
}


int OperatorConvert(string s)
{
    if (s is "+" or "-")
    {
        return 2;
    }
    else if (s is "*" or "/")
    {
        return 3;
    }
    else if (s is "^")
    {
        return 4;
    }

    return 0;
}

ArrayList Tokenize()
{
    string? newInput = Console.ReadLine();
    var b = new Queue();
    var tokens = new ArrayList();
    for (int i = 0; i < newInput!.Length; i++)
    {
        char s = newInput[i];
        if (Char.IsDigit(s))
        {
            b.Enque(Char.ToString(s));
        
        }
        else if (IsOperator(s) || (s is ')' or '('))
        {
            if (b.GetLength() > 0)
            {
                string number = MakeNumber(b);
                tokens.Add(number);
                b = new Queue();
            }
            tokens.Add(Char.ToString(s));
        
        }
    }

    if (b.GetLength() > 0)
    {
        string number = MakeNumber(b);
        tokens.Add(number);
    }

    return tokens;
}

bool IsNumber(string element)
{
    foreach (var symbol in element)
    {
        if (Char.IsDigit(symbol))
        {
            continue;
        }
        else
        {
            return false;
        }
    }

    return true;
}
ArrayList Postfix(ArrayList tokens)
{
    var operators = new Stack();
    var output = new ArrayList();
    for (int i = 0; i < tokens.GetLenght(); i++)
    {
        string currentElement = tokens.GetElement(i);
        // if number
        if (IsNumber(currentElement))
        {
            output.Add(currentElement);
        }
        // if operator
        else if (IsOperator(char.Parse(currentElement)))
        {
            while (operators.GetLength() > 0 && operators.PullCopy() != "(" &&
                   OperatorConvert(currentElement) <= OperatorConvert(operators.PullCopy()) &&
                   currentElement != "^")
            {
                output.Add(operators.Pull());
            }

            operators.Push(currentElement);
        }

        else if (currentElement == "(")
        {
            operators.Push(currentElement);
        }
        else if (currentElement == ")")
        {
            while (operators.GetLength() > 0 && operators.PullCopy() != "(")
            {
                output.Add(operators.Pull());
            }

            if (operators.PullCopy() == "(")
            {
                operators.Pull();
            }
            else
            {
                throw new Exception("there are mismatched parenthesis");
            }
        }

    }
    


    while (operators.GetLength() > 0)
    {
        var operatorToAdd = operators.Pull();
        if (operatorToAdd != "(")
        {
            output.Add(operatorToAdd);
        }
        else
        {
            throw new Exception("mismatched parethesis");
        }
    }
    return output;
}

string MakeOperation(string firstNumber, string secondNumber, string currentOperator)
{
    BigInteger numberOne = new BigInteger(firstNumber);
    BigInteger numberTwo = new BigInteger(secondNumber);
    string result = "";
    BigInteger bigintResult = new BigInteger("0");
    switch (currentOperator)
    {
        case "+":
            bigintResult = numberOne + numberTwo;
            break;
        case "-":
            bigintResult = numberOne - numberTwo;
            break;
        case "*":
            bigintResult = numberOne * numberTwo;
            break;
        // case "/":
        //     if (numberTwo == new BigInteger("0"))
        //     {
        //         throw new Exception("you cannot divide by 0");
        //     }
        //     bigintResult = numberOne / numberTwo;
        //     break;
        // case "^":
        //     bigintResult = Convert.ToDouble(Math.Pow(numberOne, numberTwo));
        //     break;
        default:
            throw new Exception("the operator is invalid");
    }

    result = bigintResult.ToString();
    return result;
}


string Count(ArrayList tokens)
{
    Stack s = new Stack();
    for (int i = 0; i < tokens.GetLenght(); i++)
    {
        string currentElement = tokens.GetElement(i);
        if (IsNumber(currentElement))
        {
            s.Push(currentElement);
        }
        else
        {
            string secondNumber = s.Pull();
            string firstNumber = s.Pull();
            string result = MakeOperation(firstNumber, secondNumber, currentElement);
            s.Push(result);
        }
    }

    return s.Pull();
}

ArrayList myTokens = Tokenize();
ArrayList postfix = Postfix(myTokens);
string result = Count(postfix);
Console.WriteLine(result);



          


