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
using System.Reflection;
using alfaNET.Common.Validation;
using Xunit;
using Xunit.Extensions;

namespace alfaNET.Common.Tests.Validation
{
    public class ExceptionUtilTests
    {
        private enum SomeEnum
        {
            Unknown,
            First,
            // ReSharper disable once UnusedMember.Local
            Second
        }

        private const string ParameterName = "parameterName";
        private const string NonEmptyText = "text";
        private const int Zero = 0;
        private const int PositiveNonZero = 1;
        private const int Negative = -1;

        [Fact]
        public void ThrowIfNull_ThrowsIfNull()
        {
            Assert.Throws<ArgumentNullException>(() => ExceptionUtil.ThrowIfNull(null, ParameterName));
        }

        [Fact]
        public void ThrowIfNull_DoesntThrowIfNotNull()
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfNull(new object(), ParameterName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\r\n")]
        public void ThrowIfNull_AcceptsNullyParamterNames(string parameterName)
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfNull(new object(), parameterName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\r\n")]
        public void ThrowIfNullOrEmpty_AcceptsNullyParamterNames(string parameterName)
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfNullOrEmpty(NonEmptyText, parameterName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\r\n")]
        public void ThrowIfNullOrWhitespace_AcceptsNullyParamterNames(string parameterName)
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfNullOrWhitespace(NonEmptyText, parameterName));
        }

        [Fact]
        public void ThrowIfNullOrEmpty_ThrowsIfNull()
        {
            Assert.Throws<ArgumentNullException>(() => ExceptionUtil.ThrowIfNullOrEmpty(null, ParameterName));
        }

        [Fact]
        public void ThrowIfNullOrEmpty_ThrowsIfEmpty()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExceptionUtil.ThrowIfNullOrEmpty("", ParameterName));
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        [InlineData(NonEmptyText)]
        public void ThrowIfNullOrEmpty_DoesntThrowIfNotSo(string @string)
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfNullOrEmpty(@string, ParameterName));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        public void ThrowIfNullOrWhitespace_ThrowsIfWhitespace(string @string)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExceptionUtil.ThrowIfNullOrWhitespace(@string, ParameterName));
        }

        [Fact]
        public void ThrowIfNullOrWhitespace_ThrowsIfNull()
        {
            Assert.Throws<ArgumentNullException>(() => ExceptionUtil.ThrowIfNullOrWhitespace(null, ParameterName));
        }

        [Theory]
        [InlineData(NonEmptyText)]
        public void ThrowIfNullOrWhitespace_DoesntThrowIfNotSo(string @string)
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfNullOrWhitespace(@string, ParameterName));
        }

        [Fact]
        public void ThrowIfZero_ThrowsIfSo()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExceptionUtil.ThrowIfZero(Zero, ParameterName));
        }

        [Theory]
        [InlineData(PositiveNonZero)]
        [InlineData(Negative)]
        public void ThrowIfZero_DoesntThrowIfNotSo(int value)
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfZero(value, ParameterName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\r\n")]
        public void ThrowIfZero_AcceptsNullyParamterNames(string parameterName)
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfZero(PositiveNonZero, parameterName));
        }

        [Fact]
        public void ThrowIfNegative_ThrowsIfSo()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExceptionUtil.ThrowIfNegative(Negative, ParameterName));
        }

        [Theory]
        [InlineData(PositiveNonZero)]
        [InlineData(Zero)]
        public void ThrowIfNegative_DoesntThrowIfNotSo(int value)
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfNegative(value, ParameterName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\r\n")]
        public void ThrowIfNegative_AcceptsNullyParamterNames(string parameterName)
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfNegative(PositiveNonZero, parameterName));
        }

        [Theory]
        [InlineData(typeof(int))]
        [InlineData(typeof(DateTime))]
        public void ThrowIfDefault_ThrowsIfSo(Type type)
        {
            var instance = Activator.CreateInstance(type);
            var genericMethod = typeof(ExceptionUtil).GetMethod("ThrowIfDefault").MakeGenericMethod(new[] { type });
            try
            {
                genericMethod.Invoke(null, new[] { instance, ParameterName });
                Assert.True(false, "Exception not thrown");
            }
            catch (TargetInvocationException e)
            {
                Assert.NotNull(e.InnerException);
                Assert.IsType<ArgumentOutOfRangeException>(e.InnerException);
            }
        }

        [Fact]
        public void ThrowIfDefault_DoesntThrowIfNotSo_Int32()
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfDefault(PositiveNonZero, ParameterName));
        }

        [Fact]
        public void ThrowIfDefault_DoesntThrowIfNotSo_DateTime()
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfDefault(DateTime.Now, ParameterName));
        }

        [Theory]
        [InlineData(Negative)]
        [InlineData(Zero)]
        public void ThrowIfZeroOrNegative_ThrowsIfSo(int integer)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExceptionUtil.ThrowIfZeroOrNegative(integer, ParameterName));
        }

        [Fact]
        public void ThrowIfZeroOrNegative_DoesntThrowIfNotSo()
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfZeroOrNegative(PositiveNonZero, ParameterName));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\r\n")]
        public void ThrowIfZeroOrNegative_AcceptsNullyParameterNames(string parameterName)
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfZeroOrNegative(PositiveNonZero, parameterName));
        }

        [Fact]
        public void ThrowIfDefaultOrUndefined_ThrowsIfDefault()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExceptionUtil.ThrowIfDefaultOrUndefined(SomeEnum.Unknown, ParameterName));
        }

        [Fact]
        public void ThrowIfDefaultOrUndefined_ThrowsIfUndefined()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ExceptionUtil.ThrowIfDefaultOrUndefined((SomeEnum)51, ParameterName));
        }

        [Fact]
        public void ThrowIfDefaultOrUndefined_DoesntThrowIfDefinedAndNotDefault()
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfDefaultOrUndefined(SomeEnum.First, ParameterName));
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\r")]
        [InlineData("\r\n")]
        public void ThrowIfDefaultOrUndefined_AcceptsNullyParameterNames(string parameterName)
        {
            Assert.DoesNotThrow(() => ExceptionUtil.ThrowIfDefaultOrUndefined(SomeEnum.First, parameterName));
        }
    }
}