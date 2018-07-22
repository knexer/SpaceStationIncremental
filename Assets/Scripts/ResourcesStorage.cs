using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

public sealed class ResourcesStorage
{
    public readonly ReadOnlyDictionary<ResourceType, ResourceStorage> ResourceStorages;

    public ResourcesStorage()
    {
        var resourceStoragesBuilder = new Dictionary<ResourceType, ResourceStorage>();
        foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
        {
            resourceStoragesBuilder[resource] = new ResourceStorage(100);
        }

        ResourceStorages = new ReadOnlyDictionary<ResourceType, ResourceStorage>(resourceStoragesBuilder);
    }

    public bool TryAddDeltas(IEnumerable<ResourceDelta> deltas)
    {
        if (deltas.All(delta => ResourceStorages[delta.Type].TryAddDelta(delta))) return true;

        foreach (ResourceDelta delta in deltas)
        {
            ResourceStorages[delta.Type].RemoveDelta(delta);
        }
        return false;
    }

    public void AddDelta(ResourceDelta delta)
    {
        ResourceStorages[delta.Type].AddDelta(delta);
    }

    public int NextAmount(ResourceType type) => ResourceStorages[type].NextAmount;

    public void Tick()
    {
        foreach (ResourceStorage storage in ResourceStorages.Values)
        {
            storage.Tick();
        }
    }
}
