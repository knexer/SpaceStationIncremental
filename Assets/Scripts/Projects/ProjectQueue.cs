using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProjectCardDropTarget))]
public class ProjectQueue : MonoBehaviour
{
    [SerializeField] private GameObject projectCardPrefab;
    [SerializeField] private GameObject projectContainer;
    [SerializeField] private ProjectCardDropTarget projectCardDropTarget;
    [SerializeField] private StationContainer station;
    [SerializeField] private ProjectCardFactory cardFactory;

    private bool viewChanging = false;

    private void Start()
    {
        station.Station.OnChanged += OnModelChanged;
        OnModelChanged();
        projectCardDropTarget.onCardAdded += project => OnViewChanged();
        projectCardDropTarget.onCardRemoved += project => OnViewChanged();
    }

    private void OnModelChanged()
    {
        if (viewChanging) return;
        // Rebuild view from model
        for (int i = projectContainer.transform.childCount - 1; i >= 0; i--)
        {
            cardFactory.DestroyCard(projectContainer.transform.GetChild(i).gameObject.GetComponent<ProjectCard>());
        }

        foreach (Project project in station.Station.Projects)
        {
            ProjectCard card = cardFactory.CreateUnique(project);
            card.transform.SetParent(projectContainer.transform, false);

            // this will reverse the order so that they go right-to-left
            card.transform.SetAsFirstSibling();
        }
    }

    private void OnViewChanged()
    {
        // Rebuild model from view
        var projects = new List<Project>();
        for (int i = 0; i < projectContainer.transform.childCount; i++)
        {
            var card = projectContainer.transform.GetChild(i).GetComponent<ProjectCard>();
            if (card != null)
            {
                projects.Add(card.Project);
            }
        }

        projects.Reverse();

        viewChanging = true;
        station.Station.ReplaceProjects(projects);
        viewChanging = false;
    }
}
