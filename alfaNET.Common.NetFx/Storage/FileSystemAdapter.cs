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
using System.IO;
using alfaNET.Common.Storage;
using alfaNET.Common.Validation;

namespace alfaNET.Common.NetFx.Storage
{
    /// <summary>
    /// A file system adapter for local file system
    /// </summary>
    public class FileSystemAdapter : IFileSystem
    {
        /// <summary>
        /// Gets the last time of writing to a file
        /// </summary>
        /// <param name="fullPath">The full path to the file. This may not be null or whitespace.</param>
        /// <returns>The time of the last write to the file.</returns>
        /// <exception cref="ArgumentNullException">In case fullPath is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">In case fullPath is whitespace</exception>
        public DateTime GetFileLastWriteTime(string fullPath)
        {
            ExceptionUtil.ThrowIfNullOrWhitespace(fullPath, "fullPath");
            return File.GetLastWriteTime(fullPath);
        }

        /// <summary>
        /// Opens a file for reading
        /// </summary>
        /// <param name="fullPath">The full path to the file. This may not be null or empty.</param>
        /// <returns>An open stream, positioned at the beginning</returns>
        /// <exception cref="ArgumentNullException">In case fullPath is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">In case fullPath is whitespace</exception>
        public Stream OpenFileForReading(string fullPath)
        {
            ExceptionUtil.ThrowIfNullOrWhitespace(fullPath, "fullPath");
            return File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// Creates a new file
        /// </summary>
        /// <param name="fullPath">The full path to the file. This may not be null or empty.</param>
        /// <returns>An empty stream pointing to the file.</returns>
        /// <remarks>If the file already exists it will be overwritten.</remarks>
        /// <exception cref="ArgumentNullException">In case fullPath is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">In case fullPath is whitespace</exception>
        public Stream CreateFile(string fullPath)
        {
            ExceptionUtil.ThrowIfNullOrWhitespace(fullPath, "fullPath");
            return File.Open(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
        }

        /// <summary>
        /// Checks where a certain file does *NOT* exist. Useful for negative checks, as "!" is less readable.
        /// </summary>
        /// <param name="fullPath">The full path to the file. This may not be null or empty.</param>
        /// <returns>true if the file does not exist, false if it exists</returns>
        /// <exception cref="ArgumentNullException">In case fullPath is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">In case fullPath is whitespace</exception>
        public bool FileDoesNotExist(string fullPath)
        {
            ExceptionUtil.ThrowIfNullOrWhitespace(fullPath, "fullPath");
            return !File.Exists(fullPath);
        }

        /// <summary>
        /// Gets the actual size in bytes of the file.
        /// </summary>
        /// <param name="fullPath">The full path to the file. This may not be null or empty.</param>
        /// <returns>The actual size in bytes of the file</returns>
        /// <exception cref="ArgumentNullException">In case fullPath is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">In case fullPath is whitespace</exception>
        public long GetFileSize(string fullPath)
        {
            ExceptionUtil.ThrowIfNullOrWhitespace(fullPath, "fullPath");
            var fileInfo = new FileInfo(fullPath);
            return fileInfo.Length;
        }
    }
}