using System;
using System.Collections.Generic;
using System.ComponentModel;
using FluentAssertions;
using NUnit.Framework;

namespace CoreSharp.Extensions.Tests
{
    [TestFixture]
    public class EnumExtensionsTests
    {
        [Test]
        public void GetValues_TypeIsNotEnum_ThrowArgumentException()
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
            var values = new[] { Dummy.Option1, Dummy.Option2, Dummy.Option3 };

            //Act 
            var result = EnumExtensions.GetValues<Dummy>();

            //Assert
            result.Should().Equal(values);
        }

        [Test]
        public void GetDictionary_TypeIsNotEnum_ThrowArgumentException()
        {
            //Act 
            Action action = () => EnumExtensions.GetDictionary<NotAnEnum>();

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void GetDictionary_WhenCalled_ReturnEnumTextValueDictionary()
        {
            //Arrange
            var dictionary = new Dictionary<string, Dummy>();
            dictionary.Add($"{Dummy.Option1}", Dummy.Option1);
            dictionary.Add($"{Dummy.Option2}", Dummy.Option2);
            dictionary.Add($"{Dummy.Option3}", Dummy.Option3);

            //Act 
            var result = EnumExtensions.GetDictionary<Dummy>();

            //Assert
            result.Should().Equal(dictionary);
        }

        [Test]
        public void GetDescription_TypeIsNotEnum_ThrowArgumentException()
        {
            //Arrange 
            var item = new NotAnEnum();

            //Act 
            Action action = () => item.GetDescription();

            //Assert
            action.Should().Throw<ArgumentException>();
        }

        [Test]
        public void GetDescription_WhenCalled_ReturnEnumDescriptionAttribute()
        {
            //Arrange 
            var item = Dummy.Option1;
            string expected = "Description 1";

            //Act 
            var result = item.GetDescription();

            result.Should().Be(expected);
        }

        //Nested
        private enum Dummy
        {
            [System.ComponentModel.Description("Description 1")]
            Option1 = 1,

            [System.ComponentModel.Description("Description 2")]
            Option2 = 2,

            [System.ComponentModel.Description("Description 3")]
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