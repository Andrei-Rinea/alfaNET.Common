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
using System.Threading;
using alfaNET.Common.Concurrency;
using alfaNET.Common.NetFx.Concurrency;
using Xunit;
using Xunit.Extensions;

namespace alfaNET.Common.NetFx.Tests.Concurrency
{
    public class LockContextTests : IDisposable
    {
        private readonly ReaderWriterLockSlim _lock;
        private readonly TimeSpan _timeOut;
        private readonly ManualResetEvent _secondThreadEnteredLock;
        private readonly ManualResetEvent _secondThreadShouldReleaseLock;
        private readonly ManualResetEvent _secondThreadReleasedLock;

        private Thread _thread;

        public LockContextTests()
        {
            _lock = new ReaderWriterLockSlim();
            _timeOut = TimeSpan.FromSeconds(1);
            _secondThreadEnteredLock = new ManualResetEvent(false);
            _secondThreadShouldReleaseLock = new ManualResetEvent(false);
            _secondThreadReleasedLock = new ManualResetEvent(false);
        }

        private void HoldLock(ReaderWriterLockContextType type)
        {
            switch (type)
            {
                case ReaderWriterLockContextType.Reader:
                    _lock.TryEnterReadLock(_timeOut);
                    _secondThreadEnteredLock.Set();
                    _secondThreadShouldReleaseLock.WaitOne();
                    _lock.ExitReadLock();
                    break;
                case ReaderWriterLockContextType.Writer:
                    _lock.TryEnterWriteLock(_timeOut);
                    _secondThreadEnteredLock.Set();
                    _secondThreadShouldReleaseLock.WaitOne();
                    _lock.ExitWriteLock();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            _secondThreadReleasedLock.Set();
        }

        [Fact]
        public void Constructor_RejectsNullLock()
        {
            Assert.Throws<ArgumentNullException>(() => new ReaderWriterLockContext(null, ReaderWriterLockContextType.Reader, _timeOut));
        }

        [Theory]
        [InlineData(ReaderWriterLockContextType.Undefined)]
        [InlineData(31)]
        public void Constructor_RejectsBadType(int type)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReaderWriterLockContext(_lock, (ReaderWriterLockContextType)type, _timeOut));
        }

        [Fact]
        public void Constructor_RejectsZeroTimeout()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new ReaderWriterLockContext(_lock, ReaderWriterLockContextType.Reader, TimeSpan.Zero));
        }

        [Fact]
        public void LockContext_TypeReader_CanEnterWhileAnotherReaderIsOn()
        {
            _thread = new Thread(() => HoldLock(ReaderWriterLockContextType.Reader));
            _thread.Start();
            _secondThreadEnteredLock.WaitOne();
            // ReSharper disable once ObjectCreationAsStatement
            using (new ReaderWriterLockContext(_lock, ReaderWriterLockContextType.Reader, _timeOut)) { }
        }

        [Theory]
        [InlineData(ReaderWriterLockContextType.Reader)]
        [InlineData(ReaderWriterLockContextType.Writer)]
        public void Constructor_SetsType(ReaderWriterLockContextType type)
        {
            using (var systemUnderTest = new ReaderWriterLockContext(_lock, type, _timeOut))
                Assert.Equal(type, systemUnderTest.Type);
        }

        [Fact]
        public void LockContext_TypeReader_CannotEnterWhileSomeWriterIsOn()
        {
            _thread = new Thread(() => HoldLock(ReaderWriterLockContextType.Writer));
            _thread.Start();
            _secondThreadEnteredLock.WaitOne();
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<TimeoutException>(() => new ReaderWriterLockContext(_lock, ReaderWriterLockContextType.Reader, _timeOut));
        }

        [Fact]
        public void LockContext_TypeReader_CanEnterAndExit_IfNoOtherLockIsOn()
        {
            // ReSharper disable once ObjectCreationAsStatement
            using (new ReaderWriterLockContext(_lock, ReaderWriterLockContextType.Reader, _timeOut)) { }
        }

        [Fact]
        public void LockContext_TypeWriter_CanEnterAndExit_IfNoOtherLockIsOn()
        {
            // ReSharper disable once ObjectCreationAsStatement
            using (new ReaderWriterLockContext(_lock, ReaderWriterLockContextType.Writer, _timeOut)) { }
        }

        [Fact]
        public void LockContext_TypeWriter_CannotEnterWhileSomeReaderIsOn()
        {
            _thread = new Thread(() => HoldLock(ReaderWriterLockContextType.Reader));
            _thread.Start();
            _secondThreadEnteredLock.WaitOne();
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<TimeoutException>(() => new ReaderWriterLockContext(_lock, ReaderWriterLockContextType.Writer, _timeOut));
        }

        [Fact]
        public void LockContext_TypeWriter_CannotEnterWhileOtherWriterIsOn()
        {
            _thread = new Thread(() => HoldLock(ReaderWriterLockContextType.Writer));
            _thread.Start();
            _secondThreadEnteredLock.WaitOne();
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<TimeoutException>(() => new ReaderWriterLockContext(_lock, ReaderWriterLockContextType.Writer, _timeOut));
        }

        public void Dispose()
        {
            if (_thread != null)
            {
                _secondThreadShouldReleaseLock.Set();
                _secondThreadReleasedLock.WaitOne();
            }
            _lock.Dispose();
        }
    }
}