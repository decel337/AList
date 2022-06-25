using System;
using System.Collections;
using System.Numerics;

namespace AList
{
    public class InterestingList<T> where T: INumber<T>

    {
    public T[] _array;
    public IEnumerator GetEnumerator() => new InterestingListEnum<T>(_array, Length);
    private const int DefaultSize = 32;
    private int _freePos = 0;
    private T _max;

    public int Length { get => _freePos; }

    public InterestingList()
    {
        _array = new T[DefaultSize];
    }

    public void Add(T element)
    {
        if (_freePos == _array.Length)
            Expand();

        _max = MaxValue(element, _max);

        _array[_freePos] = element;
        _freePos++;
    }

    public void AddAt(int pos, T element)
    {
        if (pos < 0 || pos >= Length)
            throw new IndexOutOfRangeException($"Index: {pos}, Size: {Length}");

        if (_freePos == _array.Length)
            Expand();
        
        _max = MaxValue(element, _max);
        
        _freePos++;
        T[] newArray = new T[_array.Length];
        newArray[pos] = element;

        for (int i = 0; i < pos; i++)
            newArray[i] = _array[i];

        for (int i = pos; i < Length; i++)
            newArray[i + 1] = _array[i];

        _array = newArray;
    }

    public void Remove(T element)
    {
        if (_freePos == 0)
            throw new InvalidOperationException("Trying to remove an element from an empty interesting list");

        int index = Array.IndexOf(_array, element, 0, Length);

        if (index < 0)
            return;

        RemoveAt(index);
    }

    public void RemoveAt(int pos)
    {
        if (_freePos <= pos)
            throw new InvalidOperationException("Trying to remove an element on unknown index");

        _freePos--;
        T[] newArray = new T[_array.Length];

        for (int i = 0; i < pos; i++)
            newArray[i] = _array[i];

        for (int i = pos; i < Length; i++)
            newArray[i] = _array[i + 1];

        _array = newArray;
        SearchMax();
    }

    private void SearchMax()
    {
        if (_freePos == 0)
            return;

        _max = MinValue(_array[0], _max);

        for (int i = 1; i < Length; i++)
            _max = MaxValue(_array[i], _max);
    }

    private void Expand()
    {
        T[] newArray = new T[_array.Length * 2];
        Array.Copy(_array, newArray, _array.Length);
        _array = newArray;
    }

    private T MinValue(T element1, T element2) => element1 < element2 ? element1 : element2;
    private T MaxValue(T element1, T element2) => element1 > element2 ? element1 : element2;

    public T Max()
    {
        if (_freePos == 0)
            throw new InvalidOperationException("Sequence contains no elements");

        return _max;
    }
    
    public T this[int index]
    {
        get
        {
            if (index >= 0 && index < Length)
                return _array[index];

            throw new ArgumentOutOfRangeException
                ("Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')");
        }
        set
        { 
            if (index >= 0 && index < Length)
                _array[index] = value;
            
            throw new ArgumentOutOfRangeException
                ("Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')");
        } 
    }
    }

    public class InterestingListEnum<T> : IEnumerator
    {
        private readonly T[] _array;
        private readonly int _length;
        
        private int _position = -1;

        public InterestingListEnum(T[] array, int length)
        {
            _array = array;
            _length = length;
        }

        public bool MoveNext()
        {
            _position++;
            return (_position < _length);
        }

        public void Reset() => _position = -1;


        object IEnumerator.Current { get => Current; }

        public T Current
        {
get
            {
                if (_position != -1 && _position < _length)
                    return _array[_position];
                
                throw new ArgumentOutOfRangeException
                    ("Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')");
            }
        }
    }
}