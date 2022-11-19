using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cissy.Caching.WeakCaching
{
    public class CacheItem<T> where T : class, IModel
    {
        //弱引用
        private readonly WeakReference _target;
        //过期时间
        public DateTime ExpirtTime { get; private set; }
        ////缓存键
        //public string Key { get; set; }
        //是否当前
        public bool IsCurrent
        {
            get
            {
                return DateTime.UtcNow < this.ExpirtTime && this._target.IsAlive;
            }
        }
        //缓存对象
        public T Target
        {
            get
            {
                if (IsCurrent)
                {
                    return this._target.Target as T;
                }
                else
                {
                    return default(T);
                }
            }
        }
        public CacheItem(T Item, DateTime ExpirtTime)
        {
            _target = new WeakReference(Item);
            this.ExpirtTime = ExpirtTime;
        }

    }
}
