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
using alfaNET.Common.Validation;

namespace alfaNET.Common.NetFx.Concurrency
{
    public class ReaderWriterLockContext : IReaderWriterLockContext
    {
        private readonly ReaderWriterLockSlim _readerWriterLockSlim;

        public ReaderWriterLockContext(
            ReaderWriterLockSlim readerWriterLockSlim,
            ReaderWriterLockContextType type,
            TimeSpan timeout)
        {
            ExceptionUtil.ThrowIfNull(readerWriterLockSlim, "readerWriterLockSlim");
            ExceptionUtil.ThrowIfDefaultOrUndefined(type, "type");
            ExceptionUtil.ThrowIfDefault(timeout, "timeout");

            _readerWriterLockSlim = readerWriterLockSlim;

            bool acquiredLock;
            switch (type)
            {
                case ReaderWriterLockContextType.Reader:
                    acquiredLock = _readerWriterLockSlim.TryEnterReadLock(timeout);
                    break;
                case ReaderWriterLockContextType.Writer:
                    acquiredLock = _readerWriterLockSlim.TryEnterWriteLock(timeout);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
            if (!acquiredLock) throw new TimeoutException();
            Type = type;
        }

        public ReaderWriterLockContextType Type { get; private set; }

        public void Dispose()
        {
            switch (Type)
            {
                case ReaderWriterLockContextType.Reader:
                    _readerWriterLockSlim.ExitReadLock();
                    break;
                case ReaderWriterLockContextType.Writer:
                    _readerWriterLockSlim.ExitWriteLock();
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}