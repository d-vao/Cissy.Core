using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cissy
{
    public interface IModel
    {
    }
    public interface IBlock
    {
        string BlockName { get; }
    }
    public interface IBlock<T>
    {
        string BlockName { get; }
    }
    public interface IID
    {
        string ID { get; }
    }
    public interface IDTO : IModel
    {
    }
    public abstract class DTOBase : IDTO
    {
        public int DataOrign { get; set; }
    }
    public class DTOList<T> : DTOBase where T : IDTO
    {
        public T[] List { get; set; }
    }

}
