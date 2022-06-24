using System;
using System.Collections;

namespace AList
{
    public class AList<T>
    {
        private T[] array;
        private const int defaultSize = 32;
        private int freePos = 0;
        public int Length { get => freePos; }
        
        public AList()
        {
            array = new T[defaultSize];
        }
        
        public void Add(T element)
        {
            if (freePos == array.Length)
            {
                Expand();
            }
            array[freePos] = element;
            freePos++;
        }
        
        public void AddAt(int pos, T element)
        {
            if (freePos == array.Length)
            {
                Expand();
            }
            
            freePos++;
            T[] newArray = new T[array.Length];
            newArray[pos] = element;
            
            for (int i = 0; i < pos; i++)
                newArray[i] = array[i];

            for (int i = pos; i < Length; i++)
                newArray[i + 1] = array[i];

            array = newArray;
        }
        
        private void Expand()
        {
            T[] newArray = new T[array.Length * 2];
            Array.Copy(array, newArray, array.Length);
            array = newArray;
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