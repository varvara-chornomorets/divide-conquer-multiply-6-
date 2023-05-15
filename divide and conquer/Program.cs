using System.Diagnostics;

BigInteger x = new BigInteger("1313234242425");
Console.WriteLine(x);
BigInteger y = new BigInteger("123456789") + new BigInteger("987654321");
Console.WriteLine(y);
BigInteger z = new BigInteger("87654321") - new BigInteger("12345678");
Console.WriteLine(z);
new BigInteger("123").Multiply(new BigInteger("1234"));


public class BigInteger
{
    private int[] _numbers;
    private bool _isPositive = true;

    public BigInteger(string value)
    {
        if (value[0] == '-')
        {
            _isPositive = false;
            value = value.Substring(1);
        }
        
        _numbers = new int[value.Length];
        for (int i = value.Length - 1; i >= 0; i--)
        {
            _numbers[value.Length - 1 - i] = int.Parse(value[i].ToString());
        }
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
            BigInteger number = new BigInteger(this.ToString().TrimStart('-'));
            return another.Sub(number);
        }
        if (_isPositive && !another._isPositive)
        {
            BigInteger number = new BigInteger(another.ToString().TrimStart('-'));
            return this.Sub(number);
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

            BigInteger bigIntResult = new BigInteger(string.Join("", result.Reverse()).TrimStart('0'));
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

        BigInteger bigIntResult = new BigInteger(string.Join("", result.Reverse()).TrimStart('0'));
        bigIntResult._isPositive = borrow >= 0;
        return bigIntResult;
    }

    public void MakeTheSameLenght(BigInteger first, BigInteger second)
    {
        var max = Math.Max(first._numbers.Length, second._numbers.Length);
        for (int i = first._numbers.Length; i < max; i++)
        {
            // but i cannot append the array
            first._numbers.Append(0);
        }
        for (int i = second._numbers.Length; i < max; i++)
        {
            second._numbers.Append(0);
        }
        Console.WriteLine($"first number is {first.ToString()} lenght is {first._numbers.Length}, " +
                          $"second number is {second.ToString()}, lenght is {second._numbers.Length}");
    }

    public BigInteger Multiply(BigInteger another)
    {
        MakeTheSameLenght(this, another);
        /*
        1. Take two numbers, find the longer one and make the other one the same length (by adding zeros to the beginning)
        2. Use karatsuba algorithm for those numbers (m, n):
        1. If length of the numbers is 1 - multiply it the usual way and return result
        2. If the length of the numbers is an odd number - add zero in the beginning of each number
        3. a = m[0, floor(length(m)/2)-1], b = m[floor(length(m)/2), length(m)-1] 
        4. c = n[0, floor(length(n)/2)-1], d = n[floor(length(n)/2), length(n)-1] 
        5. x = karatsuba (a+b)*(c+d) (recursive)
        6. y = karatsuba (a*c) (recursive)
        7. z = karatsuba (b*d) (recursive)
        8. return (10^length(m) * y) + (10^floor(length(m)/2) * (x-y-z)) + z*/
        return new BigInteger("123");
    }




    public static BigInteger operator +(BigInteger a, BigInteger b) => a.Add(b);
    public static BigInteger operator -(BigInteger a, BigInteger b) => a.Sub(b);
}


