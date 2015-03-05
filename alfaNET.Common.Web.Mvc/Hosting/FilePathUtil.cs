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
using System.Web.Hosting;
using alfaNET.Common.Validation;

namespace alfaNET.Common.Web.Mvc.Hosting
{
    /// <summary>
    /// <see cref="IFilePathUtil"/> implementation for System.Web
    /// </summary>
    public class FilePathUtil : IFilePathUtil
    {
        /// <summary>
        /// Maps a relative path (example : "~/App_Data/file1.txt" to an absolute, file-system path (example : "C:\Inetpub\wwwroot\MyApp\App_Data\file1.txt")
        /// </summary>
        /// <param name="relativePath">The relative path. This may not be null or whitespace.</param>
        /// <returns>Absolute, file-system path</returns>
        /// <exception cref="ArgumentNullException">In case relativePath is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">In case relativePath is whitespace</exception>
        public string MapPath(string relativePath)
        {
            ExceptionUtil.ThrowIfNullOrWhitespace(relativePath, "relativePath");
            return HostingEnvironment.MapPath(relativePath);
        }
    }
}