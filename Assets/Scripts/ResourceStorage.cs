using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public sealed class ResourceStorage
{
    public readonly int MaxAmount;
    public int CurrentAmount { get; private set; }
    public int NextAmount => CurrentAmount + nextTickDeltas.Sum(delta => delta.Amount);

    private readonly List<ResourceDelta> nextTickDeltas;

    public ResourceStorage(int maxAmount)
    {
        MaxAmount = maxAmount;
        CurrentAmount = 0;
        nextTickDeltas = new List<ResourceDelta>();
    }

    public void Tick()
    {
        CurrentAmount = NextAmount;
        nextTickDeltas.Clear();
    }

    public bool TryAddDelta(ResourceDelta delta)
    {
        AddDelta(delta);
        if (this.NextAmount >= 0 && this.NextAmount <= MaxAmount) return true;

        RemoveDelta(delta);
        return false;
    }

    public void AddDelta(ResourceDelta delta) => this.nextTickDeltas.Add(delta);
    public void RemoveDelta(ResourceDelta delta) => this.nextTickDeltas.Remove(delta);
}
