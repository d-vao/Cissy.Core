using System;
using System.Collections.Generic;
using System.Text;

namespace Cissy.Patterns
{
    public sealed class Singleton<T> where T : new()
    {
        private static T instance = new T();

        private static object lockObject = new object();

        private Singleton()
        { }


        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                        {
                            instance = new T();
                        }
                    }
                }
                return instance;
            }
        }

        public void SetInstance(T value)
        {
            instance = value;
        }
    }
}
