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
        public DateTime GetFileLastWriteTime(string fullPath)
        {
            ExceptionUtil.ThrowIfNullOrWhitespace(fullPath, "fullPath");
            return File.GetLastWriteTime(fullPath);
        }

        public Stream OpenFileForReading(string fullPath)
        {
            return File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        public Stream CreateFile(string fullPath)
        {
            return File.Open(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
        }

        public bool FileDoesNotExist(string fullPath)
        {
            return !File.Exists(fullPath);
        }

        public long GetFileSize(string fullPath)
        {
            var fileInfo = new FileInfo(fullPath);
            return fileInfo.Length;
        }
    }
}