using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Linq;

public class StationTest {

    [Test]
    public void TestUpkeep()
    {
        var station = new Station();
        ResourcesStorage storage = station.ResourcesStorage;
        storage.AddDelta(new ResourceDelta(ResourceType.Air, 20));
        station.DoTurn();
        station.AddUpkeepDelta(new ResourceDelta(ResourceType.Air, -10));

        // upkeep depletes stored resource first
        Assert.AreEqual(10, storage.NextAmount(ResourceType.Air));

        station.DoTurn();
        Assert.AreEqual(0, storage.NextAmount(ResourceType.Air));

        // and will launch resources via the rocket instead of going negative
        station.DoTurn();
        Assert.AreEqual(0, storage.NextAmount(ResourceType.Air));
    }

    [Test]
    public void TestAffordableProject()
    {
        var station = new Station();
        bool isCompleted = false;
        var project = new BasicProject("", "", new []{new ResourceDelta(ResourceType.Air, 10)}, () => isCompleted = true);

        station.AddProject(project);

        Assert.AreEqual(0, station.ResourcesStorage.NextAmount(ResourceType.Air));

        Assert.IsFalse(isCompleted);
        station.DoTurn();
        Assert.IsTrue(isCompleted);
    }

    [Test]
    public void TestExpensiveProjectCannotBeCompleted()
    {
        var station = new Station();
        bool isCompleted = false;
        var project = new BasicProject("", "", new []{new ResourceDelta(ResourceType.Air, 100), new ResourceDelta(ResourceType.Food, 100), }, () => isCompleted = true);

        station.AddProject(project);
        station.DoTurn();

        Assert.IsFalse(isCompleted);
    }

    [Test]
    public void TestExpensiveProjectCanBeCompletedWithExistingResources()
    {
        var station = new Station();
        bool isCompleted = false;
        var project = new BasicProject("", "", new[] { new ResourceDelta(ResourceType.Air, 100), new ResourceDelta(ResourceType.Food, 100),  }, () => isCompleted = true);

        station.ResourcesStorage.AddDelta(new ResourceDelta(ResourceType.Air, 100));
        station.DoTurn();
        station.AddProject(project);
        station.DoTurn();

        Assert.IsTrue(isCompleted);
    }

    [Test]
    public void TestProjectsCompletedOnlyOnce()
    {
        var station = new Station();
        int numCompletions = 0;
        var project = new BasicProject("", "", new[] { new ResourceDelta(ResourceType.Air, 100) }, () => numCompletions++);

        station.AddProject(project);
        station.DoTurn();
        Assert.AreEqual(1, numCompletions);

        station.DoTurn();
        Assert.AreEqual(1, numCompletions);
    }

    [Test]
    public void TestUpkeepHasDibs()
    {
        var station = new Station();
        bool isCompleted = false;
        var project = new BasicProject("", "", new[] {new ResourceDelta(ResourceType.Air, 100)}, () => isCompleted = true);
        
        station.AddProject(project);
        station.DoTurn();
        Assert.IsTrue(isCompleted);

        isCompleted = false;
        station.AddProject(project);
        station.AddUpkeepDelta(new ResourceDelta(ResourceType.Air, -10));
        station.DoTurn();

        Assert.IsFalse(isCompleted);
    }

    [Test]
    public void TestCalculateChangesIsIdempotent()
    {
        var station = new Station();
        station.AddUpkeepDelta(new ResourceDelta(ResourceType.Air, 10));
        station.AddUpkeepDelta(new ResourceDelta(ResourceType.Water, 10));

        Assert.AreEqual(10, station.ResourcesStorage.NextAmount(ResourceType.Air));
    }

    [Test]
    public void TestProjectCanModifyStationDuringEndTurn()
    {
        var station = new Station();
        var project = new BasicProject("", "", new[] { new ResourceDelta(ResourceType.Air, 100), new ResourceDelta(ResourceType.Food, 100) },
            () => station.AddUpkeepDelta(new ResourceDelta(ResourceType.Water, 10)));

        station.ResourcesStorage.AddDelta(new ResourceDelta(ResourceType.Air, 100));
        station.DoTurn();

        station.AddProject(project);

        station.DoTurn();

        // upkeep delta should be applied
        Assert.AreEqual(10, station.ResourcesStorage.NextAmount(ResourceType.Water));

        // and project should be removed
        Assert.AreEqual(null, station.Projects.FirstOrDefault());
    }
}
