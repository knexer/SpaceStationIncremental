using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    [SerializeField] private StationComponent station;
    [SerializeField] private GameObject RowPrefab;

    private List<GameObject> rows;

	// Use this for initialization
	void Start ()
	{
	    station.Station.OnChanged += Rebuild;
        rows = new List<GameObject>();
        foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
        {
            GameObject row = Instantiate(RowPrefab, this.transform);
            rows.Add(row);
            row.GetComponent<ResourceBar>().Init(resource, station.Station.ResourcesStorage.ResourceStorages[resource]);
        }
        Rebuild();
	}

    private void Rebuild()
    {
        int maxValue = station.Station.ResourcesStorage.ResourceStorages.Values
            .Select(storage => Math.Max(storage.NextAmount, storage.CurrentAmount)).Max();

        // for each resource type
        int rowIndex = 0;
        foreach (ResourceType resource in Enum.GetValues(typeof(ResourceType)))
        {
            ResourceStorage storage = station.Station.ResourcesStorage.ResourceStorages[resource];
            rows[rowIndex].SetActive(storage.CurrentAmount > 0 || storage.NextAmount > 0);
            rows[rowIndex].GetComponent<ResourceBar>().OnChanged(maxValue);
            rowIndex++;
        }
    }
}
