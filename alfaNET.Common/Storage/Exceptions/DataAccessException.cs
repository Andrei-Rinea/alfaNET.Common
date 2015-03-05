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

namespace alfaNET.Common.Storage.Exceptions
{
    /// <summary>
    /// A data access exception occured. This can include inability to read data or write data or other issues.
    /// </summary>
    public class DataAccessException : Exception
    {
        /// <summary>
        /// Constructs a new instance of <see cref="DataAccessException"/>
        /// </summary>
        public DataAccessException()
        {

        }

        /// <summary>
        /// Constructs a new instance of <see cref="DataAccessException"/>
        /// </summary>
        /// <param name="message">Exception message</param>
        public DataAccessException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructs a new instance of <see cref="DataAccessException"/>
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Root cause</param>
        public DataAccessException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}