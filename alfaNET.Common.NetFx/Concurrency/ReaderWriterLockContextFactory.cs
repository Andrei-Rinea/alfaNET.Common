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
    /// <summary>
    /// <see cref="IReaderWriterLockContextFactory"/> implementation for .NET Full Profile using ReaderWriterLockSlim
    /// </summary>
    public class ReaderWriterLockContextFactory : IReaderWriterLockContextFactory
    {
        private readonly ReaderWriterLockSlim _readerWriterLockSlim;
        private readonly TimeSpan _timeout;

        /// <summary>
        /// Constructs a new <see cref="ReaderWriterLockContextFactory"/>
        /// </summary>
        /// <param name="timeout">Timeout for <see cref="ReaderWriterLockContext"/> instances</param>
        public ReaderWriterLockContextFactory(TimeSpan timeout)
        {
            ExceptionUtil.ThrowIfDefault(timeout, "timeout");

            _readerWriterLockSlim = new ReaderWriterLockSlim();
            _timeout = timeout;
        }

        /// <summary>
        /// Creates an instance of an implementation of <see cref="IReaderWriterLockContext"/>
        /// </summary>
        /// <param name="type">The lock context type</param>
        /// <returns>an instance of an implementation of <see cref="IReaderWriterLockContext"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">In case lock context type is undefined</exception>
        public IReaderWriterLockContext CreateReaderWriterLockContext(ReaderWriterLockContextType type)
        {
            ExceptionUtil.ThrowIfDefaultOrUndefined(type, "type");

            return new ReaderWriterLockContext(_readerWriterLockSlim, type, _timeout);
        }

        /// <summary>
        /// Disposes of ((in)directly) unmanaged resources
        /// </summary>
        public void Dispose()
        {
            if (_readerWriterLockSlim != null)
                _readerWriterLockSlim.Dispose();
        }
    }
}