using System.Diagnostics;
using Microsoft.VisualBasic.CompilerServices;

BigInteger x = new BigInteger("1313234242425");
Console.WriteLine(x);
BigInteger y = new BigInteger("23456789") + new BigInteger("987654321");
Console.WriteLine(y);
BigInteger z = new BigInteger("13") - new BigInteger("14");
Console.WriteLine(z);
BigInteger d = new BigInteger("-1758934753489534") * (new BigInteger("-123578498578345"));
Console.WriteLine($"d is {d}");


public class BigInteger
{
    private int[] _numbers;
    private bool _isPositive = true;
    private bool _isLarger = false;

    public BigInteger(string value)
    {
        if (value[0] == '-')
        {
            _isPositive = false;
            _isLarger = false;
            value = value.Substring(1);
        }

        _numbers = new int[value.Length];
        for (int i = value.Length - 1; i >= 0; i--)
        {
            _numbers[value.Length - 1 - i] = int.Parse(value[i].ToString());
        }
    }

    public BigInteger(int[] numbers, bool isPositive)
    {
        _numbers = numbers;
        _isPositive = _isPositive;
    }
    public override string ToString()
    {
        string result = "";
        if (!_isPositive)
        {
            result += "-";
        }

        for (int i = _numbers.Length - 1; i >= 0; i--)
        {
            result += _numbers[i].ToString();
        }

        return result;
    }

    public BigInteger Add(BigInteger another)
    {
        int[] a = _numbers;
        int[] b = another._numbers;
        int[] result = new int[Math.Max(a.Length, b.Length) + 1];

        int carry = 0;
        if (!_isPositive && another._isPositive)
        {
            this._isPositive = true;
            return another.Sub(this);
        }

        if (_isPositive && !another._isPositive)
        {
            another._isPositive = true;
            return this.Sub(another);
        }
        else
        {
            for (int i = 0; i < result.Length; i++)
            {
                int sum = carry;
                if (i < a.Length) sum += a[i];
                if (i < b.Length) sum += b[i];
                result[i] = sum % 10;
                carry = sum / 10;
            }
            var ready = string.Join("", result.Reverse()).TrimStart('0');
            BigInteger bigIntResult = new BigInteger("0");
            if (ready.Length > 0)
            {
                bigIntResult = new BigInteger(ready);
            }
             
            bigIntResult._isPositive = _isPositive;
            return bigIntResult;
        }
    }


    public BigInteger Sub(BigInteger another)
    {
        int[] a = _numbers;
        int[] b = another._numbers;
        int[] result = new int[Math.Max(a.Length, b.Length)];

        int borrow = 0;
        if (_isPositive && !another._isPositive)
        {
            BigInteger number = new BigInteger(another.ToString().TrimStart('-'));
            return this.Add(number);
        }

        if (a.Length < b.Length)
        {
            this._isLarger = true;
            BigInteger number = another - this;
            number._isPositive = false;
            return number;
        }
        else if ((a[0] < b[0]) && !(_isLarger))
        {
            this._isLarger = true;
            another._isLarger = true;
            BigInteger number = another - this;
            number._isPositive = false;
            return number;
        }
        for (int i = 0; i < result.Length; i++)
        {
            int diff = borrow;
            if (i < a.Length) diff += a[i];
            if (i < b.Length) diff -= b[i];
            result[i] = (diff + 10) % 10;
            if (diff < 0)
            {
                borrow = -1;
            }
            else
            {
                borrow = 0;
            }

        }
        string ready = string.Join("", result.Reverse()).TrimStart('0');
        BigInteger bigIntResult = new  BigInteger("0");
        if (ready.Length>0) bigIntResult= new BigInteger(ready);
        bigIntResult._isPositive = borrow >= 0;
        return bigIntResult;
    }


    private BigInteger[] MakeTheSameLength(BigInteger first, BigInteger second)
    {
        // Console.WriteLine($"first is {first}, second is {second}");
        var max = Math.Max(first._numbers.Length, second._numbers.Length);
        int[] updatedFirstNumbers = new int[max];
        int[] updatedSecondNumbers = new int[max];
        for (int i = 0; i < first._numbers.Length; i++)
        {
            updatedFirstNumbers[i] = first._numbers[i];
        }

        for (int i = first._numbers.Length; i < max; i++)
        {
            updatedFirstNumbers[i] = 0;
        }
        for (int i = 0; i < second._numbers.Length; i++)
        {
            updatedSecondNumbers[i] = second._numbers[i];
        }

        for (int i = second._numbers.Length; i < max; i++)
        {
            updatedSecondNumbers[i] = 0;
        }
        // Console.WriteLine("FIRST");
        // foreach (var t in first._numbers)
        // {
        //     Console.WriteLine(t);
        // }
        // Console.WriteLine("SECOND");
        // foreach (var t in second._numbers)
        // {
        //     Console.WriteLine(t);
        // }

        first._numbers = updatedFirstNumbers;
        first._isPositive = true;
        second._isPositive = true;
        second._numbers = updatedSecondNumbers;
        // Console.WriteLine("FIRST");
        // foreach (var t in first._numbers)
        // {
        //     Console.WriteLine(t);
        // }
        // Console.WriteLine("SECOND");
        // foreach (var t in second._numbers)
        // {
        //     Console.WriteLine(t);
        // }
        // Console.WriteLine($"updated first is {first}, updated second is {second}, max is {max}");
        return new[] { first, second };
    }

    private bool NumberIsZero(BigInteger number)
    {
        var result = true;
        foreach (var digit in number._numbers)
        {
            if (digit != 0)
            {
                result = false;
            }
        }

        return result;
    }

    private BigInteger MultiplyBy10InPower(BigInteger number, int power)
    {
        if (NumberIsZero(number))
        {
            return new BigInteger("0");
        }
        var updatedNumbers = new int[number._numbers.Length + power];
        for (int i = 0; i < power; i++)
        {
            updatedNumbers[i] = 0;
        }

        for (int i = power; i < updatedNumbers.Length; i++)
        {
            updatedNumbers[i] = number._numbers[i - power];
        }

        number._numbers = updatedNumbers;
        return number;
    }

    public BigInteger Karatsuba(BigInteger another)
    {
        bool sign = !((this._isPositive && !another._isPositive) || (!this._isPositive && another._isPositive));
        // 0. make numbers the same length and remove signs
        var sameLength = MakeTheSameLength(this, another);
        var first = sameLength[0];
        var second= sameLength[1];
            
        // check sign
       
        
        var length = first._numbers.Length;
        
        if (length == 1)
        {
            var firstInt = first._numbers[0];
            var secondInt = second._numbers[0];
            return new BigInteger((firstInt * secondInt).ToString());
        }

        if (NumberIsZero(first) || (NumberIsZero(second)))
        {
            return new BigInteger("0");
        }
        // choose base, calculate coefficients
        int m = length / 2;
        var x1 = new BigInteger(first._numbers[m..], true);
        var x0 = new BigInteger(first._numbers[0..m], true);
        var y1 = new BigInteger(second._numbers[m..], true);
        var y0 = new BigInteger(second._numbers[0..m], true);
        // Console.WriteLine($"x1 is {x1}, x0 is {x0}, y1 is {y1}, y0 is {y0}, m is {m}, length is {length} " +
                          // $"first number is {first}, second number is {second}");
        // Console.WriteLine("------------------------------------");
        var z2 = x1.Karatsuba(y1);
        var z0 = x0.Karatsuba(y0);
 
        var z1 = ((x1 + x0).Karatsuba(y1 + y0) - z2) - z0;

        var result = MultiplyBy10InPower(z2, 2*m) + MultiplyBy10InPower(z1, m) + z0;
        result._isPositive = sign;

        return result;
    }




    public static BigInteger operator +(BigInteger a, BigInteger b) => a.Add(b);
    public static BigInteger operator -(BigInteger a, BigInteger b) => a.Sub(b);
    public static BigInteger operator *(BigInteger a, BigInteger b) => a.Karatsuba(b);
}


