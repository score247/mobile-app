namespace LiveScore.Common.Tests.Extensions
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using AutoFixture;
    using AutoFixture.Kernel;

    public static class AutoFixtureExt
    {
        public static FixtureCustomization<T> For<T>(this Fixture fixture)
            => new FixtureCustomization<T>(fixture);
    }

    public class FixtureCustomization<T>
    {
        public Fixture Fixture { get; }

        public FixtureCustomization(Fixture fixture)
        {
            Fixture = fixture;
        }

        public FixtureCustomization<T> With<TProp>(Expression<Func<T, TProp>> expr, TProp value)
        {
            Fixture.Customizations.Add(new OverridePropertyBuilder<T, TProp>(expr, value));
            return this;
        }

        public T Create() => Fixture.Create<T>();
    }

    public class OverridePropertyBuilder<T, TProp> : ISpecimenBuilder
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly TProp _value;

        public OverridePropertyBuilder(Expression<Func<T, TProp>> expr, TProp value)
        {
            _propertyInfo = (expr.Body as MemberExpression)?.Member as PropertyInfo ??
                            throw new InvalidOperationException("invalid property expression");
            _value = value;
        }

        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as ParameterInfo;
            if (pi == null)
                return new NoSpecimen();

            var camelCase = Regex.Replace(_propertyInfo.Name, @"(\w)(.*)",
                m => m.Groups[1].Value.ToLower() + m.Groups[2]);

            if (pi.ParameterType != typeof(TProp) || pi.Name != camelCase)
                return new NoSpecimen();

            return _value;
        }
    }
}