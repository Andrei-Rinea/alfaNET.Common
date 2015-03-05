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
using System.Text;

namespace alfaNET.Common.Security
{
    /// <summary>
    /// A contract for hashing data
    /// </summary>
    public interface IHasher
    {
        /// <summary>
        /// Hashes a string by converting it to its binary representation, based on the <see cref="Encoding"/>, and then hashing the resulting bytes using the algorithm.
        /// </summary>
        /// <param name="data">The actual string to be hashed.</param>
        /// <param name="salt">The salt (entropy) data to be used in the process.</param>
        /// <returns>A base64 representation of the hashed data.</returns>
        string Hash(string data, string salt);

        /// <summary>
        /// The <see cref="Encoding"/> used for converting the string data to its binary representation and vice-versa
        /// </summary>
        Encoding Encoding { get; }
    }
}