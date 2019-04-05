﻿namespace LiveScore.Domain.Enumerations
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    [Serializable]
    public abstract class Enumeration : IComparable
    {
        private readonly string _displayName;
        private readonly byte _value;

        protected Enumeration()
        {
        }

        protected Enumeration(byte value, string displayName)
        {
            _value = value;
            _displayName = displayName;
        }

        public string DisplayName
        {
            get { return _displayName; }
        }

        public byte Value
        {
            get { return _value; }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "1",
            Justification = "Reviewed"),
        System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "Reviewed")]
        public static byte AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = (byte)Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }

        public static T FromDisplayName<T>(string displayName) where T : Enumeration, new()
        {
            var matchingItem = Parse<T, string>(displayName, "display name", item => item.DisplayName == displayName);
            return matchingItem;
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
            if (ReferenceEquals(left, null))
            {
                if (ReferenceEquals(right, null))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (ReferenceEquals(right, null))
                {
                    return false;
                }
                else
                {
                    return left.Value == right.Value;
                }
            }
        }

        public static bool operator !=(Enumeration left, Enumeration right)
        {
            return !(left == right);
        }

        public static bool operator >(Enumeration left, Enumeration right)
        {
            ValidateInputArguments(left, right);
            return left.Value > right.Value;
        }

        public static bool operator >=(Enumeration left, Enumeration right)
        {
            ValidateInputArguments(left, right);
            return left.Value >= right.Value;
        }

        public static bool operator <(Enumeration left, Enumeration right)
        {
            ValidateInputArguments(left, right);
            return left.Value < right.Value;
        }

        public static bool operator <=(Enumeration left, Enumeration right)
        {
            ValidateInputArguments(left, right);
            return left.Value <= right.Value;
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

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = _value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
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

        private static void ValidateInputArguments(Enumeration left, Enumeration right)
        {
            if (ReferenceEquals(left, null))
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (ReferenceEquals(right, null))
            {
                throw new ArgumentNullException(nameof(right));
            }
        }

        private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration, new()
        {
            var matchingItem = GetAll<T>().FirstOrDefault(predicate);

            if (matchingItem == null)
            {
                var message = string.Format("'{0}' is not a valid {1} in {2}", value, description, typeof(T));
                throw new ArgumentOutOfRangeException(message);
            }

            return matchingItem;
        }
    }
}
