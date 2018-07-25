using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

public class ResourcesStorageTest
{

    [Test]
    public void AllTypesHaveStorages()
    {
        ResourcesStorage storages = new ResourcesStorage();
        Assert.AreEqual(0, storages.NextAmount(ResourceType.Air));
        Assert.AreEqual(0, storages.NextAmount(ResourceType.Food));
        Assert.AreEqual(0, storages.NextAmount(ResourceType.Water));
    }

    [Test]
    public void HappyCase()
    {
        var storages = new ResourcesStorage();
        storages.AddDelta(new ResourceDelta(ResourceType.Air, 20));
        storages.AddDelta(new ResourceDelta(ResourceType.Food, -20));

        Assert.AreEqual(20, storages.NextAmount(ResourceType.Air));
        Assert.AreEqual(-20, storages.NextAmount(ResourceType.Food));

        storages.Tick();

        Assert.AreEqual(20, storages.NextAmount(ResourceType.Air));
        Assert.AreEqual(20, storages.ResourceStorages[ResourceType.Air].CurrentAmount);
    }

    [Test]
    public void TryAddRejectsAllDeltas()
    {
        var storages = new ResourcesStorage();
        storages.TryAddDeltas(new[]
        {
            new ResourceDelta(ResourceType.Air, 20), new ResourceDelta(ResourceType.Air, -30),
            new ResourceDelta(ResourceType.Food, 20)
        });

        Assert.AreEqual(0, storages.NextAmount(ResourceType.Air));
        Assert.AreEqual(0, storages.NextAmount(ResourceType.Food));
    }
}
