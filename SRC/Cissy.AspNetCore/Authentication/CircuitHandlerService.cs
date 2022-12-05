using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Cissy.Authentication
{
    public class CircuitHandlerService : CircuitHandler
    {
        public ConcurrentDictionary<string, Circuit> Circuits { get; set; }
        public event EventHandler CircuitsChanged;

        protected virtual void OnCircuitsChanged()
            => CircuitsChanged?.Invoke(this, EventArgs.Empty);

        public override Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            Circuits[circuit.Id] = circuit;
            OnCircuitsChanged();
            return Task.CompletedTask;
        }

        public override Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
        {
            Circuits.TryRemove(circuit.Id, out var circuitRemoved);
            OnCircuitsChanged();
            return Task.CompletedTask;
        }
    }

}
