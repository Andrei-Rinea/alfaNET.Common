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
using System.Text;
using alfaNET.Common.Data;
using Xunit;
using Xunit.Extensions;

namespace alfaNET.Common.Tests.Data
{
    public class SafeConvertTests
    {
        private const string Garbage = "lkadsjweflkjnm32lkjrflk23jnkln3kldfrnlk23nklr32Z(*#%$#&%";
        private const string Text = "Nihil sine Deo";
        private const string EncodedText = "TmloaWwgc2luZSBEZW8=";
        private static readonly Encoding TextEncoding = Encoding.UTF8;


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData(Garbage)]
        public void FromBase64String_DoesNotThrowOnInvalidArgument(string text)
        {
            Assert.DoesNotThrow(() => SafeConvert.FromBase64String(text));
        }

        [Fact]
        public void FromBase64String_DoesNotReturnNullOnFailure()
        {
            var bytes = SafeConvert.FromBase64String(Garbage);
            Assert.NotNull(bytes);
        }

        [Fact]
        public void FromBase64String_ReturnsEmptyArrayOnFailure()
        {
            var bytes = SafeConvert.FromBase64String(Garbage);
            Assert.Empty(bytes);
        }

        [Fact]
        public void FromBase64String_ReturnsCorrectBytes()
        {
            var bytes = SafeConvert.FromBase64String(EncodedText);
            var @string = TextEncoding.GetString(bytes);
            Assert.Equal(Text, @string);
        }
    }
}