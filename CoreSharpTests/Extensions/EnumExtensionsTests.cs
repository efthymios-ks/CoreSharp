﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using FluentAssertions;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture()]
    public class EnumExtensionsTests
    {
        [Test]
        public void GetValues_WhenTypeNotEnum_ThrowArgumentException()
        {
            //Act 
            Action action = () => EnumExtensions.GetValues<NotAnEnum>();

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void GetValues_WhenCalled_ReturnEnumValues()
        {
            //Arrange
            var values = new[] { DummyEnum.Option1, DummyEnum.Option2, DummyEnum.Option3 };

            //Act 
            var result = EnumExtensions.GetValues<DummyEnum>();

            //Assert
            result.Should().Equal(values);
        }

        [Test]
        public void GetDictionary_WhenTypeNotEnum_ThrowArgumentException()
        {
            //Act 
            Action action = () => EnumExtensions.GetDictionary<NotAnEnum>();

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void GetDictionary_WhenTypeNotEnum_ReturnEnumTextValueDictionary()
        {
            //Arrange
            var dictionary = new Dictionary<string, DummyEnum>();
            dictionary.Add($"{DummyEnum.Option1}", DummyEnum.Option1);
            dictionary.Add($"{DummyEnum.Option2}", DummyEnum.Option2);
            dictionary.Add($"{DummyEnum.Option3}", DummyEnum.Option3);

            //Act 
            var result = EnumExtensions.GetDictionary<DummyEnum>();

            //Assert
            result.Should().Equal(dictionary);
        }

        //Nested
        private enum DummyEnum
        {
            Option1 = 1,
            Option2 = 2,
            Option3 = 3
        }

        private struct NotAnEnum : IConvertible
        {
            public TypeCode GetTypeCode()
            {
                throw new NotImplementedException();
            }

            public bool ToBoolean(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public byte ToByte(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public char ToChar(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public DateTime ToDateTime(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public decimal ToDecimal(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public double ToDouble(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public short ToInt16(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public int ToInt32(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public long ToInt64(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public sbyte ToSByte(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public float ToSingle(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public string ToString(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public object ToType(Type conversionType, IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public ushort ToUInt16(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public uint ToUInt32(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }

            public ulong ToUInt64(IFormatProvider provider)
            {
                throw new NotImplementedException();
            }
        }
    }
}