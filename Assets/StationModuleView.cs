﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StationModuleView : MonoBehaviour
{
    [SerializeField] private StationContainer station;
    [SerializeField] private string stationModuleName;
    [SerializeField] private SpriteButton button;
    [SerializeField] private ModuleModalView projectPanel;
    private IStationModule module;

	// Use this for initialization
	void Start ()
	{
	    this.module = station.Station.Modules.First(candidate => candidate.Name == stationModuleName);
	    this.button.OnClick += ShowProjectsDialog;
	}

    private void ShowProjectsDialog()
    {
        projectPanel.Activate(module);
    }

    // TODO at some point station modules will likely have some notion of 'stages' or 'levels',
    // with a different sprite and set of projects for each level.  This class will need to swap
    // out the sprite buttons and colliders when that level changes.
}
