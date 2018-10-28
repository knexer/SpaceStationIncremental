using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// TODO the project cards need to be buttons that enqueue the project
public class ModuleModalView : MonoBehaviour
{
    private IStationModule currentModule;
    [SerializeField] private ProjectCard cardTemplate;
    [SerializeField] private RectTransform cardContainer;
    [SerializeField] private Text title;

    public void Activate(IStationModule module)
    {
        this.gameObject.SetActive(true);
        this.currentModule = module;
        this.title.text = module.Name;
        foreach (Project availableProject in module.AvailableProjects())
        {
            ProjectCard card = Instantiate(cardTemplate);
            card.transform.SetParent(cardContainer, false);
            card.Init(availableProject);
        }
    }
}
