using System;
using System.Numerics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AList
{
    public class JsonConverterFactoryForListOfT : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
            => typeToConvert.IsGenericType
            && typeToConvert.GetGenericTypeDefinition() == typeof(InterestingList<>);

        public override JsonConverter CreateConverter(
            Type typeToConvert, JsonSerializerOptions options)
        {
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