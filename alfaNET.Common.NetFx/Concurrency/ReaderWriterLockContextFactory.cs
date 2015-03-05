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
    public class ReaderWriterLockContextFactory : IReaderWriterLockContextFactory
    {
        private readonly ReaderWriterLockSlim _readerWriterLockSlim;
        private readonly TimeSpan _timeout;

        public ReaderWriterLockContextFactory(TimeSpan timeout)
        {
            ExceptionUtil.ThrowIfDefault(timeout, "timeout");

            _readerWriterLockSlim = new ReaderWriterLockSlim();
            _timeout = timeout;
        }

        public IReaderWriterLockContext CreateReaderWriterLockContext(ReaderWriterLockContextType type)
        {
            ExceptionUtil.ThrowIfDefaultOrUndefined(type, "type");

            return new ReaderWriterLockContext(_readerWriterLockSlim, type, _timeout);
        }

        public void Dispose()
        {
            if (_readerWriterLockSlim != null)
                _readerWriterLockSlim.Dispose();
        }
    }
}