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
using alfaNET.Common.Data;
using alfaNET.Common.Logging;
using alfaNET.Common.Security;
using NSubstitute;
using Xunit;
using Xunit.Extensions;

namespace alfaNET.Common.NetFx.Tests.Security
{
    public class SimpleFileAuthenticatorTests
    {
        private readonly IHasher _hasher;
        private readonly ILogger _logger;
        private readonly Func<Stream> _configStreamFunc;
        private readonly SimpleAuthenticator _dummyAuthenticator;

        private const string Username = "Username";
        private const string Password = "Password";

        private const string Salt = "673dd104fd9744cdb7e08101ae04a84d";
        private const string Hash = "QL3XtJc37ojGSYOjjGeOzen7le972H81DDSPKN/BvqOnzfuqx1XJCNQOMqOAU5JacYGjmFv3Ohf8ZXwAdWdADw==";

        private const string WrongUsername = "Usernaem";
        private const string WrongPassword = "Passwrod";

        private const string Config1 = @"<users>
                                            <user name=""" + Username + @""" salt=""" + Salt + @""" hash=""" + Hash + @""" role=""Administrator""/>
                                        </users>";

        public SimpleFileAuthenticatorTests()
        {
            _hasher = Substitute.For<IHasher>();
            _logger = Substitute.For<ILogger>();
            _configStreamFunc = () => null;
            _dummyAuthenticator = new SimpleAuthenticator(_hasher, _configStreamFunc, _logger);
        }

        [Fact]
        public void Constructor_RejectsNullHasher()
        {
            Assert.Throws<ArgumentNullException>(() => new SimpleAuthenticator(null, _configStreamFunc, _logger));
        }

        [Fact]
        public void Constructor_RejectsNullConfigStreamFunc()
        {
            Assert.Throws<ArgumentNullException>(() => new SimpleAuthenticator(_hasher, null, _logger));
        }

        [Fact]
        public void Constructor_RejectsLogger()
        {
            Assert.Throws<ArgumentNullException>(() => new SimpleAuthenticator(_hasher, _configStreamFunc, null));
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, Password)]
        [InlineData(Username, null)]
        [InlineData("", Password)]
        [InlineData(Username, "")]
        public void Authenticate_RejectsBadParameters(string username, string password)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _dummyAuthenticator.Authenticate(username, password));
        }

        private AuthenticationResult GetAuthResult(string username, string password)
        {
            var systemUnderTest = new SimpleAuthenticator(_hasher, () => Config1.ToStream(), _logger);
            return systemUnderTest.Authenticate(username, password);
        }

        [Fact]
        public void Authenticate_ReturnsNotFoundForInexistentUser()
        {
            Assert.Equal(AuthenticationResult.UserNotFound, GetAuthResult(WrongUsername, Password));
        }

        [Fact]
        public void Authenticate_ReturnsWrongCredentialsForWrongCredentials()
        {
            Assert.Equal(AuthenticationResult.WrongCredentials, GetAuthResult(Username,WrongPassword));
        }

        [Fact]
        public void Authenticate_ReturnsSuccessfulForCorrectCredentials()
        {
            _hasher.Hash(Password, Salt).Returns(Hash);
            Assert.Equal(AuthenticationResult.Successful, GetAuthResult(Username,Password));
        }

        [Fact]
        public void Authenticate_CallsHasherCorrectly()
        {
            GetAuthResult(Username, Password);
            _hasher.Received().Hash(Password, Salt);
        }
    }
}