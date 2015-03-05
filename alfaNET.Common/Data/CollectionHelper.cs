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
using System.Linq;

using alfaNET.Common.Validation;

namespace alfaNET.Common.Data
{
    public static class CollectionHelper
    {
        public static IEnumerable<Tuple<int, TEntry>> GetDuplicates<TEntry>(IList<TEntry> entries, EqualityComparerPredicate<TEntry> comparerPredicate)
        {
            ExceptionUtil.ThrowIfNull(entries, "entries");
            if (entries.Count < 2) return Enumerable.Empty<Tuple<int, TEntry>>();
            ExceptionUtil.ThrowIfNull(comparerPredicate, "comparerPredicate");

            var duplicateIndices = new List<int>();
            for (var i = 0; i < entries.Count; i++)
            {
                if (duplicateIndices.Contains(i))
                    continue;
                for (var j = 0; j < entries.Count; j++)
                {
                    if (i == j) continue;
                    if (comparerPredicate(entries[i], entries[j]))
                        duplicateIndices.Add(j);
                }
            }
            return duplicateIndices
                .Select(i => new Tuple<int, TEntry>(i, entries[i]))
                .ToArray();
        }
    }
}