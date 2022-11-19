using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cissy.Patterns
{

    public class Bus<Port> where Port : IBlock
    {
        private IDictionary<string, Port> portContainer;

        public Port this[string name]
        {
            get
            {
                Port port;
                if (portContainer.TryGetValue(name, out port))
                    return port;
                else
                    return default(Port);
            }
        }
        public Bus()
        {
            portContainer = new Dictionary<string, Port>();
        }
        public virtual void PlugInPort(Port MessagePort)
        {
            portContainer[MessagePort.BlockName] = MessagePort;
        }
        public virtual void UnPlugPort(Port MessagePort)
        {
            portContainer.Remove(MessagePort.BlockName);
        }
        public virtual void ClearPorts()
        {
            foreach (Port port in GetAllPorts())
            {
                this.UnPlugPort(port);
            }
        }
        public virtual Port GetPort(string Name)
        {
            return this[Name];
        }
        public virtual Port[] GetAllPorts()
        {
            return portContainer.Values.ToArray();
        }
    }
}
