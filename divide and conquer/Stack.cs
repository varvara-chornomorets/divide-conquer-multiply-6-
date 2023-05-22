namespace divide_and_conquer;


public class Stack
{
    private const int Capacity = 50;

    private string[] _array = new string[Capacity];

    private int _pointer;

    public void Push(string value)
    {
        if (_pointer == _array.Length)
        {
            throw new Exception("Stack overflowed");
        }

        _array[_pointer] = value;
        _pointer++;
    }

    public string Pull()
    {
        if (_pointer < 0)
        {
            return null;
        }

        var value = _array[_pointer-1];
        _pointer--;
        return value;
    }

    public string PullCopy()
    {
        if (_pointer < 0)
        {
            return null;
        }

        var value = _array[_pointer-1];
        return value;
    }

    public int GetLength()
    {
        return _pointer;
    }

    public void Print()
    {
        for (int i = 0; i < _pointer; i++)
        {
            Console.WriteLine(_array[i]);
        }
    }
}