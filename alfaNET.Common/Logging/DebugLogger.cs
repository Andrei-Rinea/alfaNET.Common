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
using System.Diagnostics;

namespace alfaNET.Common.Logging
{
    public class DebugLogger : ILogger
    {
        public void Log(LogLevel level, string message, Exception exception = null, params object[] messageData)
        {
            var line = level + " ";
            if (messageData.Length > 0)
                line += string.Format(message, messageData);
            else
                line += message;
            if (exception != null)
                line += Environment.NewLine + exception;
            line += Environment.NewLine;
            Debug.WriteLine(line);
        }
    }
}