// Copyright 2015 Andrei Rînea
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;

namespace alfaNET.Common.Validation
{
    /// <summary>
    /// Helper class for validating parameters and throwing appropiate exceptions if validation fails.
    /// </summary>
    public static class ExceptionUtil
    {
        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if the object is null.
        /// </summary>
        /// <param name="object">The parameter values.</param>
        /// <param name="parameterName">The parameter name. This is used in generating the exception message.</param>
        /// <exception cref="ArgumentNullException">In case the parameter value is null</exception>
        public static void ThrowIfNull(object @object, string parameterName)
        {
            if (@object == null) throw new ArgumentNullException(parameterName);
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if the string is null or <see cref="ArgumentOutOfRangeException"/> if the string is empty.
        /// </summary>
        /// <param name="string">The parameter values.</param>
        /// <param name="parameterName">The parameter name. This is used in generating the exception message.</param>
        /// <exception cref="ArgumentNullException">In case the string is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">In case the string is empty</exception>
        public static void ThrowIfNullOrEmpty(string @string, string parameterName)
        {
            ThrowIfNull(@string, parameterName);
            if (string.IsNullOrEmpty(@string)) throw new ArgumentOutOfRangeException(parameterName, parameterName + " is null or empty");
        }

        /// <summary>
        /// Throws <see cref="ArgumentNullException"/> if the string is null or <see cref="ArgumentOutOfRangeException"/> if the string is empty or whitespace only.
        /// </summary>
        /// <param name="string">The parameter values.</param>
        /// <param name="parameterName">The parameter name. This is used in generating the exception message.</param>
        /// <exception cref="ArgumentNullException">In case the string is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">In case the string is empty or whitespace only</exception>
        public static void ThrowIfNullOrWhitespace(string @string, string parameterName)
        {
            ThrowIfNull(@string, parameterName);
            if (string.IsNullOrWhiteSpace(@string)) throw new ArgumentOutOfRangeException(parameterName, parameterName + " is null or whitespace");
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> if the integer is zero.
        /// </summary>
        /// <param name="integer">The parameter value</param>
        /// <param name="parameterName">The parameter name. This is used in generating the exception message.</param>
        /// <exception cref="ArgumentOutOfRangeException">In case the integer is zero</exception>
        public static void ThrowIfZero(int integer, string parameterName)
        {
            if (integer == 0) throw new ArgumentOutOfRangeException(parameterName, parameterName + " is zero");
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> if the integer is negative. Zero is allowed.
        /// </summary>
        /// <param name="integer">The parameter value</param>
        /// <param name="parameterName">The parameter name. This is used in generating the exception message.</param>
        /// <exception cref="ArgumentOutOfRangeException">In case the integer is negative.</exception>
        public static void ThrowIfNegative(int integer, string parameterName)
        {
            if (integer < 0) throw new ArgumentOutOfRangeException(parameterName, parameterName + " is negative");
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> if the integer is zero or negative.
        /// </summary>
        /// <param name="parameterValue">The parameter value</param>
        /// <param name="parameterName">The parameter name. This is used in generating the exception message.</param>
        /// <exception cref="ArgumentOutOfRangeException">In case the integer is zero or negative.</exception>
        public static void ThrowIfZeroOrNegative(int parameterValue, string parameterName)
        {
            ThrowIfZero(parameterValue, parameterName);
            ThrowIfNegative(parameterValue, parameterName);
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> if the value type has its default value.
        /// </summary>
        /// <typeparam name="T">The value type of the parameter</typeparam>
        /// <param name="parameterValue">The parameter value</param>
        /// <param name="parameterName">The parameter name. This is used in generating the exception message.</param>
        /// <exception cref="ArgumentOutOfRangeException">In case the parameter has the default value</exception>
        public static void ThrowIfDefault<T>(T parameterValue, string parameterName) where T : struct
        {
            if (Equals(default(T), parameterValue)) throw new ArgumentOutOfRangeException(parameterName, parameterName + " has default value for its type (" + typeof(T) + ")");
        }

        /// <summary>
        /// Throws <see cref="ArgumentOutOfRangeException"/> in case an enum value is either undefined in the enum type either has the enum default value.
        /// </summary>
        /// <typeparam name="T">The enum type</typeparam>
        /// <param name="enumValue">The enum parameter value</param>
        /// <param name="parameterName">The parameter name. This is used in generating the exception message.</param>
        /// <exception cref="ArgumentOutOfRangeException">In case the enum value is either undefined either default value</exception>
        public static void ThrowIfDefaultOrUndefined<T>(T enumValue, string parameterName)
            where T : struct
        {
            if (Equals(enumValue, default(T))) throw new ArgumentOutOfRangeException("parameterName", parameterName + " is the default value of the enum " + typeof(T));
            if (!Enum.IsDefined(typeof(T), enumValue)) throw new ArgumentOutOfRangeException("parameterName", parameterName + " is undefined in enum " + typeof(T));
        }
    }
}