using System;
using System.Collections;

namespace AList
{
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
                
                throw new IndexOutOfRangeException
                    ("Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')");
            }
        }
    }
}