using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Calculation.Model
{
    /// <summary>
    /// Simple number.
    /// </summary>
    public sealed class Number : IHasValue, IEquatable<Number>
    {
        private readonly decimal _value;

        /// <summary>
        /// Initialize new instance of <see cref="Number"/>.
        /// </summary>
        /// <param name="value"></param>
        public Number(decimal value)
        {
            _value = value;
        }

        /// <inheritdoc />
        public decimal GetValue() => _value;

        #region Equality members

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return Equals(obj as Number);
        }

        /// <inheritdoc />
        public bool Equals([AllowNull] Number other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return GetValue() == other.GetValue();
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return GetValue().GetHashCode();
        }

        /// <inheritdoc />
        public static bool operator ==(Number lhs, Number rhs)
        {
            if (lhs is null)
            {
                return rhs is null;
            }

            return lhs.Equals(rhs);
        }

        /// <inheritdoc />
        public static bool operator !=(Number lhs, Number rhs)
        {
            return !(lhs == rhs);
        }

        #endregion

        /// <inheritdoc />
        public override string ToString()
        {
            return _value.ToString(CultureInfo.CurrentCulture);
        }
    }
}
