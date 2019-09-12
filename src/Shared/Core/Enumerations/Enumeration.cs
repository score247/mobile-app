﻿using MessagePack;
// <auto-generated>
// </auto-generated>

namespace LiveScore.Core.Enumerations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using MessagePack;

    [Serializable, MessagePackObject]
    [Union(0, typeof(EventType))]
    [Union(1, typeof(Language))]
    [Union(2, typeof(LeagueRoundType))]
    [Union(3, typeof(PeriodType))]
    [Union(4, typeof(MatchStatus))]
    public abstract class Enumeration : IComparable
    {
        protected Enumeration()
        {
        }

        protected Enumeration(byte value, string displayName)
        {
            Value = value;
            DisplayName = displayName;
        }

        [Key(0)]
        public string DisplayName { get; set; }

        [Key(1)]
        public byte Value { get; set; }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
        {
            return Parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
        }

        public static T FromValue<T>(byte value) where T : Enumeration, new()
        {
            var matchingItem = Parse<T, byte>(value, "value", item => item.Value == value);
            return matchingItem;
        }

        public static IEnumerable<T> GetAll<T>() where T : Enumeration, new()
        {
            var type = typeof(T);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                var instance = new T();
                var locatedValue = info.GetValue(instance) as T;

                if (locatedValue != null)
                {
                    yield return locatedValue;
                }
            }
        }

        public static IEnumerable GetAll(Type type)
        {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            foreach (var info in fields)
            {
                object instance = Activator.CreateInstance(type);
                yield return info.GetValue(instance);
            }
        }

        public static bool operator ==(Enumeration left, Enumeration right)
        {
            return left is null ? right is null : !(right is null) && left.Value == right.Value;
        }

        public static bool operator !=(Enumeration left, Enumeration right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var otherValue = obj as Enumeration;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public virtual int CompareTo(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return Value.CompareTo(((Enumeration)obj).Value);
        }

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem != null)
            {
                return matchingItem;
            }

            var message = $"'{value}' is not a valid {description} in {typeof(T)}";
            throw new ArgumentOutOfRangeException(message);

        }
    }
}