using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationContainer : MonoBehaviour {
    public Station Station = new Station();

    public void Awake()
    {
        // TODO temp code to set up a nontrivial model for the UI to render
        Station.ResourcesStorage.AddDelta(new ResourceDelta(ResourceType.Air, 20));
        Station.ResourcesStorage.AddDelta(new ResourceDelta(ResourceType.Water, 30));
        Station.ResourcesStorage.AddDelta(new ResourceDelta(ResourceType.Air, 10));
        Station.ResourcesStorage.AddDelta(new ResourceDelta(ResourceType.Water, -10));
        Station.ResourcesStorage.AddDelta(new ResourceDelta(ResourceType.Food, 40));
        Station.DoTurn();

        Station.AddProject(new Project("Test Project 1", "Does nothing but take your stuff.",
            new[] {new ResourceDelta(ResourceType.Food, 60), new ResourceDelta(ResourceType.Water, 100)}, () => { }));
        Station.AddProject(new Project("Test Project 2", "Builds a magical air-from-nothing factory!",
            new[] {new ResourceDelta(ResourceType.Food, 10), new ResourceDelta(ResourceType.Water, 10)},
            () => Station.AddUpkeepDelta(new ResourceDelta(ResourceType.Air, 10))));
    }

    public void OnNextTurnPressed()
    {
        Station.DoTurn();
    }
}
