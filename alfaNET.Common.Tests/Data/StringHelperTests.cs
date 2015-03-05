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
using System.Collections.Generic;
using System.IO;
using System.Text;
using alfaNET.Common.Data;
using Xunit;
using Xunit.Extensions;

namespace alfaNET.Common.Tests.Data
{
    public class StringHelperTests
    {
        private const string String = "Nihil sine deo";
        private static readonly Dictionary<string, byte[]> CorrectBytes = new Dictionary<string, byte[]>
        {
            { "UTF-8",  new byte[] { 78,  105,  104,  105,  108,  32,  115,  105,  110,  101,  32,  100,  101,  111 } },
            { "UTF-16",  new byte[] { 78, 0, 105, 0, 104, 0, 105, 0, 108, 0, 32, 0, 115, 0, 105, 0, 110, 0, 101, 0, 32, 0, 100, 0, 101, 0, 111, 0 } },
            { "ASCII",  new byte[] { 78,  105, 104, 105, 108, 32, 115, 105, 110, 101, 32, 100, 101, 111 } }
        };

        [Fact]
        public void ToStream_RejectsNullString()
        {
            Assert.Throws<ArgumentNullException>(() => StringHelper.ToStream(null));
        }

        [Fact]
        public void ToStream_ReturnsNonNull()
        {
            Assert.NotNull(String.ToStream());
        }

        private static byte[] GetBytes(string @string, string encodingString = null)
        {
            var encoding = encodingString == null ? null : Encoding.GetEncoding(encodingString);
            using (var stream = @string.ToStream(encoding))
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        [Fact]
        public void ToStream_ReturnsNonEmptyStream()
        {
            var bytes = GetBytes(String);
            Assert.NotEmpty(bytes);
        }

        [Theory]
        [InlineData(String, "UTF-8")]
        [InlineData(String, "UTF-16")]
        [InlineData(String, "ASCII")]
        public void ToStream_ReturnsCorrectStream(string @string, string encodingString)
        {
            var bytes = GetBytes(@string, encodingString);
            var correctBytes = CorrectBytes[encodingString];
            Assert.Equal(correctBytes, bytes);
        }
    }
}