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

namespace alfaNET.Common.Logging
{
    /// <summary>
    /// Contract for a logger object that allows logging various kinds of messages.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a message to the current logger.
        /// </summary>
        /// <param name="level">The severity of the message.</param>
        /// <param name="message">The actual message. This may contain tokens.</param>
        /// <param name="exception">A caught exception for including besides the message. This is optional, it may be null.</param>
        /// <param name="messageData">A list of objects to replace the optional tokens in the message. This is optional.</param>
        void Log(LogLevel level, string message, Exception exception = null, params object[] messageData);
    }
}