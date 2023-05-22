namespace divide_and_conquer;


public class ArrayList
{
    private string[] _array = new string[10];
    private int _pointer = 0;

    public void Add(string value)
    {
        _array[_pointer] = value;
        _pointer++;
        if (_pointer == _array.Length)
        {
            string[] extendedArray = new string[_array.Length * 2];
            for (int i = 0; i < _array.Length; i++)
            {
                extendedArray[i] = _array[i];
            }

            _array = extendedArray;
        }
    }

    public int GetLenght()
    {
        return _pointer;
    }

    public void Remove(string value)
    {
        for (int i = 0; i < _array.Length; i++)
        {
            if (_array[i] == value)
            {
                for (int k = i; k < _array.Length - 1; k++)
                {
                    _array[k] = _array[k + 1];
                }

                _pointer--;
                break;
            }
        }

        {

        }
    }

    public string GetElement(int index)
    {
        return _array[index];
    }
    
    public void Print()
    {
        foreach (var value in _array)
        {
            Console.WriteLine(value);
        }
    }

}