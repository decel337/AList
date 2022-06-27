using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AList
{
    [JsonConverter(typeof(JsonConverterFactoryForListOfT))]
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
    
    public int SingleAsync(T elem)
    {
        var chunks = ToChunks(100).ToArray();

        if (chunks.Count() == 0)
        {
            throw new ArgumentOutOfRangeException();
        }

        int index = -1;

        Parallel.For(0, chunks.Length, (i, _) =>
        {
            int indexPredict = SearchNumber(chunks[i], elem);

            if (indexPredict != -1 && index != -1)
                throw new Exception();

            if (indexPredict != -1 && i != 0)
                index = indexPredict + i * chunks[i - 1].Count();
            else if(indexPredict != -1)
                index = indexPredict;
        });
        
        return index;
    }

    private int SearchNumber(List<T> chunk, T searchElem)
    {
        Console.WriteLine("After");

        if (chunk.Count(x => x == searchElem) > 1)
        {
            throw new ArgumentException();
        }
        
        Console.WriteLine("Before");
        return chunk.IndexOf(searchElem);
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

    public class JsonConverterFactoryForListOfT : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.IsGenericType
            && typeToConvert.GetGenericTypeDefinition() == typeof(InterestingList<>);

        public override JsonConverter CreateConverter(
            Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert.IsGenericType &&
                typeToConvert.GetGenericTypeDefinition() == typeof(InterestingList<>));

            Type elementType = typeToConvert.GetGenericArguments()[0];

            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(JsonConverterForListOfT<>)
                    .MakeGenericType(new Type[] { elementType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: null,
                culture: null)!;

            return converter;
        }
    }

    public class JsonConverterForListOfT<T> : JsonConverter<InterestingList<T>> where T: INumber<T>
    {
        public override InterestingList<T> Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException();
            }
            reader.Read();

            InterestingList<T> elements = new();
            do
            {
                reader.Read();
                while (reader.TokenType != JsonTokenType.EndArray)
                {
                    elements.Add(JsonSerializer.Deserialize<T>(ref reader, options)!);

                    reader.Read();
                }

                reader.Read();
            } while (reader.TokenType == JsonTokenType.StartArray);

            return elements;
        }

        public override void Write(
            Utf8JsonWriter writer, InterestingList<T> list, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            var chunks = list.ToChunks(100);

            foreach (var chunk in chunks)
            {
                writer.WriteStartArray();
                foreach (var element in chunk)
                {
                    JsonSerializer.Serialize(writer, element, options);
                }
                writer.WriteEndArray();
            }

            writer.WriteEndArray();
        }
    }
}