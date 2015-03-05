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

namespace alfaNET.Common.Data
{
    /// <summary>
    /// Helper class for safe converting data. That is, if input data is invalid an empty result will be returned rather than throwing an exception
    /// </summary>
    public static class SafeConvert
    {
        /// <summary>
        /// Conversion error result for byte arrays return-based methods
        /// </summary>
        public static readonly byte[] ConvertError = { };

        /// <summary>
        /// Converts a base64 string to its byte array representation
        /// </summary>
        /// <param name="value">The base64 string</param>
        /// <returns>The byte array representation or an empty byte array result (reference-wise equal to ConvertError).</returns>
        public static byte[] FromBase64String(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return ConvertError;
            try { return Convert.FromBase64String(value); }
            catch { return ConvertError; }
        }
    }
}