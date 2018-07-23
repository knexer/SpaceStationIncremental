using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationComponent : MonoBehaviour {
    public Station Station = new Station();

    public void Awake()
    {
        // TODO temp code to set up a nontrivial model for the UI to render
        Station.ResourcesStorage.AddDelta(new ResourceDelta(ResourceType.Air, 20));
        Station.ResourcesStorage.AddDelta(new ResourceDelta(ResourceType.Water, 30));
        Station.DoTurn();
        Station.ResourcesStorage.AddDelta(new ResourceDelta(ResourceType.Air, 10));
        Station.ResourcesStorage.AddDelta(new ResourceDelta(ResourceType.Water, -10));
        Station.ResourcesStorage.AddDelta(new ResourceDelta(ResourceType.Food, 40));
    }
}
