using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ResourceStorageTest {

    [Test]
    public void HappyCase()
    {
        ResourceStorage storage = new ResourceStorage(100);
        storage.AddDelta(new ResourceDelta(ResourceType.Air, 50));
        storage.AddDelta(new ResourceDelta(ResourceType.Air, -20));
        ResourceDelta temporary = new ResourceDelta(ResourceType.Air, -40);
        storage.AddDelta(temporary);

        Assert.AreEqual(0, storage.CurrentAmount);
        Assert.AreEqual(-10, storage.NextAmount);

        storage.RemoveDelta(temporary);

        Assert.AreEqual(0, storage.CurrentAmount);
        Assert.AreEqual(30, storage.NextAmount);

        storage.Tick();

        Assert.AreEqual(30, storage.CurrentAmount);
        Assert.AreEqual(30, storage.NextAmount);
    }

    [Test]
    public void TryAddRejectsNegativeNextAmount()
    {
        var storage = new ResourceStorage(100);
        Assert.IsFalse(storage.TryAddDelta(new ResourceDelta(ResourceType.Air, -10)));
        Assert.AreEqual(0, storage.NextAmount);
    }
}
