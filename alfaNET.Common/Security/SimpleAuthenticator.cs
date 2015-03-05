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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

using alfaNET.Common.Data;
using alfaNET.Common.Logging;
using alfaNET.Common.Validation;

namespace alfaNET.Common.Security
{
    /// <summary>
    /// A simple implementation for XML-based authentication stores.
    /// </summary>
    public class SimpleAuthenticator : IAuthenticator
    {
        private class UserEntry
        {
            public UserEntry(string name, string salt, string hashBase64, string role)
            {
                Name = name;
                Salt = salt;
                HashBase64 = hashBase64;
                Role = role;
            }

            public string Name { get; private set; }
            public string Salt { get; private set; }
            public string HashBase64 { get; private set; }
            // ReSharper disable once MemberCanBePrivate.Local
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string Role { get; private set; }

            public string GetErrorMessage()
            {
                var errors = new List<string>();
                if (string.IsNullOrWhiteSpace(Name))
                    errors.Add("empty or whitespace name");
                if (string.IsNullOrEmpty(Salt))
                    errors.Add("unspecified salt");
                if (string.IsNullOrEmpty(HashBase64))
                    errors.Add("missing or invalid hash");
                if (errors.Any())
                    return "User entry has " + string.Join(",", errors) + ".";
                return null;
            }
        }

        private readonly IHasher _hasher;
        private readonly Func<Stream> _configStreamFunc;
        private readonly ILogger _logger;
        private readonly EqualityComparerPredicate<string> _usernameFinder;

        private UserEntry[] _userEntries;

        /// <summary>
        /// Constructs a new instance of <see cref="SimpleAuthenticator"/>.
        /// </summary>
        /// <param name="hasher">The hasher instance for hashing passwords. This may not be null.</param>
        /// <param name="configStreamFunc">The <see cref="Func{Stream}"/> used to produce the configuration stream. In future, in order to support automatic reloading, a simple <see cref="Stream"/> could not be used. This may not be null. Nor should it produce nulls.</param>
        /// <param name="logger">A logger used to signal certain configuration issues. This may not be null.</param>
        /// <param name="usernameFinder">A matcher used for authenticating users. This could take into account culture and/or casing. This may be null, in which case  a current culture / case ignoring comparator is used.</param>
        /// <exception cref="ArgumentNullException">In case hasher, configStreamFunc or logger is null.</exception>
        public SimpleAuthenticator(
            IHasher hasher,
            Func<Stream> configStreamFunc,
            ILogger logger,
            EqualityComparerPredicate<string> usernameFinder = null)
        {
            ExceptionUtil.ThrowIfNull(hasher, "hasher");
            ExceptionUtil.ThrowIfNull(configStreamFunc, "configStreamFunc");
            ExceptionUtil.ThrowIfNull(logger, "logger");

            _hasher = hasher;
            _configStreamFunc = configStreamFunc;
            _logger = logger;
            _usernameFinder = usernameFinder ?? ((s1, s2) => string.Equals(s1, s2, StringComparison.CurrentCultureIgnoreCase));
        }

        private void EnsureDataIsLoaded()
        {
            if (_userEntries == null)
                LoadData();
        }

        private void LoadData()
        {
            var configStream = _configStreamFunc();
            if (configStream == null) throw new InvalidOperationException("config stream generator yielded null");

            var userEntries = new List<UserEntry>();

            using (configStream)
            using (var xmlReader = XmlReader.Create(configStream))//new XmlTextReader(configStream) { WhitespaceHandling = WhitespaceHandling.None })
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType != XmlNodeType.Element) continue;
                    if (xmlReader.Name == "user")
                        ReadUserEntry(xmlReader, userEntries);
                }
            }
            if (!userEntries.Any())
                _logger.Log(LogLevel.Error, "No (valid) user entries found in config.");
            var duplicateInfo = CollectionHelper.GetDuplicates(userEntries, (a, b) => string.Equals(a.Name, b.Name, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (duplicateInfo.Any())
            {
                foreach (var duplicateItem in duplicateInfo)
                    _logger.Log(LogLevel.Warn, "Duplicate user entry '{0}' at index {1}", null, duplicateItem.Item2.Name, duplicateItem.Item1);
                var duplicates = duplicateInfo.Select(d => d.Item2).ToArray();
                _userEntries = userEntries.Where(u => !duplicates.Contains(u)).ToArray();
            }
            else
                _userEntries = userEntries.ToArray();
        }

        private void ReadUserEntry(XmlReader xmlReader, ICollection<UserEntry> userEntries)
        {
            string name = null, salt = null, role = null, hashBase64 = null;

            while (xmlReader.MoveToNextAttribute())
            {
                switch (xmlReader.Name)
                {
                    case "name":
                        name = xmlReader.Value;
                        break;
                    case "salt":
                        salt = xmlReader.Value;
                        break;
                    case "hash":
                        hashBase64 = xmlReader.Value;
                        break;
                    case "role":
                        role = xmlReader.Value;
                        break;
                }
            }
            var userEntry = new UserEntry(name, salt, hashBase64, role);
            var errorMessage = userEntry.GetErrorMessage();
            if (errorMessage != null)
                _logger.Log(LogLevel.Warn, errorMessage);
            else
                userEntries.Add(userEntry);
        }

        /// <summary>
        /// Requests to authenticate a user.
        /// </summary>
        /// <param name="username">The username. This may not be null or empty.</param>
        /// <param name="password">The password. This may not be null or empty.</param>
        /// <returns>An <see cref="AuthenticationResult"/> describing the result of the authentication request.</returns>
        /// <exception cref="ArgumentOutOfRangeException">In case username or password is/are null or empty.</exception>
        public AuthenticationResult Authenticate(string username, string password)
        {
            ExceptionUtil.ThrowIfNullOrEmpty(username, "username");
            ExceptionUtil.ThrowIfNullOrEmpty(password, "password");

            EnsureDataIsLoaded();

            var entry = _userEntries.SingleOrDefault(u => _usernameFinder(u.Name, username));
            if (entry == null) return AuthenticationResult.UserNotFound;

            var computedHashBase64 = _hasher.Hash(password, entry.Salt);
            return string.Equals(computedHashBase64, entry.HashBase64, StringComparison.OrdinalIgnoreCase) ?
                AuthenticationResult.Successful : AuthenticationResult.WrongCredentials;
        }
    }
}