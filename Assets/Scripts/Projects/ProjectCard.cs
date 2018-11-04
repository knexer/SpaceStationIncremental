using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProjectCard : MonoBehaviour
{
    [SerializeField] private Text Name;
    [SerializeField] private Text Description;
    [SerializeField] private Text Cost;

    public Project Project { get; private set; }

    public void Init(Project project)
    {
        this.Project = project;
        Name.text = project.Name;
        Description.text = project.Description;
        Cost.text = string.Join("\n", project.Cost.Select(cost => $"{cost.Type}: {cost.Amount}"));
    }
}
