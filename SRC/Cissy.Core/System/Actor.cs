using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System
{
    public class Me
    {
       
    }
    public class Public
    {
      
    }
    public static class Actor
    {
        public static Me Me
        {
            get
            {              
                return new Me();
            }
        }
        public static Public Public
        {
            get
            {               
                return new Public();
            }
        }       
    }
}
