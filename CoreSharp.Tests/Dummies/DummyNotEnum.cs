﻿using System;

namespace CoreSharp.Tests.Dummies
{
    internal struct DummyNotAnEnum : IConvertible
    {
        public TypeCode GetTypeCode()
        {
            return default;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            return default;
        }

        public byte ToByte(IFormatProvider provider)
        {
            return default;
        }

        public char ToChar(IFormatProvider provider)
        {
            return default;
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            return default;
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            return default;
        }

        public double ToDouble(IFormatProvider provider)
        {
            return default;
        }

        public short ToInt16(IFormatProvider provider)
        {
            return default;
        }

        public int ToInt32(IFormatProvider provider)
        {
            return default;
        }

        public long ToInt64(IFormatProvider provider)
        {
            return default;
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            return default;
        }

        public float ToSingle(IFormatProvider provider)
        {
            return default;
        }

        public string ToString(IFormatProvider provider)
        {
            return default;
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return default;
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            return default;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            return default;
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            return default;
        }
    }
}
