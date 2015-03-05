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
using System.Security.Cryptography;
using System.Text;

using alfaNET.Common.Security;
using alfaNET.Common.Validation;

namespace alfaNET.Common.NetFx.Security
{
    public class Sha512Hasher : IHasher
    {
        private readonly SHA512Managed _hasher;

        public Sha512Hasher(Encoding encoding)
        {
            ExceptionUtil.ThrowIfNull(encoding, "encoding");
            _hasher = new SHA512Managed();
            Encoding = encoding;
        }

        public Encoding Encoding { get; private set; }

        public string Hash(string data, string salt)
        {
            var completeData = data + salt;
            var bytes = Encoding.GetBytes(completeData);
            var hashBytes = _hasher.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}