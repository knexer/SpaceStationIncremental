using System;
using System.Collections.Generic;

public class Project
{
    public readonly IReadOnlyList<ResourceDelta> Cost;
    private readonly Action projectCompletedCallback;
    public void OnProjectCompleted() => projectCompletedCallback();

    public Project(IReadOnlyList<ResourceDelta> cost, Action projectCompletedCallback)
    {
        Cost = cost;
        this.projectCompletedCallback = projectCompletedCallback;
    }
}