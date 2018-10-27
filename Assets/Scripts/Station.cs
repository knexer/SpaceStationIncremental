using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts;


public sealed class Station
{
    public readonly ResourcesStorage ResourcesStorage;
    private readonly List<ResourceDelta> upkeepDeltas;
    private readonly List<Project> projects;
    public IEnumerable<Project> Projects => projects.AsReadOnly();
    public readonly IEnumerable<IStationModule> Modules;

    private int numCompletedProjectsNextTurn;
    private bool doingTurn = false;

    public event Action OnChanged;

    public Station()
    {
        ResourcesStorage = new ResourcesStorage();
        projects = new List<Project>();
        numCompletedProjectsNextTurn = 0;
        upkeepDeltas = new List<ResourceDelta>();

        Modules = new[] {new CrewModule()};
    }

    public void AddUpkeepDelta(ResourceDelta upkeep)
    {
        upkeepDeltas.Add(upkeep);
        CalculateNextTurn();
    }

    public void RemoveUpkeepDelta(ResourceDelta upkeep)
    {
        upkeepDeltas.Remove(upkeep);
        CalculateNextTurn();
    }

    public void AddProject(Project project)
    {
        projects.Add(project);
        CalculateNextTurn();
    }

    public void RemoveProject(Project project)
    {
        projects.Remove(project);
        CalculateNextTurn();
    }

    public void DoTurn()
    {
        ResourcesStorage.Tick();

        doingTurn = true;
        for (int i = 0; i < numCompletedProjectsNextTurn; i++)
        {
            projects[i].OnProjectCompleted();
        }
        projects.RemoveRange(0, numCompletedProjectsNextTurn);
        doingTurn = false;

        CalculateNextTurn();
    }

    private void CalculateNextTurn()
    {
        if (doingTurn) return;

        ResourcesStorage.Reset();

        foreach (ResourceDelta delta in upkeepDeltas)
        {
            ResourcesStorage.AddDelta(delta);
        }

        var rocket = new Rocket(); // TODO this needs to stick around, really, if only so that the rocket contents can be communicated to the player

        // Upkeep gets dibs on rocket capacity.
        EnsureUpkeepIsMet(rocket);

        // Fill any remaining space with materials for however many projects we can complete this turn.
        TryCompleteProjects(rocket);

        // TODO fill the rest of the rocket's capacity with some player-configurable default resource.
        // TODO or, possibly, include some subset of the resources for the next project, if any.

        OnChanged?.Invoke();
    }

    private void EnsureUpkeepIsMet(Rocket rocket)
    {
        foreach (KeyValuePair<ResourceType, ResourceStorage> resourceStorage in ResourcesStorage.ResourceStorages)
        {
            if (resourceStorage.Value.NextAmount >= 0) continue;

            var shortfall = new ResourceDelta(resourceStorage.Key, -resourceStorage.Value.NextAmount);
            if (!rocket.TryAddCargo(new[] { shortfall }))
            {
                // There's not enough space on the rocket to handle upkeep!
                // TODO it needs to be impossible to get into this state, or something's gotta give.
                throw new Exception();
            }
            ResourcesStorage.AddDelta(shortfall);
        }
    }

    private void TryCompleteProjects(Rocket rocket)
    {
        numCompletedProjectsNextTurn = 0;
        foreach (Project project in projects)
        {
            // Figure out what resources still need to be launched.
            IEnumerable<ResourceDelta> remainingCosts = from cost in project.Cost
                let remaining = cost.Amount - ResourcesStorage.NextAmount(cost.Type)
                where remaining > 0
                select new ResourceDelta(cost.Type, remaining);

            // If the rocket doesn't have room for all this, we can't complete this project
            if (!rocket.TryAddCargo(remainingCosts))
            {
                break;
            }

            // This project will be completed this tick.
            numCompletedProjectsNextTurn++;

            // Its resources will be included in the rocket's cargo
            // TODO and hopefully there's space in space for all of it!
            if (!ResourcesStorage.TryAddDeltas(remainingCosts)) throw new Exception();

            // And they'll be deducted from the storage after that
            ResourcesStorage.TryAddDeltas(project.Cost.Select(cost => new ResourceDelta(cost.Type, -cost.Amount)));
        }
    }
}