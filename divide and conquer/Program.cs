using System.Diagnostics;
using Microsoft.VisualBasic.CompilerServices;

BigInteger x = new BigInteger("1313234242425");
Console.WriteLine(x);
BigInteger y = new BigInteger("23456789") + new BigInteger("987654321");
Console.WriteLine(y);
BigInteger z = new BigInteger("87654321") - new BigInteger("12345678");
Console.WriteLine(z);
BigInteger d = new BigInteger("-534759348543543734534984739").Multiply(new BigInteger("-90782868767967969796799878789789"));
Console.WriteLine($"d is {d}");


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

        // if ((this.ToString() == "0") && (another.ToString() == "0"))
        // {
        //     BigInteger bigIntResult0 = new BigInteger(string.Join("", result.Reverse()));
        //     return bigIntResult0;
        // }
        //string ToCheck = string.Join("", result.Reverse()).TrimStart('0');
        string ready = string.Join("", result.Reverse()).TrimStart('0');
        BigInteger bigIntResult = new  BigInteger("0");
        if (ready.Length>0) bigIntResult= new BigInteger(ready);
        bigIntResult._isPositive = borrow >= 0;
        return bigIntResult;
    }


    private string[] MakeStrings(BigInteger first, BigInteger second)
    {
        var firstStr = first.ToString();
        var secondStr = second.ToString();
        if (!first._isPositive)
        {
            firstStr = firstStr[1..];
        }

        if (!second._isPositive)
        {
            secondStr = secondStr[1..];
        }

        var result = new string[2];

        result[0] = firstStr;
        result[1] = secondStr;
        return result;
    }

    public BigInteger Multiply(BigInteger another)
    {
        // 1. Take two numbers, omit minuses, make them strings
        var strNumbers = MakeStrings(this, another);
        //   2. Use karatsuba algorithm for those numbers (m, n)
        var absoluteResult = Karatsuba(strNumbers[0], strNumbers[1]);
        // 3. return correct number with sign
        var result = new BigInteger(absoluteResult);
        if ((this._isPositive && !another._isPositive) || (!this._isPositive && another._isPositive))
        {
            result._isPositive = false;
        }
        return result;
    }

    private string[] MakeTheSameLength(string first, string second)
    {
        var max = Math.Max(first.Length, second.Length);
        for (int i = first.Length; i < max; i++)
        {
            first = "0" + first;
        }
        for (int j = second.Length; j < max; j++)
        {
            second = "0" + second;
        }

        return new[] { first, second };
    }

    private string MultiplyBy10InPower(string number, int power)
    {
        if (number.TrimStart('0') == "")
        {
            return "0";
        }
        for (int i = 0; i < power; i++)
        {
            number += "0";
        }

        return number;
    }
    private string AddEverythingUp(string ac, string bd, string aPlusBCPlusD, int length)
    {
        BigInteger acTenPowerN = new BigInteger(MultiplyBy10InPower(ac, length));
        var aplusBCplusDminusACminusBD = new BigInteger(aPlusBCPlusD) - new BigInteger(ac) - new BigInteger(bd);
        BigInteger TenPowerN2 = new BigInteger(MultiplyBy10InPower(aplusBCplusDminusACminusBD.ToString(), length / 2));
        var a = bd;
        BigInteger result  = acTenPowerN + TenPowerN2 + new BigInteger(bd);
        return result.ToString();
    }

private string Karatsuba(string first, string second)
{
    if ((first.TrimStart('0') == "") || second.TrimStart('0') == "")
    {
        return "0";
    }
    // 0. make numbers the same length
        var sameLength = MakeTheSameLength(first, second);
        first = sameLength[0];
        second = sameLength[1];
        
        var length = first.Length;
        // 1. If length of the numbers is 1 - multiply it the usual way and return result
        if (length == 1)
        {
            int numericResult = int.Parse(first) * int.Parse(second);
            var result = numericResult.ToString();
            return result;
        }

        if (length % 2 == 1)
        {
            first = "0" + first;
            second = "0" + second;
            length += 1;
        }

        var a = first[0..(length/2)];
        var b = first[(length / 2).. (length)];
        var c = second[0..(length / 2)];
        var d = second[(length/ 2).. (length)];
        var aBigInteger = new BigInteger(a);
        var bBigInteger = new BigInteger(b);
        var cBigInteger = new BigInteger(c);
        var dBigInteger = new BigInteger(d);
        var ac = Karatsuba(a, c);
        var bd = Karatsuba(b, d);
        var aPlusBCPlusD = Karatsuba((aBigInteger+bBigInteger).ToString(), (cBigInteger + dBigInteger).ToString());
        // Console.WriteLine($"{ac}, {bd}, {aPlusBCPlusD}");
        string finalResult = AddEverythingUp(ac, bd, aPlusBCPlusD, length);
        return finalResult;

    }




    public static BigInteger operator +(BigInteger a, BigInteger b) => a.Add(b);
    public static BigInteger operator -(BigInteger a, BigInteger b) => a.Sub(b);
}


/*
 0. make numbers the same length
1. If length of the numbers is 1 - multiply it the usual way and return result
(2. If the length of the numbers is an odd number - add zero in the beginning of each number)
3. a = m[0, floor(length(m)/2)-1], b = m[floor(length(m)/2), length(m)-1] 
4. c = n[0, floor(length(n)/2)-1], d = n[floor(length(n)/2), length(n)-1] 
5. x = karatsuba (a+b)*(c+d) (recursive)
6. y = karatsuba (a*c) (recursive)
7. z = karatsuba (b*d) (recursive)
8. return (10^length(m) * y) + (10^floor(length(m)/2) * (x-y-z)) + z (not this, lol)*/
