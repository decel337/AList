using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AList;

namespace AList
{
    [JsonConverter(typeof(JsonConverterFactoryForListOfT))]
    public class InterestingList<T> : IEnumerable where T: INumber<T>
    {
    private T[] _array;
    public IEnumerator GetEnumerator() => new InterestingListEnum<T>(_array, Length);
    private const int DefaultSize = 32;
    private T _max;
    private int _freePos = 0;
    [Format("max")]
    public T Max {
        get
        {
            if (_freePos == 0)
                throw new InvalidOperationException("Sequence contains no elements");
            return _max;
        }
        set
        {
            _max = value;
        }}
    [Format("simple")]
    public int simple { get; set; }
    
    [Format("length")]
    public int Length { get => _freePos; }

    public InterestingList()
    {
        _array = new T[DefaultSize];
        simple = 100;
    }

    public void Add(T element)
    {
        if (_freePos == _array.Length)
            Expand();

        if (_freePos == 0)
        {
            _max = element;
        }
        
        _max = MaxValue(element, _max);

        _array[_freePos] = element;
        _freePos++;
    }

    public void AddAt(int pos, T element)
    {
        if (pos < 0 || pos >= Length)
            throw new IndexOutOfRangeException("Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')" +
                                               $"Index: {pos}, Size: {Length}");

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
        if (pos < 0 || pos >= Length)
            throw new IndexOutOfRangeException($"Trying to remove an element on unknown index. Index: {pos}, Size: {Length}");

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

    public T this[int index]
    {
        get
        {
            if (index >= 0 && index < Length)
                return _array[index];

            throw new IndexOutOfRangeException
                ("Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')" +
                 $"Index: {index}, Size: {Length}");
        }
        set
        { 
            if (index >= 0 && index < Length)
                _array[index] = value;
            
            throw new IndexOutOfRangeException
                ("Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')" +
                 $"Index: {index}, Size: {Length}");
        } 
    }

    public int SingleAsync(T elem)
    {
        var chunks = ToChunks(100).ToArray();

        if (!chunks.Any())
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }

        int index = -1;

        Parallel.For(0, chunks.Length, (i, _) =>
        {
            int indexPredict = SearchNumber(chunks[i], elem);

            if (indexPredict != -1 && index != -1)
                throw new InvalidOperationException("Sequence contains more than one element");

            if (indexPredict != -1 && i != 0)
                index = indexPredict + i * chunks[i - 1].Count();
            else if(indexPredict != -1)
                index = indexPredict;
        });

        if (index == -1)
        {
            throw new InvalidOperationException("Sequence don't contain any element");
        }
        
        return index;
    }

    public int SingleAsyncWithTask(T elem)
    {
        const int chunkSize = 100;
        var chunks = ToChunks(chunkSize).ToArray();

        if (!chunks.Any())
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }

        List<int> indexes = new();
        
        var tasks = chunks.Select((chunk, index)=>SearchNumberAsync(chunk, elem, indexes, index, chunkSize));

        Task.WhenAll(tasks).Wait();

        if (!indexes.Any())
        {
            throw new InvalidOperationException("Sequence don't contain any element");
        }

        foreach (var index in indexes)
        {
            Console.WriteLine(index);
        }
        
        return indexes.Single();
    }
    
    public async Task<int> SingleAsyncWithTaskAwait(T elem)
    {
        const int chunkSize = 100;
        var chunks = ToChunks(chunkSize).ToArray();

        if (!chunks.Any())
        {
            throw new InvalidOperationException("Sequence contains no elements");
        }

        List<int> indexes = new();
        
        var tasks = chunks.Select((chunk, index)=>SearchNumberAsync(chunk, elem, indexes, index, chunkSize));

        await Task.WhenAll(tasks);

        if (!indexes.Any())
        {
            throw new InvalidOperationException("Sequence don't contain any element");
        }

        foreach (var index in indexes)
        {
            Console.WriteLine(index);
        }
        
        return indexes.Single();
    }

    private int SearchNumber(List<T> chunk, T searchElem)
    {
        if (chunk.Count(x => x == searchElem) > 1)
        {
            throw new InvalidOperationException("Sequence contains more than one element");
        }
        return chunk.IndexOf(searchElem);
    }
    
    private async Task SearchNumberAsync(List<T> chunk, T searchElem, List<int> indexes, int numberchunk, int chunksize)
    {
        if (chunk.Count(x => x == searchElem) > 1)
        {
            throw new InvalidOperationException("Sequence contains more than one element");
        }

        int indexPredict = await Task.Run(() => chunk.IndexOf(searchElem));
        
        if (indexPredict != -1 && indexes.Any())
            throw new InvalidOperationException("Sequence contains more than one element");

        if (indexPredict != -1)
            indexes.Add(indexPredict + numberchunk * chunksize);
    }
    

    public IEnumerable<List<T>> ToChunks(int chunkSize)
    {
        List<T> chunk = new List<T>(chunkSize);
        for (int i = 0; i < Length; i++)
        {
            chunk.Add(_array[i]);
            if (chunk.Count == chunkSize) {
                yield return chunk;
                chunk = new List<T>(chunkSize);
            }
        }

        if (chunk.Any())
            yield return chunk;
    }

    public override string ToString()
    {
        string output = "";
        for (int i = 0; i < Length; i++)
        {
            output += _array[i] + " ";
        }

        return output;
    }
    }
}