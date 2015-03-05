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
using alfaNET.Common.Data;
using Xunit;
using Xunit.Extensions;

namespace alfaNET.Common.Tests.Data
{
    public class CollectionHelperTests
    {
        private readonly List<int> _nonEmptyListWithoutDuplicates;
        private readonly List<int> _listWithDuplicates;
        private readonly EqualityComparerPredicate<int> _intComparer;

        public CollectionHelperTests()
        {
            _nonEmptyListWithoutDuplicates = new List<int> { 1, 2, 3 };
            _listWithDuplicates = new List<int> { 1, 2, 3, 5, 2, 1 };
            _intComparer = (a, b) => a == b;
        }

        [Fact]
        public void GetDuplicates_RejectsNullList()
        {
            Assert.Throws<ArgumentNullException>(() => CollectionHelper.GetDuplicates(null, _intComparer));
        }

        [Fact]
        public void GetDuplicates_RejectsNullComparer()
        {
            Assert.Throws<ArgumentNullException>(() => CollectionHelper.GetDuplicates(_nonEmptyListWithoutDuplicates, null));
        }

        private IEnumerable<Tuple<int, int>> CreateListAndGetResult(int itemsCount)
        {
            var list = new List<int>();
            for (var i = 0; i < itemsCount; i++)
                list.Add(0);
            return CollectionHelper.GetDuplicates(list, _intComparer);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void GetDuplicates_ReturnsNonNullForSmallLists(int itemsCount)
        {
            Assert.NotNull(CreateListAndGetResult(itemsCount));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        public void GetDuplicates_ReturnsEmptyForSmallLists(int itemsCount)
        {
            Assert.Empty(CreateListAndGetResult(itemsCount));
        }

        [Fact]
        public void GetDuplicates_ReturnsDuplicatesForListWithDuplicates()
        {
            var duplicates = CollectionHelper.GetDuplicates(_listWithDuplicates, _intComparer).ToArray();
            Assert.NotNull(duplicates);
            Assert.NotEmpty(duplicates);
            var firstDuplicate = duplicates.FirstOrDefault(d => d.Item2 == 1);
            var secondDuplicate = duplicates.FirstOrDefault(d => d.Item2 == 2);
            Assert.NotNull(firstDuplicate);
            Assert.NotNull(secondDuplicate);
            Assert.Equal(5, firstDuplicate.Item1);
            Assert.Equal(4, secondDuplicate.Item1);
        }

        [Fact]
        public void GetDuplicates_ReturnsEmptyForListWithDuplicates()
        {
            var duplicates = CollectionHelper.GetDuplicates(_nonEmptyListWithoutDuplicates, _intComparer);
            Assert.NotNull(duplicates);
            Assert.Empty(duplicates);
        }
    }
}