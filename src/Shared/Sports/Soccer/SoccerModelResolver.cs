﻿#pragma warning disable 618
#pragma warning disable 612
#pragma warning disable 414
#pragma warning disable 168

namespace MessagePack.Resolvers
{
    using System;
    using MessagePack;

    public class SoccerModelResolver : global::MessagePack.IFormatterResolver
    {
        public static readonly global::MessagePack.IFormatterResolver Instance = new SoccerModelResolver();

        SoccerModelResolver()
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
                var f = SoccerModelResolverGetFormatterHelper.GetFormatter(typeof(T));
                if (f != null)
                {
                    formatter = (global::MessagePack.Formatters.IMessagePackFormatter<T>)f;
                }
            }
        }
    }

    internal static class SoccerModelResolverGetFormatterHelper
    {
        static readonly global::System.Collections.Generic.Dictionary<Type, int> lookup;

        static SoccerModelResolverGetFormatterHelper()
        {
            lookup = new global::System.Collections.Generic.Dictionary<Type, int>(12)
            {
                {typeof(global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Matches.MatchPeriod>), 0 },
                {typeof(global::System.Collections.Generic.IEnumerable<global::LiveScore.Soccer.Models.Matches.Match>), 1 },
                {typeof(global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Matches.TimelineEvent>), 2 },
                {typeof(global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.BetTypeOdds>), 3 },
                {typeof(global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.OddsMovement>), 4 },
                {typeof(global::LiveScore.Soccer.Models.Matches.Match), 5 },
                {typeof(global::LiveScore.Soccer.Models.Matches.MatchList), 6 },
                {typeof(global::LiveScore.Core.Models.Matches.GoalScorer), 7 },
                {typeof(global::LiveScore.Core.Models.Matches.TimelineEvent), 8 },
                {typeof(global::LiveScore.Soccer.Models.Matches.MatchInfo), 9 },
                {typeof(global::LiveScore.Soccer.Models.Odds.MatchOdds), 10 },
                {typeof(global::LiveScore.Soccer.Models.Odds.MatchOddsMovement), 11 },
            };
        }

        internal static object GetFormatter(Type t)
        {
            int key;
            if (!lookup.TryGetValue(t, out key)) return null;

            switch (key)
            {
                case 0: return new global::MessagePack.Formatters.InterfaceEnumerableFormatter<global::LiveScore.Core.Models.Matches.MatchPeriod>();
                case 1: return new global::MessagePack.Formatters.InterfaceEnumerableFormatter<global::LiveScore.Soccer.Models.Matches.Match>();
                case 2: return new global::MessagePack.Formatters.InterfaceEnumerableFormatter<global::LiveScore.Core.Models.Matches.TimelineEvent>();
                case 3: return new global::MessagePack.Formatters.InterfaceEnumerableFormatter<global::LiveScore.Core.Models.Odds.BetTypeOdds>();
                case 4: return new global::MessagePack.Formatters.InterfaceEnumerableFormatter<global::LiveScore.Core.Models.Odds.OddsMovement>();
                case 5: return new MessagePack.Formatters.LiveScore.Soccer.Models.Matches.MatchFormatter();
                case 6: return new MessagePack.Formatters.LiveScore.Soccer.Models.Matches.MatchListFormatter();
                case 7: return new MessagePack.Formatters.LiveScore.Core.Models.Matches.GoalScorerFormatter();
                case 8: return new MessagePack.Formatters.LiveScore.Core.Models.Matches.TimelineEventFormatter();
                case 9: return new MessagePack.Formatters.LiveScore.Soccer.Models.Matches.MatchInfoFormatter();
                case 10: return new MessagePack.Formatters.LiveScore.Soccer.Models.Odds.MatchOddsFormatter();
                case 11: return new MessagePack.Formatters.LiveScore.Soccer.Models.Odds.MatchOddsMovementFormatter();
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

namespace MessagePack.Formatters.LiveScore.Soccer.Models.Matches
{
    using System;
    using MessagePack;


    public sealed class MatchFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Soccer.Models.Matches.Match>
    {

        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Soccer.Models.Matches.Match value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteArrayHeader(ref bytes, offset, 27);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Id, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::System.DateTimeOffset>().Serialize(ref bytes, offset, value.EventDate, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::System.DateTimeOffset>().Serialize(ref bytes, offset, value.CurrentPeriodStartTime, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.LeagueId, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.LeagueName, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.HomeTeamId, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.HomeTeamName, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.AwayTeamId, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.AwayTeamName, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.MatchStatus>().Serialize(ref bytes, offset, value.MatchStatus, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.MatchStatus>().Serialize(ref bytes, offset, value.EventStatus, formatterResolver);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.HomeScore);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.AwayScore);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.WinnerId, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.AggregateWinnerId, formatterResolver);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.AggregateHomeScore);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.AggregateAwayScore);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.HomeRedCards);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.HomeYellowRedCards);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.AwayRedCards);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.AwayYellowRedCards);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.MatchTime);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.StoppageTime, formatterResolver);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.InjuryTimeAnnounced);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.EventType>().Serialize(ref bytes, offset, value.LastTimelineType, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Matches.MatchPeriod>>().Serialize(ref bytes, offset, value.MatchPeriods, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.CountryCode, formatterResolver);
            return offset - startOffset;
        }

        public global::LiveScore.Soccer.Models.Matches.Match Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __EventDate__ = default(global::System.DateTimeOffset);
            var __CurrentPeriodStartTime__ = default(global::System.DateTimeOffset);
            var __LeagueId__ = default(string);
            var __LeagueName__ = default(string);
            var __HomeTeamId__ = default(string);
            var __HomeTeamName__ = default(string);
            var __AwayTeamId__ = default(string);
            var __AwayTeamName__ = default(string);
            var __MatchStatus__ = default(global::LiveScore.Core.Enumerations.MatchStatus);
            var __EventStatus__ = default(global::LiveScore.Core.Enumerations.MatchStatus);
            var __HomeScore__ = default(byte);
            var __AwayScore__ = default(byte);
            var __WinnerId__ = default(string);
            var __AggregateWinnerId__ = default(string);
            var __AggregateHomeScore__ = default(byte);
            var __AggregateAwayScore__ = default(byte);
            var __HomeRedCards__ = default(byte);
            var __HomeYellowRedCards__ = default(byte);
            var __AwayRedCards__ = default(byte);
            var __AwayYellowRedCards__ = default(byte);
            var __MatchTime__ = default(byte);
            var __StoppageTime__ = default(string);
            var __InjuryTimeAnnounced__ = default(byte);
            var __LastTimelineType__ = default(global::LiveScore.Core.Enumerations.EventType);
            var __MatchPeriods__ = default(global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Matches.MatchPeriod>);
            var __CountryCode__ = default(string);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Id__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __EventDate__ = formatterResolver.GetFormatterWithVerify<global::System.DateTimeOffset>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __CurrentPeriodStartTime__ = formatterResolver.GetFormatterWithVerify<global::System.DateTimeOffset>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __LeagueId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __LeagueName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 5:
                        __HomeTeamId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 6:
                        __HomeTeamName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 7:
                        __AwayTeamId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 8:
                        __AwayTeamName__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 9:
                        __MatchStatus__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.MatchStatus>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 10:
                        __EventStatus__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.MatchStatus>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 11:
                        __HomeScore__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 12:
                        __AwayScore__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 13:
                        __WinnerId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 14:
                        __AggregateWinnerId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 15:
                        __AggregateHomeScore__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 16:
                        __AggregateAwayScore__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 17:
                        __HomeRedCards__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 18:
                        __HomeYellowRedCards__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 19:
                        __AwayRedCards__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 20:
                        __AwayYellowRedCards__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 21:
                        __MatchTime__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 22:
                        __StoppageTime__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 23:
                        __InjuryTimeAnnounced__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 24:
                        __LastTimelineType__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.EventType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 25:
                        __MatchPeriods__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Matches.MatchPeriod>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 26:
                        __CountryCode__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Soccer.Models.Matches.Match(__Id__, __EventDate__, __CurrentPeriodStartTime__, __LeagueId__, __LeagueName__, __HomeTeamId__, __HomeTeamName__, __AwayTeamId__, __AwayTeamName__, __MatchStatus__, __EventStatus__, __HomeScore__, __AwayScore__, __WinnerId__, __AggregateWinnerId__, __AggregateHomeScore__, __AggregateAwayScore__, __HomeRedCards__, __HomeYellowRedCards__, __AwayRedCards__, __AwayYellowRedCards__, __MatchTime__, __StoppageTime__, __InjuryTimeAnnounced__, __LastTimelineType__, __MatchPeriods__, __CountryCode__);
            return ____result;
        }
    }


    public sealed class MatchListFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Soccer.Models.Matches.MatchList>
    {

        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Soccer.Models.Matches.MatchList value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 1);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Soccer.Models.Matches.Match>>().Serialize(ref bytes, offset, value.Matches, formatterResolver);
            return offset - startOffset;
        }

        public global::LiveScore.Soccer.Models.Matches.MatchList Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Matches__ = default(global::System.Collections.Generic.IEnumerable<global::LiveScore.Soccer.Models.Matches.Match>);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Matches__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Soccer.Models.Matches.Match>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Soccer.Models.Matches.MatchList();
            ____result.Matches = __Matches__;
            return ____result;
        }
    }


    public sealed class MatchInfoFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Soccer.Models.Matches.MatchInfo>
    {

        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Soccer.Models.Matches.MatchInfo value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 5);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Soccer.Models.Matches.Match>().Serialize(ref bytes, offset, value.Match, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Matches.TimelineEvent>>().Serialize(ref bytes, offset, value.TimelineEvents, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Matches.Venue>().Serialize(ref bytes, offset, value.Venue, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Referee, formatterResolver);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, value.Attendance);
            return offset - startOffset;
        }

        public global::LiveScore.Soccer.Models.Matches.MatchInfo Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadArrayHeader(bytes, offset, out readSize);
            offset += readSize;

            var __Match__ = default(global::LiveScore.Soccer.Models.Matches.Match);
            var __TimelineEvents__ = default(global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Matches.TimelineEvent>);
            var __Venue__ = default(global::LiveScore.Core.Models.Matches.Venue);
            var __Referee__ = default(string);
            var __Attendance__ = default(int);

            for (int i = 0; i < length; i++)
            {
                var key = i;

                switch (key)
                {
                    case 0:
                        __Match__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Soccer.Models.Matches.Match>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __TimelineEvents__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Matches.TimelineEvent>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __Venue__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Matches.Venue>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __Referee__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __Attendance__ = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Soccer.Models.Matches.MatchInfo(__Match__, __TimelineEvents__, __Venue__, __Referee__, __Attendance__);
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


    public sealed class GoalScorerFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Models.Matches.GoalScorer>
    {

        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Models.Matches.GoalScorer value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedArrayHeaderUnsafe(ref bytes, offset, 3);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Id, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Name, formatterResolver);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Method, formatterResolver);
            return offset - startOffset;
        }

        public global::LiveScore.Core.Models.Matches.GoalScorer Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __Method__ = default(string);

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
                        __Method__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Core.Models.Matches.GoalScorer();
            ____result.Id = __Id__;
            ____result.Name = __Name__;
            ____result.Method = __Method__;
            return ____result;
        }
    }


    public sealed class TimelineEventFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Core.Models.Matches.TimelineEvent>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public TimelineEventFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "Id", 0},
                { "Name", 1},
                { "Type", 2},
                { "Time", 3},
                { "MatchTime", 4},
                { "StoppageTime", 5},
                { "MatchClock", 6},
                { "Team", 7},
                { "Period", 8},
                { "PeriodType", 9},
                { "HomeScore", 10},
                { "AwayScore", 11},
                { "GoalScorer", 12},
                { "Assist", 13},
                { "Player", 14},
                { "InjuryTimeAnnounced", 15},
                { "HomeShootoutPlayer", 16},
                { "IsHomeShootoutScored", 17},
                { "AwayShootoutPlayer", 18},
                { "IsAwayShootoutScored", 19},
                { "ShootoutHomeScore", 20},
                { "ShootoutAwayScore", 21},
                { "IsFirstShoot", 22},
                { "IsHome", 23},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Id"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Name"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Type"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Time"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("MatchTime"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("StoppageTime"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("MatchClock"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Team"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Period"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("PeriodType"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("HomeScore"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("AwayScore"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("GoalScorer"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Assist"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Player"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("InjuryTimeAnnounced"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("HomeShootoutPlayer"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("IsHomeShootoutScored"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("AwayShootoutPlayer"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("IsAwayShootoutScored"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("ShootoutHomeScore"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("ShootoutAwayScore"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("IsFirstShoot"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("IsHome"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Core.Models.Matches.TimelineEvent value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteMapHeader(ref bytes, offset, 24);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Id, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Name, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.EventType>().Serialize(ref bytes, offset, value.Type, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[3]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.DateTimeOffset>().Serialize(ref bytes, offset, value.Time, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[4]);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.MatchTime);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[5]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.StoppageTime, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[6]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.MatchClock, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[7]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.Team, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[8]);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.Period);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[9]);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.PeriodType>().Serialize(ref bytes, offset, value.PeriodType, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[10]);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.HomeScore);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[11]);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.AwayScore);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[12]);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Matches.GoalScorer>().Serialize(ref bytes, offset, value.GoalScorer, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[13]);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Teams.Player>().Serialize(ref bytes, offset, value.Assist, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[14]);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Teams.Player>().Serialize(ref bytes, offset, value.Player, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[15]);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.InjuryTimeAnnounced);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[16]);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Teams.Player>().Serialize(ref bytes, offset, value.HomeShootoutPlayer, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[17]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.IsHomeShootoutScored);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[18]);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Teams.Player>().Serialize(ref bytes, offset, value.AwayShootoutPlayer, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[19]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.IsAwayShootoutScored);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[20]);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.ShootoutHomeScore);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[21]);
            offset += MessagePackBinary.WriteByte(ref bytes, offset, value.ShootoutAwayScore);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[22]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.IsFirstShoot);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[23]);
            offset += MessagePackBinary.WriteBoolean(ref bytes, offset, value.IsHome);
            return offset - startOffset;
        }

        public global::LiveScore.Core.Models.Matches.TimelineEvent Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
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
            var __Type__ = default(global::LiveScore.Core.Enumerations.EventType);
            var __Time__ = default(global::System.DateTimeOffset);
            var __MatchTime__ = default(byte);
            var __StoppageTime__ = default(string);
            var __MatchClock__ = default(string);
            var __Team__ = default(string);
            var __Period__ = default(byte);
            var __PeriodType__ = default(global::LiveScore.Core.Enumerations.PeriodType);
            var __HomeScore__ = default(byte);
            var __AwayScore__ = default(byte);
            var __GoalScorer__ = default(global::LiveScore.Core.Models.Matches.GoalScorer);
            var __Assist__ = default(global::LiveScore.Core.Models.Teams.Player);
            var __Player__ = default(global::LiveScore.Core.Models.Teams.Player);
            var __InjuryTimeAnnounced__ = default(byte);
            var __HomeShootoutPlayer__ = default(global::LiveScore.Core.Models.Teams.Player);
            var __IsHomeShootoutScored__ = default(bool);
            var __AwayShootoutPlayer__ = default(global::LiveScore.Core.Models.Teams.Player);
            var __IsAwayShootoutScored__ = default(bool);
            var __ShootoutHomeScore__ = default(byte);
            var __ShootoutAwayScore__ = default(byte);
            var __IsFirstShoot__ = default(bool);
            var __IsHome__ = default(bool);

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
                    case 2:
                        __Type__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.EventType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 3:
                        __Time__ = formatterResolver.GetFormatterWithVerify<global::System.DateTimeOffset>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 4:
                        __MatchTime__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 5:
                        __StoppageTime__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 6:
                        __MatchClock__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 7:
                        __Team__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 8:
                        __Period__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 9:
                        __PeriodType__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Enumerations.PeriodType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 10:
                        __HomeScore__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 11:
                        __AwayScore__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 12:
                        __GoalScorer__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Matches.GoalScorer>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 13:
                        __Assist__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Teams.Player>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 14:
                        __Player__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Teams.Player>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 15:
                        __InjuryTimeAnnounced__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 16:
                        __HomeShootoutPlayer__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Teams.Player>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 17:
                        __IsHomeShootoutScored__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 18:
                        __AwayShootoutPlayer__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Teams.Player>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 19:
                        __IsAwayShootoutScored__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 20:
                        __ShootoutHomeScore__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 21:
                        __ShootoutAwayScore__ = MessagePackBinary.ReadByte(bytes, offset, out readSize);
                        break;
                    case 22:
                        __IsFirstShoot__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    case 23:
                        __IsHome__ = MessagePackBinary.ReadBoolean(bytes, offset, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Core.Models.Matches.TimelineEvent();
            ____result.Id = __Id__;
            ____result.Name = __Name__;
            ____result.Type = __Type__;
            ____result.Time = __Time__;
            ____result.MatchTime = __MatchTime__;
            ____result.StoppageTime = __StoppageTime__;
            ____result.MatchClock = __MatchClock__;
            ____result.Team = __Team__;
            ____result.Period = __Period__;
            ____result.PeriodType = __PeriodType__;
            ____result.HomeScore = __HomeScore__;
            ____result.AwayScore = __AwayScore__;
            ____result.GoalScorer = __GoalScorer__;
            ____result.Assist = __Assist__;
            ____result.Player = __Player__;
            ____result.InjuryTimeAnnounced = __InjuryTimeAnnounced__;
            ____result.HomeShootoutPlayer = __HomeShootoutPlayer__;
            ____result.IsHomeShootoutScored = __IsHomeShootoutScored__;
            ____result.AwayShootoutPlayer = __AwayShootoutPlayer__;
            ____result.IsAwayShootoutScored = __IsAwayShootoutScored__;
            ____result.ShootoutHomeScore = __ShootoutHomeScore__;
            ____result.ShootoutAwayScore = __ShootoutAwayScore__;
            ____result.IsFirstShoot = __IsFirstShoot__;
            ____result.IsHome = __IsHome__;
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

namespace MessagePack.Formatters.LiveScore.Soccer.Models.Odds
{
    using System;
    using MessagePack;


    public sealed class MatchOddsFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Soccer.Models.Odds.MatchOdds>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MatchOddsFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "MatchId", 0},
                { "BetTypeOddsList", 1},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("MatchId"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("BetTypeOddsList"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Soccer.Models.Odds.MatchOdds value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.MatchId, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.BetTypeOdds>>().Serialize(ref bytes, offset, value.BetTypeOddsList, formatterResolver);
            return offset - startOffset;
        }

        public global::LiveScore.Soccer.Models.Odds.MatchOdds Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __MatchId__ = default(string);
            var __BetTypeOddsList__ = default(global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.BetTypeOdds>);

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
                        __MatchId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __BetTypeOddsList__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.BetTypeOdds>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Soccer.Models.Odds.MatchOdds();
            ____result.MatchId = __MatchId__;
            ____result.BetTypeOddsList = __BetTypeOddsList__;
            return ____result;
        }
    }


    public sealed class MatchOddsMovementFormatter : global::MessagePack.Formatters.IMessagePackFormatter<global::LiveScore.Soccer.Models.Odds.MatchOddsMovement>
    {

        readonly global::MessagePack.Internal.AutomataDictionary ____keyMapping;
        readonly byte[][] ____stringByteKeys;

        public MatchOddsMovementFormatter()
        {
            this.____keyMapping = new global::MessagePack.Internal.AutomataDictionary()
            {
                { "MatchId", 0},
                { "Bookmaker", 1},
                { "OddsMovements", 2},
            };

            this.____stringByteKeys = new byte[][]
            {
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("MatchId"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("Bookmaker"),
                global::MessagePack.MessagePackBinary.GetEncodedStringBytes("OddsMovements"),
                
            };
        }


        public int Serialize(ref byte[] bytes, int offset, global::LiveScore.Soccer.Models.Odds.MatchOddsMovement value, global::MessagePack.IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return global::MessagePack.MessagePackBinary.WriteNil(ref bytes, offset);
            }
            
            var startOffset = offset;
            offset += global::MessagePack.MessagePackBinary.WriteFixedMapHeaderUnsafe(ref bytes, offset, 3);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[0]);
            offset += formatterResolver.GetFormatterWithVerify<string>().Serialize(ref bytes, offset, value.MatchId, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[1]);
            offset += formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Odds.Bookmaker>().Serialize(ref bytes, offset, value.Bookmaker, formatterResolver);
            offset += global::MessagePack.MessagePackBinary.WriteRaw(ref bytes, offset, this.____stringByteKeys[2]);
            offset += formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.OddsMovement>>().Serialize(ref bytes, offset, value.OddsMovements, formatterResolver);
            return offset - startOffset;
        }

        public global::LiveScore.Soccer.Models.Odds.MatchOddsMovement Deserialize(byte[] bytes, int offset, global::MessagePack.IFormatterResolver formatterResolver, out int readSize)
        {
            if (global::MessagePack.MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var startOffset = offset;
            var length = global::MessagePack.MessagePackBinary.ReadMapHeader(bytes, offset, out readSize);
            offset += readSize;

            var __MatchId__ = default(string);
            var __Bookmaker__ = default(global::LiveScore.Core.Models.Odds.Bookmaker);
            var __OddsMovements__ = default(global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.OddsMovement>);

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
                        __MatchId__ = formatterResolver.GetFormatterWithVerify<string>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 1:
                        __Bookmaker__ = formatterResolver.GetFormatterWithVerify<global::LiveScore.Core.Models.Odds.Bookmaker>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    case 2:
                        __OddsMovements__ = formatterResolver.GetFormatterWithVerify<global::System.Collections.Generic.IEnumerable<global::LiveScore.Core.Models.Odds.OddsMovement>>().Deserialize(bytes, offset, formatterResolver, out readSize);
                        break;
                    default:
                        readSize = global::MessagePack.MessagePackBinary.ReadNextBlock(bytes, offset);
                        break;
                }
                
                NEXT_LOOP:
                offset += readSize;
            }

            readSize = offset - startOffset;

            var ____result = new global::LiveScore.Soccer.Models.Odds.MatchOddsMovement();
            ____result.MatchId = __MatchId__;
            ____result.Bookmaker = __Bookmaker__;
            ____result.OddsMovements = __OddsMovements__;
            return ____result;
        }
    }

}

#pragma warning restore 168
#pragma warning restore 414
#pragma warning restore 618
#pragma warning restore 612