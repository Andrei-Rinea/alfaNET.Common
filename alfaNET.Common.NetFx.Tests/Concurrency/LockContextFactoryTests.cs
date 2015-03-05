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
using alfaNET.Common.Concurrency;
using alfaNET.Common.NetFx.Concurrency;
using Xunit;
using Xunit.Extensions;

namespace alfaNET.Common.NetFx.Tests.Concurrency
{
    public class LockContextFactoryTests
    {
        private readonly LockContextFactory _systemUnderTest;

        public LockContextFactoryTests()
        {
            var timeOut = TimeSpan.FromSeconds(1);
            _systemUnderTest = new LockContextFactory(timeOut);
        }

        [Fact]
        public void Constructor_RejectsZeroTimeout()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LockContextFactory(TimeSpan.Zero));
        }

        [Theory]
        [InlineData(LockContextType.Undefined)]
        [InlineData(123)]
        public void CreateLockContext_RejectsBadType(int type)
        {
            var lockContextType = (LockContextType)type;
            Assert.Throws<ArgumentOutOfRangeException>(() => _systemUnderTest.CreateLockContext(lockContextType));
        }

        [Theory]
        [InlineData(LockContextType.Read)]
        [InlineData(LockContextType.Write)]
        public void CreateLockContext_ReturnsNonNull(LockContextType type)
        {
            var lockContext = _systemUnderTest.CreateLockContext(type);
            Assert.NotNull(lockContext);
        }
    }
}