﻿// Copyright 2015 Andrei Rînea
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
    /// A contract for authenticating users
    /// </summary>
    public interface IAuthenticator
    {
        /// <summary>
        /// Requests to authenticate a user.
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>An <see cref="AuthenticationResult"/> describing the result of the authentication request.</returns>
        AuthenticationResult Authenticate(string username, string password);
    }
}