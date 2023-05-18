using System.Diagnostics;
using Microsoft.VisualBasic.CompilerServices;

// BigInteger x = new BigInteger("1313234242425");
// Console.WriteLine(x);
// BigInteger y = new BigInteger("23456789") + new BigInteger("987654321");
// Console.WriteLine(y);
// BigInteger z = new BigInteger("87654321") - new BigInteger("12345678");
// Console.WriteLine(z);
BigInteger d = new BigInteger("123").Karatsuba(new BigInteger("12345"));
// Console.WriteLine($"d is {d}");


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


    private BigInteger[] MakeTheSameLength(BigInteger first, BigInteger second)
    {
        Console.WriteLine($"first is {first}, second is {second}");
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

    public BigInteger Karatsuba(BigInteger another)
    {
        // 0. make numbers the same length
        var sameLength = MakeTheSameLength(this, another);
        var first = sameLength[0];
        var second= sameLength[1];
            
        // check sign
        bool sign = !((this._isPositive && !another._isPositive) || (!this._isPositive && another._isPositive));
        
        var length = first._numbers.Length;
        if (length == 1)
        {
            var firstInt = first._numbers[0];
            var secondInt = second._numbers[0];
            return new BigInteger(new[] { firstInt + secondInt }, sign);
        }
        
        // if (length % 2 == 1)
        // {
        //     first = "0" + first;
        //     second = "0" + second;
        //     length += 1;
        // }
        //
        // var a = first[0..(length/2)];
        // var b = first[(length / 2).. (length)];
        // var c = second[0..(length / 2)];
        // var d = second[(length/ 2).. (length)];
        // var aBigInteger = new BigInteger(a);
        // var bBigInteger = new BigInteger(b);
        // var cBigInteger = new BigInteger(c);
        // var dBigInteger = new BigInteger(d);
        // var ac = Karatsuba(a, c);
        // var bd = Karatsuba(b, d);
        // var aPlusBCPlusD = Karatsuba((aBigInteger+bBigInteger).ToString(), (cBigInteger + dBigInteger).ToString());
        // // Console.WriteLine($"{ac}, {bd}, {aPlusBCPlusD}");
        // string finalResult = AddEverythingUp(ac, bd, aPlusBCPlusD, length);
        // return finalResult;
        return new BigInteger("4");

    }




    public static BigInteger operator +(BigInteger a, BigInteger b) => a.Add(b);
    public static BigInteger operator -(BigInteger a, BigInteger b) => a.Sub(b);
}


