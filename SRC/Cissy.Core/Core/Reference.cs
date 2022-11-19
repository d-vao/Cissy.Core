using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy
{
    public class Reference<T> : IModel
    {
        public Reference(T value)
        {
            this.Value = value;
        }
        public T Value;
    }
    public class IntRef : Reference<int>
    {
        public IntRef(int value) : base(value)
        { }
    }
    public class StringRef : Reference<string>
    {
        public StringRef(string value) : base(value)
        { }
    }
    public class DoubleRef : Reference<double>
    {
        public DoubleRef(double value) : base(value)
        { }
    }
    public class LongRef : Reference<long>
    {
        public LongRef(long value) : base(value)
        { }
    }
    public class FloatRef : Reference<float>
    {
        public FloatRef(float value) : base(value)
        { }
    }
    public class CharRef : Reference<char>
    {
        public CharRef(char value) : base(value)
        { }
    }
    public class ByteRef : Reference<byte>
    {
        public ByteRef(byte value) : base(value)
        { }
    }
    public class ShortRef : Reference<short>
    {
        public ShortRef(short value) : base(value)
        { }
    }
}
