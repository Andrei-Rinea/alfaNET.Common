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
using System.Web.Mvc;
using alfaNET.Common.Web.Mvc.Controllers;
using Xunit;

namespace alfaNET.Common.Web.Mvc.Tests.Controllers
{
    public class ModelStateDictionaryExtensionsTests
    {
        [Fact]
        public void IsInvalid_RejectsNullDictionary()
        {
            Assert.Throws<ArgumentNullException>(() => ModelStateDictionaryExtensions.IsInvalid(null));
        }

        [Fact]
        public void IsInvalid_ReturnsTrueForInvalid()
        {
            var modelStateDictionary = new ModelStateDictionary();
            modelStateDictionary.AddModelError("a", new Exception());
            var result = modelStateDictionary.IsInvalid();
            Assert.True(result);
        }

        [Fact]
        public void IsInvalid_ReturnsFalseForValid()
        {
            var modelStateDictionary = new ModelStateDictionary();
            var result = modelStateDictionary.IsInvalid();
            Assert.False(result);
        }
    }
}