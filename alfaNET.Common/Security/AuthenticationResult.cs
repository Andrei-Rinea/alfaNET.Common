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
namespace alfaNET.Common.Security
{
    /// <summary>
    /// The result of an authentication request
    /// </summary>
    public enum AuthenticationResult
    {
        /// <summary>
        /// Default member, used for detecting lack of initialization
        /// </summary>
        Unknown,

        /// <summary>
        /// The identity was not found in the store.
        /// </summary>
        UserNotFound,

        /// <summary>
        /// The authentication is successful, the credentials are correct.
        /// </summary>
        Successful,

        /// <summary>
        /// The authentication is unsuccessful because the credentials are wrong. The user was found in the store. Do not, usually, leak this (that the user was found in the store) to the final user.
        /// </summary>
        WrongCredentials,

        /// <summary>
        /// The user is temporarily blocked. The user was found in the store, but based on the use policy, because of too many failed authentication requests, the user was blocked for a certain amount of time.
        /// </summary>
        TemporarilyBlocked
    }
}