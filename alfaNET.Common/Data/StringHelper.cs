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
using System.IO;
using System.Text;

using alfaNET.Common.Validation;

namespace alfaNET.Common.Data
{
    public static class StringHelper
    {
        public static Stream ToStream(this string @string, Encoding encoding = null)
        {
            ExceptionUtil.ThrowIfNull(@string, "string");
            encoding = encoding ?? Encoding.UTF8;
            var bytes = encoding.GetBytes(@string);
            return new MemoryStream(bytes);
        }
    }
}