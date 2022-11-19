using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cissy.Caching.WeakCaching
{
    public class Key : Tuple<string, Type>
    {
        public Key(string key, Type result)
            : base(key, result)
        {
        }
        public override bool Equals(object obj)
        {
            if (obj is Key)
            {
                Key k = obj as Key;
                return k.Item1.Equals(this.Item1, StringComparison.OrdinalIgnoreCase) && k.Item2 == this.Item2;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
