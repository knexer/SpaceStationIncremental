using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectQueue : MonoBehaviour
{
    [SerializeField] private GameObject projectCardPrefab;
    [SerializeField] private GameObject projectContainer;
    [SerializeField] private StationContainer station;

    private void Start()
    {
        station.Station.OnChanged += Rebuild;
        Rebuild();
    }

    private void Rebuild()
    {
        for (int i = projectContainer.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(projectContainer.transform.GetChild(i).gameObject);
        }

        foreach (Project project in station.Station.Projects)
        {
            var card = GameObject.Instantiate(projectCardPrefab, projectContainer.transform).GetComponent<ProjectCard>();
            card.Init(project);

            // this will reverse the order so that they go right-to-left
            card.transform.SetAsFirstSibling();
        }
    }
}
