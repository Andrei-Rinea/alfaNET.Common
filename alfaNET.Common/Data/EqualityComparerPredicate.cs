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

using System.Collections.Generic;

namespace alfaNET.Common.Data
{
    /// <summary>
    /// Delegate for specifying an equality comparer. This is more lightweight than creating an implementation of <see cref="IEqualityComparer{T}"/>
    /// </summary>
    /// <typeparam name="TEntry">The type of compared items</typeparam>
    /// <param name="item1">The first item</param>
    /// <param name="item2">The second item</param>
    /// <returns></returns>
    public delegate bool EqualityComparerPredicate<in TEntry>(TEntry item1, TEntry item2);
}