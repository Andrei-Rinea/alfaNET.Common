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
using System.Text;
using alfaNET.Common.NetFx.Security;
using Xunit;
using Xunit.Extensions;

namespace alfaNET.Common.NetFx.Tests.Security
{
    public class Sha512HasherTests
    {
        private const string Text = "Nihil sine Deo";
        private const string Salt = null;
        private const string Hash = "UJCb4qWTw9Ad2/LVYsTjOjN4oyS8XJuTIxqXSsho/GIcnjd2pGRoXY7uoqEDCzLPg9dgepew1AgFadZhPSa5hQ==";

        private readonly Sha512Hasher _systemUnderTest;

        public Sha512HasherTests()
        {
            _systemUnderTest = new Sha512Hasher(Encoding.UTF8);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "")]
        [InlineData("", null)]
        [InlineData("", "")]
        [InlineData("\t", "")]
        [InlineData("", "\t")]
        [InlineData("\t", "   ")]
        [InlineData("  ", "\t")]
        public void Hash_DoesNotRejectFalseyValues(string data, string salt)
        {
            Assert.DoesNotThrow(() => _systemUnderTest.Hash(data, salt));
        }

        [Fact]
        public void Hash_OutputIs64InLength()
        {
            var hashString = _systemUnderTest.Hash("a", "b");
            var hash = Convert.FromBase64String(hashString);
            Assert.Equal(64, hash.Length);
        }

        [Fact]
        public void Hash_HashesCorrectly()
        {
            var hash = _systemUnderTest.Hash(Text, Salt);
            Assert.Equal(Hash, hash);
        }
    }
}