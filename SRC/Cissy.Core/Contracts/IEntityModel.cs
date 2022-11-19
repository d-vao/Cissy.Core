using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using Cissy.Database;

namespace Cissy
{


    //public static class EntityExtensions
    //{
    //    public static T Clone<T>(this T obj) where T : class ,IEntity
    //    {
    //        using (MemoryStream memStream = new MemoryStream())
    //        {
    //            BinaryFormatter binaryFormatter = new BinaryFormatter(null,
    //                 new StreamingContext(StreamingContextStates.Clone));
    //            binaryFormatter.Serialize(memStream, obj);
    //            memStream.Seek(0, SeekOrigin.Begin);
    //            return binaryFormatter.Deserialize(memStream) as T;
    //        }
    //    }

    //}
    public interface IEntityModel
    {
        IEntity Entity { get; }
    }
    public interface IEntityModel<T> where T : class, IEntity
    {
        T Entity { get; }
    }
    [Serializable]
    public abstract class EntityModel<T> : IEntityModel<T>, INotifyPropertyChanged, INotifyPropertyChanging where T : class, IEntity
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        public EntityModel(T entity)
        {
            this._entity = entity;
        }
        private T _entity;
        //public T EntityCopy { get { return _entity.Clone(); } }
        public T Entity { get { return _entity; } }

        #region "INotifyPropertyChanged"

        /// <summary>
        /// Raises the PropertyChanged event safely.
        /// </summary>
        /// <param name="propertyName">
        /// The property Name.
        /// </param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region "INotifyPropertyChanging"

        /// <summary>
        /// Method called just before a property's value will change.
        /// </summary>
        /// <param name="propertyName">The name of the property whose value changing.</param>
        /// <remarks>
        /// 
        /// This method should only be called when a value is definitely going to be changed. This
        /// should occur after any value validation or other methods are called.
        /// 
        /// </remarks>
        protected virtual void OnPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

        protected virtual bool SetValue<TValueType>(string propertyName, TValueType newValue, ref TValueType oldValue)
        {
            bool isChanged = (!Object.Equals(newValue, oldValue));

            if (isChanged)
            {
                OnPropertyChanging(propertyName);
                oldValue = newValue;
                OnPropertyChanged(propertyName);
            }
            return isChanged;

        }
    }
    public interface IEntityModelFactory<Entity> : IContractVersion where Entity : class, IEntity
    {
        IEntityModel<Entity> BuildModel(Entity entity);
    }
}
