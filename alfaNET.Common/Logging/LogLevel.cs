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
namespace alfaNET.Common.Logging
{
    /// <summary>
    /// Logging message severity levels
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Default value used for detecting lack of initialization
        /// </summary>
        Unknown,

        /// <summary>
        /// Trace level. Used for tracing performance issues usually.
        /// </summary>
        Trace,

        /// <summary>
        /// Debug level. Used for detecting logical errors usually.
        /// </summary>
        Debug,

        /// <summary>
        /// Info level. Used for informational purposes. For example app start.
        /// </summary>
        Info,

        /// <summary>
        /// Warn level. Used for slightly-abnormal situations usually.
        /// </summary>
        Warn,

        /// <summary>
        /// Error level. Used for runtime errors that do not force the app to shut down.
        /// </summary>
        Error,

        /// <summary>
        /// Fatal level. Used for runtime errors that force the app to shut down.
        /// </summary>
        Fatal
    }
}