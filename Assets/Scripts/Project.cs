using System;
using System.Collections.Generic;

public class Project
{
    public readonly string Name;
    public readonly string Description;
    public readonly IReadOnlyList<ResourceDelta> Cost;
    private readonly Action projectCompletedCallback;
    public void OnProjectCompleted() => projectCompletedCallback();

    public Project(string name, string description, IReadOnlyList<ResourceDelta> cost, Action projectCompletedCallback)
    {
        Name = name;
        Description = description;
        Cost = cost;
        this.projectCompletedCallback = projectCompletedCallback;
    }
}