﻿#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class CoreModelResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new CoreModelResolver();

        CoreModelResolver()
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
                var f = CoreModelResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class CoreModelResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static CoreModelResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(13)
            {
                {typeof(global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.BetOptionOdds>), 0 },
                {typeof(global::LiveScore.Core.Enumerations.Enumeration), 1 },
                {typeof(global::LiveScore.Core.Enumerations.EventType), 2 },
                {typeof(global::LiveScore.Core.Enumerations.MatchStatus), 3 },
                {typeof(global::LiveScore.Core.Enumerations.OddsTrend), 4 },
                {typeof(global::LiveScore.Core.Enumerations.PeriodType), 5 },
                {typeof(global::LiveScore.Core.Models.Matches.MatchPeriod), 6 },
                {typeof(global::LiveScore.Core.Models.Matches.Venue), 7 },
                {typeof(global::LiveScore.Core.Models.Odds.BetOptionOdds), 8 },
                {typeof(global::LiveScore.Core.Models.Odds.Bookmaker), 9 },
                {typeof(global::LiveScore.Core.Models.Odds.BetTypeOdds), 10 },
                {typeof(global::LiveScore.Core.Models.Odds.OddsMovement), 11 },
                {typeof(global::LiveScore.Core.Models.Teams.Player), 12 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.InterfaceEnumerableFormatter<global::LiveScore.Core.Models.Odds.BetOptionOdds>();
                case 1: return new MessagePack.Formatters.LiveScore.Core.Enumerations.EnumerationFormatter();
                case 2: return new MessagePack.Formatters.LiveScore.Core.Enumerations.EventTypeFormatter();
                case 3: return new MessagePack.Formatters.LiveScore.Core.Enumerations.MatchStatusFormatter();
                case 4: return new MessagePack.Formatters.LiveScore.Core.Enumerations.OddsTrendFormatter();
                case 5: return new MessagePack.Formatters.LiveScore.Core.Enumerations.PeriodTypeFormatter();
                case 6: return new MessagePack.Formatters.LiveScore.Core.Models.Matches.MatchPeriodFormatter();
                case 7: return new MessagePack.Formatters.LiveScore.Core.Models.Matches.VenueFormatter();
                case 8: return new MessagePack.Formatters.LiveScore.Core.Models.Odds.BetOptionOddsFormatter();
                case 9: return new MessagePack.Formatters.LiveScore.Core.Models.Odds.BookmakerFormatter();
                case 10: return new MessagePack.Formatters.LiveScore.Core.Models.Odds.BetTypeOddsFormatter();
                case 11: return new MessagePack.Formatters.LiveScore.Core.Models.Odds.OddsMovementFormatter();
                case 12: return new MessagePack.Formatters.LiveScore.Core.Models.Teams.PlayerFormatter();
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

namespace MessagePack.Formatters.LiveScore.Core.Enumerations
{
    using System;
    using System.Collections.Generic;
    using MessagePack;

    public sealed class EnumerationFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Enumerations.Enumeration>
    {
        readonly Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>> typeToKeyAndJumpMap;
        readonly Dictionary<int, int> keyToJumpMap;

        public EnumerationFormatter()
        {
            this.typeToKeyAndJumpMap = new Dictionary<RuntimeTypeHandle, KeyValuePair<int, int>>(5, global::MessagePack.Internal.RuntimeTypeHandleEqualityComparer.Default)
            {
                { typeof(global::LiveScore.Core.Enumerations.EventType).TypeHandle, new KeyValuePair<int, int>(0, 0) },
                { typeof(global::LiveScore.Core.Enumerations.Language).TypeHandle, new KeyValuePair<int, int>(1, 1) },
                { typeof(global::LiveScore.Core.Enumerations.LeagueRoundType).TypeHandle, new KeyValuePair<int, int>(2, 2) },
                { typeof(global::LiveScore.Core.Enumerations.PeriodType).TypeHandle, new KeyValuePair<int, int>(3, 3) },
                { typeof(global::LiveScore.Core.Enumerations.MatchStatus).TypeHandle, new KeyValuePair<int, int>(4, 4) },
            };
            this.keyToJumpMap = new Dictionary<int, int>(5)
            {
                { 0, 0 },
                { 1, 1 },
                { 2, 2 },
                { 3, 3 },
                { 4, 4 },
            };
        }

        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Enumerations.Enumeration value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            KeyValuePair<int, int> keyValuePair;
            if (value != null && this.typeToKeyAndJumpMap.TryGetValue(value.GetType().TypeHandle, out keyValuePair))
            {
                var startOffset = offset;
                offset += MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
                offset += MessagePackBinary.WriteInt32(ref bytes, offset, keyValuePair.Key);
                switch (keyValuePair.Value)
                {
                    case 0:
                        offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.EventType>().Serialize(ref bytes, offset, (global::LiveScore.Core.Enumerations.EventType)value, formatterResolver);
                        break;
                    case 1:
                        offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.Language>().Serialize(ref bytes, offset, (global::LiveScore.Core.Enumerations.Language)value, formatterResolver);
                        break;
                    case 2:
                        offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.LeagueRoundType>().Serialize(ref bytes, offset, (global::LiveScore.Core.Enumerations.LeagueRoundType)value, formatterResolver);
                        break;
                    case 3:
                        offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.PeriodType>().Serialize(ref bytes, offset, (global::LiveScore.Core.Enumerations.PeriodType)value, formatterResolver);
                        break;
                    case 4:
                        offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.MatchStatus>().Serialize(ref bytes, offset, (global::LiveScore.Core.Enumerations.MatchStatus)value, formatterResolver);
                        break;
                    default:
                        break;
                }

                return offset - startOffset;
            }

            return MessagePackBinary.WriteNil(ref bytes, offset);
        }
        
        public global::LiveScore.Core.Enumerations.Enumeration Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            
            if (MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize) != 2)
            {
                throw new InvalidOperationException("Invalid Union data was detected. Type:global::LiveScore.Core.Enumerations.Enumeration");
            }
            offset += readSize;

            var key = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
            offset += readSize;

            if (!this.keyToJumpMap.TryGetValue(key, out key))
            {
                key = -1;
            }

            global::LiveScore.Core.Enumerations.Enumeration result = null;
            switch (key)
            {
                case 0:
                    result = (global::LiveScore.Core.Enumerations.Enumeration)formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.EventType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                    offset += readSize;
                    break;
                case 1:
                    result = (global::LiveScore.Core.Enumerations.Enumeration)formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.Language>().Deserialize(bytes, offset, formatterResolver, out readSize);
                    offset += readSize;
                    break;
                case 2:
                    result = (global::LiveScore.Core.Enumerations.Enumeration)formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.LeagueRoundType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                    offset += readSize;
                    break;
                case 3:
                    result = (global::LiveScore.Core.Enumerations.Enumeration)formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.PeriodType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                    offset += readSize;
                    break;
                case 4:
                    result = (global::LiveScore.Core.Enumerations.Enumeration)formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.MatchStatus>().Deserialize(bytes, offset, formatterResolver, out readSize);
                    offset += readSize;
                    break;
                default:
                    offset += MessagePackBinary.ReadNextBlock(bytes, offset);
                    break;
            }
            
            readSize = offset - startOffset;
            
            return result;
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

namespace MessagePack.Formatters.LiveScore.Core.Enumerations
{
    using System;
    using MessagePack;


    public sealed class EventTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Enumerations.EventType>
    {

        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Enumerations.EventType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.DisplayName, formatterResolver);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.Value);
            return offset - startOffset;
        }

        public global::LiveScore.Core.Enumerations.EventType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __DisplayName__ = default(string);
            var __Value__ = default(byte);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __DisplayName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Value__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Core.Enumerations.EventType();
            ____result.DisplayName = __DisplayName__;
            ____result.Value = __Value__;
            return ____result;
        }
    }


    public sealed class MatchStatusFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Enumerations.MatchStatus>
    {

        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Enumerations.MatchStatus value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.DisplayName, formatterResolver);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.Value);
            return offset - startOffset;
        }

        public global::LiveScore.Core.Enumerations.MatchStatus Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __DisplayName__ = default(string);
            var __Value__ = default(byte);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __DisplayName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Value__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Core.Enumerations.MatchStatus();
            ____result.DisplayName = __DisplayName__;
            ____result.Value = __Value__;
            return ____result;
        }
    }


    public sealed class OddsTrendFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Enumerations.OddsTrend>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OddsTrendFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "DisplayName", 0},
                { "Value", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("DisplayName"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Value"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Enumerations.OddsTrend value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.DisplayName, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.Value);
            return offset - startOffset;
        }

        public global::LiveScore.Core.Enumerations.OddsTrend Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __DisplayName__ = default(string);
            var __Value__ = default(byte);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __DisplayName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Value__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Core.Enumerations.OddsTrend();
            ____result.DisplayName = __DisplayName__;
            ____result.Value = __Value__;
            return ____result;
        }
    }


    public sealed class PeriodTypeFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Enumerations.PeriodType>
    {

        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Enumerations.PeriodType value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 2);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.DisplayName, formatterResolver);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.Value);
            return offset - startOffset;
        }

        public global::LiveScore.Core.Enumerations.PeriodType Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __DisplayName__ = default(string);
            var __Value__ = default(byte);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __DisplayName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Value__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Core.Enumerations.PeriodType();
            ____result.DisplayName = __DisplayName__;
            ____result.Value = __Value__;
            return ____result;
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

namespace MessagePack.Formatters.LiveScore.Core.Models.Matches
{
    using System;
    using MessagePack;


    public sealed class MatchPeriodFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Models.Matches.MatchPeriod>
    {

        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Models.Matches.MatchPeriod value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 4);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.HomeScore);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.AwayScore);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.PeriodType>().Serialize(ref bytes, offset, value.PeriodType, formatterResolver);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Number);
            return offset - startOffset;
        }

        public global::LiveScore.Core.Models.Matches.MatchPeriod Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __HomeScore__ = default(int);
            var __AwayScore__ = default(int);
            var __PeriodType__ = default(global::LiveScore.Core.Enumerations.PeriodType);
            var __Number__ = default(int);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __HomeScore__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 1:
                        __AwayScore__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 2:
                        __PeriodType__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.PeriodType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __Number__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Core.Models.Matches.MatchPeriod();
            ____result.HomeScore = __HomeScore__;
            ____result.AwayScore = __AwayScore__;
            ____result.PeriodType = __PeriodType__;
            ____result.Number = __Number__;
            return ____result;
        }
    }


    public sealed class VenueFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Models.Matches.Venue>
    {

        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Models.Matches.Venue value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 5);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Id, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Name, formatterResolver);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Capacity);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.CityName, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.CountryName, formatterResolver);
            return offset - startOffset;
        }

        public global::LiveScore.Core.Models.Matches.Venue Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Id__ = default(string);
            var __Name__ = default(string);
            var __Capacity__ = default(int);
            var __CityName__ = default(string);
            var __CountryName__ = default(string);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Id__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __Capacity__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 3:
                        __CityName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __CountryName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Core.Models.Matches.Venue();
            ____result.Id = __Id__;
            ____result.Name = __Name__;
            ____result.Capacity = __Capacity__;
            ____result.CityName = __CityName__;
            ____result.CountryName = __CountryName__;
            return ____result;
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

namespace MessagePack.Formatters.LiveScore.Core.Models.Odds
{
    using System;
    using MessagePack;


    public sealed class BetOptionOddsFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Models.Odds.BetOptionOdds>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BetOptionOddsFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "Type", 0},
                { "LiveOdds", 1},
                { "OpeningOdds", 2},
                { "OptionValue", 3},
                { "OpeningOptionValue", 4},
                { "OddsTrend", 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Type"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("LiveOdds"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("OpeningOdds"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("OptionValue"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("OpeningOptionValue"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("OddsTrend"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Models.Odds.BetOptionOdds value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 6);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Type, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<decimal>().Serialize(ref bytes, offset, value.LiveOdds, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<decimal>().Serialize(ref bytes, offset, value.OpeningOdds, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.OptionValue, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.OpeningOptionValue, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.OddsTrend>().Serialize(ref bytes, offset, value.OddsTrend, formatterResolver);
            return offset - startOffset;
        }

        public global::LiveScore.Core.Models.Odds.BetOptionOdds Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Type__ = default(string);
            var __LiveOdds__ = default(decimal);
            var __OpeningOdds__ = default(decimal);
            var __OptionValue__ = default(string);
            var __OpeningOptionValue__ = default(string);
            var __OddsTrend__ = default(global::LiveScore.Core.Enumerations.OddsTrend);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Type__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __LiveOdds__ = formatterResolver.GetFormatterWithVerify<decimal>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __OpeningOdds__ = formatterResolver.GetFormatterWithVerify<decimal>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __OptionValue__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __OpeningOptionValue__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __OddsTrend__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.OddsTrend>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Core.Models.Odds.BetOptionOdds(__Type__, __LiveOdds__, __OpeningOdds__, __OptionValue__, __OpeningOptionValue__, __OddsTrend__);
            return ____result;
        }
    }


    public sealed class BookmakerFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Models.Odds.Bookmaker>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BookmakerFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "Id", 0},
                { "Name", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Id"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Name"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Models.Odds.Bookmaker value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Id, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Name, formatterResolver);
            return offset - startOffset;
        }

        public global::LiveScore.Core.Models.Odds.Bookmaker Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Id__ = default(string);
            var __Name__ = default(string);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Id__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Core.Models.Odds.Bookmaker();
            ____result.Id = __Id__;
            ____result.Name = __Name__;
            return ____result;
        }
    }


    public sealed class BetTypeOddsFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Models.Odds.BetTypeOdds>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public BetTypeOddsFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "Id", 0},
                { "Name", 1},
                { "Bookmaker", 2},
                { "BetOptions", 3},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Id"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Name"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Bookmaker"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("BetOptions"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Models.Odds.BetTypeOdds value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 4);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.Id);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Name, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Odds.Bookmaker>().Serialize(ref bytes, offset, value.Bookmaker, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.BetOptionOdds>>().Serialize(ref bytes, offset, value.BetOptions, formatterResolver);
            return offset - startOffset;
        }

        public global::LiveScore.Core.Models.Odds.BetTypeOdds Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Id__ = default(byte);
            var __Name__ = default(string);
            var __Bookmaker__ = default(global::LiveScore.Core.Models.Odds.Bookmaker);
            var __BetOptions__ = default(global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.BetOptionOdds>);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __Id__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 1:
                        __Name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __Bookmaker__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Odds.Bookmaker>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __BetOptions__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.BetOptionOdds>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Core.Models.Odds.BetTypeOdds();
            ____result.Id = __Id__;
            ____result.Name = __Name__;
            ____result.Bookmaker = __Bookmaker__;
            ____result.BetOptions = __BetOptions__;
            return ____result;
        }
    }


    public sealed class OddsMovementFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Models.Odds.OddsMovement>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public OddsMovementFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "BetOptions", 0},
                { "MatchTime", 1},
                { "UpdateTime", 2},
                { "IsMatchStarted", 3},
                { "HomeScore", 4},
                { "AwayScore", 5},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("BetOptions"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("MatchTime"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("UpdateTime"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("IsMatchStarted"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("HomeScore"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("AwayScore"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Models.Odds.OddsMovement value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 6);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.BetOptionOdds>>().Serialize(ref bytes, offset, value.BetOptions, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.MatchTime, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.DateTimeOffset>().Serialize(ref bytes, offset, value.UpdateTime, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.IsMatchStarted);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.HomeScore);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.AwayScore);
            return offset - startOffset;
        }

        public global::LiveScore.Core.Models.Odds.OddsMovement Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __BetOptions__ = default(global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.BetOptionOdds>);
            var __MatchTime__ = default(string);
            var __UpdateTime__ = default(global::System.DateTimeOffset);
            var __IsMatchStarted__ = default(bool);
            var __HomeScore__ = default(int);
            var __AwayScore__ = default(int);

            for (int i = 0; i < length; i++)
            {
                var stringKey = global::MessagePack.MessagePackBinary.ReadStringSegment(bytes, offset, out readSize);
                offset += readSize;
                int key;
                if (!____keyMapping.TryGetValueSafe(stringKey, out key))
                {
                    readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                    goto NEXT_LOOP;
                }

                switch (key)
                {
                    case 0:
                        __BetOptions__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.BetOptionOdds>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __MatchTime__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __UpdateTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTimeOffset>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __IsMatchStarted__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 4:
                        __HomeScore__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 5:
                        __AwayScore__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Core.Models.Odds.OddsMovement();
            ____result.BetOptions = __BetOptions__;
            ____result.MatchTime = __MatchTime__;
            ____result.UpdateTime = __UpdateTime__;
            ____result.IsMatchStarted = __IsMatchStarted__;
            ____result.HomeScore = __HomeScore__;
            ____result.AwayScore = __AwayScore__;
            return ____result;
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

namespace MessagePack.Formatters.LiveScore.Core.Models.Teams
{
    using System;
    using MessagePack;


    public sealed class PlayerFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Models.Teams.Player>
    {

        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Models.Teams.Player value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 6);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Id, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Name, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Type, formatterResolver);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.JerseyNumber);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Position, formatterResolver);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Order);
            return offset - startOffset;
        }

        public global::LiveScore.Core.Models.Teams.Player Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Id__ = default(string);
            var __Name__ = default(string);
            var __Type__ = default(string);
            var __JerseyNumber__ = default(int);
            var __Position__ = default(string);
            var __Order__ = default(int);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Id__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Name__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __Type__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __JerseyNumber__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    case 4:
                        __Position__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __Order__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Core.Models.Teams.Player();
            ____result.Id = __Id__;
            ____result.Name = __Name__;
            ____result.Type = __Type__;
            ____result.JerseyNumber = __JerseyNumber__;
            ____result.Position = __Position__;
            ____result.Order = __Order__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612