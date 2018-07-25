using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class BasicProject : Project
{
    private readonly Action projectCompletedCallback;

    public BasicProject(string name, string description, IReadOnlyList<ResourceDelta> cost, Action projectCompletedCallback) : base(name, description, cost)
    {
        this.projectCompletedCallback = projectCompletedCallback;
    }

    public override void OnProjectCompleted()
    {
        projectCompletedCallback();
    }
}

