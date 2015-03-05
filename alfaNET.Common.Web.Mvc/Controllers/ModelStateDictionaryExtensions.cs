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
using alfaNET.Common.Validation;

namespace alfaNET.Common.Web.Mvc.Controllers
{
    /// <summary>
    /// Helper class for easing working with ModelStateDictionary
    /// </summary>
    public static class ModelStateDictionaryExtensions
    {
        /// <summary>
        /// Reverse of the IsValid, in order to produce easier to read code
        /// </summary>
        /// <param name="modelStateDictionary">The <see cref="System.Web.ModelBinding.ModelStateDictionary"/> instance</param>
        /// <returns>true if invalid or false if valid</returns>
        /// <exception cref="ArgumentNullException">In case modelStateDictionary is null</exception>
        public static bool IsInvalid(this ModelStateDictionary modelStateDictionary)
        {
            ExceptionUtil.ThrowIfNull(modelStateDictionary, "modelStateDictionary");
            return !modelStateDictionary.IsValid;
        }
    }
}