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
    /// <summary>
    /// SHA 512 Hasher
    /// </summary>
    public class Sha512Hasher : IHasher
    {
        private readonly SHA512Managed _hasher;

        /// <summary>
        /// Constructs a new instance of <see cref="Sha512Hasher"/>
        /// </summary>
        /// <param name="encoding">Encoding used to do string-byte[] conversions. This may not be null.</param>
        /// <exception cref="ArgumentNullException">In case encoding is null</exception>
        public Sha512Hasher(Encoding encoding)
        {
            ExceptionUtil.ThrowIfNull(encoding, "encoding");
            _hasher = new SHA512Managed();
            Encoding = encoding;
        }

        /// <summary>
        /// The <see cref="Encoding"/> used for converting the string data to its binary representation and vice-versa
        /// </summary>
        public Encoding Encoding { get; private set; }

        /// <summary>
        /// Hashes a string by converting it to its binary representation, based on the <see cref="Encoding"/>, and then hashing the resulting bytes using the algorithm.
        /// </summary>
        /// <param name="data">The actual string to be hashed.</param>
        /// <param name="salt">The salt (entropy) data to be used in the process.</param>
        /// <returns>A base64 representation of the hashed data.</returns>
        /// <exception cref="ArgumentNullException">In case data or salt is null</exception>
        public string Hash(string data, string salt)
        {
            ExceptionUtil.ThrowIfNull(data, "data");
            ExceptionUtil.ThrowIfNull(salt, "salt");

            var completeData = data + salt;
            var bytes = Encoding.GetBytes(completeData);
            var hashBytes = _hasher.ComputeHash(bytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}