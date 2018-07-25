using System;
using System.Collections.Generic;

public abstract class Project
{
    public readonly string Name;
    public readonly string Description;
    public readonly IReadOnlyList<ResourceDelta> Cost;
    private readonly Action projectCompletedCallback;
    public abstract void OnProjectCompleted();

    protected Project(string name, string description, IReadOnlyList<ResourceDelta> cost)
    {
        Name = name;
        Description = description;
        Cost = cost;
    }
}