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
using System.IO;
using System.Text;

using alfaNET.Common.Validation;

namespace alfaNET.Common.Data
{
    /// <summary>
    /// Helper class for converting strings.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Converts a string to a stream containing its binary representation in respect to an Encoding.
        /// </summary>
        /// <param name="string">The string instance. This may not be null but it may be empty.</param>
        /// <param name="encoding">The encoding used to convert the string to the binary representation. This may be null, in which case UTF-8 will be assumed.</param>
        /// <returns>A stream (MemoryStream) containing the binary representation of the string, open, poisitioned at the beginning.</returns>
        /// <exception cref="ArgumentNullException">In case @string is null</exception>
        public static Stream ToStream(this string @string, Encoding encoding = null)
        {
            ExceptionUtil.ThrowIfNull(@string, "string");
            encoding = encoding ?? Encoding.UTF8;
            var bytes = encoding.GetBytes(@string);
            return new MemoryStream(bytes);
        }
    }
}