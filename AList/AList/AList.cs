using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace AList
{
    public class AList<T> where T: IComparable
    {
        private T[] _array;
        private T[] _arrayMax;
        private const int DefaultSize = 32;
        private int _freePos = 0;
        public int Length { get => _freePos; }
        private T Max;
        
        public AList()
        {
            _array = new T[DefaultSize];
            /*if (_array[0] is int || _array[0] is char || _array[0] is Enum)
            {
                throw new Exception();
            }*/
        }
        
        public void Add(T element)
        {
            if (_freePos == _array.Length)
            {
                Expand();
            }
            
            if (element.CompareTo(Max) > 0)
            {
                Max = element;
            }
            
            _array[_freePos] = element;
            _arrayMax[_freePos] = Max;

            _freePos++;
        }
        
        public void AddAt(int pos, T element)
        {
            if (pos < 0 || pos >= Length)
                throw new IndexOutOfRangeException($"Index: {pos}, Size: {Length}");

            if (_freePos == _array.Length)
                Expand();

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
                throw new InvalidOperationException("Trying to remove an element from an empty alist");

            int index = Array.IndexOf(_array, element, 0, Length);

            if (index < 0)
                return;
            
            RemoveAt(index);
            
        }
        
        public void RemoveAt(int pos)
        {
            if (_freePos < pos)
            {
                throw new InvalidOperationException("Trying to remove an element on unknown index");
            }
            
            _freePos--;
            T[] newArray = new T[_array.Length];

            for (int i = 0; i < pos; i++)
                newArray[i] = _array[i];

            for (int i = pos; i < Length; i++)
                newArray[i] = _array[i+1];

            _array = newArray;
        }

        public T SearchMax()
        {
            return Max;
        }

        private void Expand()
        {
            T[] newArray = new T[_array.Length * 2];
            Array.Copy(_array, newArray, _array.Length);
            _array = newArray;
        }

        /*IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) GetEnumerator();
        }
        
        public PeopleEnum GetEnumerator()
        {
            return new PeopleEnum(_people);
        }*/
    }
    
    /*public class PeopleEnum : IEnumerator
    {
        public Person[] _people;

        // Enumerators are positioned before the first element
        // until the first MoveNext() call.
        int position = -1;

        public PeopleEnum(Person[] list)
        {
            _people = list;
        }

        public bool MoveNext()
        {
            position++;
            return (position < _people.Length);
        }

        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public Person Current
        {
            get
            {
                try
                {
                    return _people[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }*/
}