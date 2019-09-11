#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class GeneratedResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new GeneratedResolver();

        GeneratedResolver()
        {

        }

        public global::MessagePack.Formatters.IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly global::MessagePack.Formatters.IMessagePackFormatter<T> formatter;

            static FormatterCache()
            {
                var f = GeneratedResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class GeneratedResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static GeneratedResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(3)
            {
                {typeof(global::System.Collections.Generic.List<global::messagepackserver.SubModel>), 0 },
                {typeof(global::messagepackserver.SubModel), 1 },
                {typeof(global::messagepackserver.TestModel), 2 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.ListFormatter<global::messagepackserver.SubModel>();
                case 1: return new MessagePack.Formatters.messagepackserver.SubModelFormatter();
                case 2: return new MessagePack.Formatters.messagepackserver.TestModelFormatter();
                default: return null;
            }
        }
    }
}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612



#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Formatters.messagepackserver
{
    using System;
    using MessagePack;


    public sealed class SubModelFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::messagepackserver.SubModel>
    {

        public int Serialize(ref byte[] bytes, int offset, global::messagepackserver.SubModel value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Id);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Name, formatterResolver);
            return offset - startOffset;
        }

        public global::messagepackserver.SubModel Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Id__ = default(int);
            var __Name__ = default(string);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Id__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::messagepackserver.SubModel(__Id__, __Name__);
            return ____result;
        }
    }


    public sealed class TestModelFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::messagepackserver.TestModel>
    {

        public int Serialize(ref byte[] bytes, int offset, global::messagepackserver.TestModel value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 4);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Id);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Name, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::messagepackserver.SubModel>().Serialize(ref bytes, offset, value.SubModel, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::messagepackserver.SubModel>>().Serialize(ref bytes, offset, value.SubModels, formatterResolver);
            return offset - startOffset;
        }

        public global::messagepackserver.TestModel Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Id__ = default(int);
            var __Name__ = default(string);
            var __SubModel__ = default(global::messagepackserver.SubModel);
            var __SubModels__ = default(global::System.Collections.Generic.List<global::messagepackserver.SubModel>);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Id__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __SubModel__ = formatterResolver.GetFormatterWithVerify<global::messagepackserver.SubModel>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __SubModels__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.List<global::messagepackserver.SubModel>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::messagepackserver.TestModel(__Id__, __Name__, __SubModel__, __SubModels__);
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612
