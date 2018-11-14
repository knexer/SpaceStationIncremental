using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectCardFactory : MonoBehaviour
{
    [SerializeField] private ProjectCard cardTemplate;

    private readonly Dictionary<Project, GameObject> CardRegistry = new Dictionary<Project, GameObject>();

    public ProjectCard CreateUnique(Project project)
    {
        if (CardRegistry.ContainsKey(project) && CardRegistry[project] != null && !CardRegistry[project].Equals(null)) return null;

        ProjectCard card = Instantiate(cardTemplate);
        card.Init(project);

        CardRegistry[project] = card.gameObject;

        return card;
    }

    public void DestroyCard(ProjectCard card)
    {
        CardRegistry.Remove(card.Project);
        Destroy(card.gameObject);
    }
}
