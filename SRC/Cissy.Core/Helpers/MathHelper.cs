using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cissy
{
    public static class MathHelper
    {
        /// <summary>
        /// 按二进制位拆分整数值，如1,2,4,8
        /// </summary>
        /// <param name="X"></param>
        /// <returns></returns>
        public static int[] ToFlags(this int X)
        {
            List<int> list = new List<int>();
            int i = 0;
            while (X > 0)
            {
                int m = X % 2;
                X = X / 2;
                if (m == 1)
                    list.Add((int)Math.Pow(2, i));
                i++;
            }
            return list.ToArray();
        }
    }
}
