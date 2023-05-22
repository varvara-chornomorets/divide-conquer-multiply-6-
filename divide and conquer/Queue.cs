namespace divide_and_conquer;

public class Queue
{
    private int _pointer = 0;
    private static int _capacity = 50;
    private string[] _array = new string[_capacity];

    public void Enque(string value)
    {
        if (_pointer == _capacity)
        {
            throw new Exception("queue is overloaded");
        }

        _array[_pointer] = value;
        _pointer++;

    }

    public void Deque()
    {
        if (_pointer == 0)
        {
            throw new Exception("queue is empty, but you try to take an element out");
        }
        string result = _array[0];
        _pointer--;
        for (int i = 0; i < _pointer && _pointer < 49; i++ )
        {
            _array[i] = _array[i + 1];
        }
    }

    public void Print()
    {
        foreach (var number in _array)
        {
            Console.WriteLine(number);
        }
    }

    public int GetLength()
    {
        return _pointer;
    }

    public string GetValue(int index)
    {
        return _array[index];
    }

}