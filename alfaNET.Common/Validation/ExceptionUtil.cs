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
    public static class ExceptionUtil
    {
        public static void ThrowIfNull(object @object, string parameterName)
        {
            if (@object == null) throw new ArgumentNullException(parameterName);
        }

        public static void ThrowIfNullOrEmpty(string @string, string parameterName)
        {
            if (string.IsNullOrEmpty(@string)) throw new ArgumentOutOfRangeException(parameterName, parameterName + " is null or empty");
        }

        public static void ThrowIfNullOrWhitespace(string @string, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(@string)) throw new ArgumentOutOfRangeException(parameterName, parameterName + " is null or whitespace");
        }

        public static void ThrowIfZero(int integer, string parameterName)
        {
            if (integer == 0) throw new ArgumentOutOfRangeException(parameterName, parameterName + " is zero");
        }

        public static void ThrowIfNegative(int integer, string parameterName)
        {
            if (integer < 0) throw new ArgumentOutOfRangeException(parameterName, parameterName + " is negative");
        }

        public static void ThrowIfDefault<T>(T parameterValue, string parameterName) where T : struct
        {
            if (Equals(default(T), parameterValue)) throw new ArgumentOutOfRangeException(parameterName, parameterName + " has default value for its type (" + typeof(T) + ")");
        }

        public static void ThrowIfZeroOrNegative(int parameterValue, string parameterName)
        {
            if (parameterValue <= 0) throw new ArgumentOutOfRangeException(parameterName, parameterName + " is negative");
        }

        public static void ThrowIfDefaultOrUndefined<T>(T enumValue, string parameterName)
            where T : struct
        {
            if (Equals(enumValue, default(T))) throw new ArgumentOutOfRangeException("parameterName", parameterName + " is the default value of the enum " + typeof(T));
            if (!Enum.IsDefined(typeof(T), enumValue)) throw new ArgumentOutOfRangeException("parameterName", parameterName + " is undefined in enum " + typeof(T));
        }
    }
}