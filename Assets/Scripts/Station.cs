using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts;


public sealed class Station
{
    private readonly ResourcesStorage resourcesStorage;
    private readonly List<ResourceDelta> upkeepDeltas;
    private readonly List<Project> projects;
    private int numCompletedProjectsNextTurn;

    public Station()
    {
        resourcesStorage = new ResourcesStorage();
        projects = new List<Project>();
        numCompletedProjectsNextTurn = 0;
        upkeepDeltas = new List<ResourceDelta>();
    }

    public void DoTurn()
    {
        resourcesStorage.Tick();

        for (int i = 0; i < numCompletedProjectsNextTurn; i++)
        {
            projects[i].OnProjectCompleted();
        }
        projects.RemoveRange(0, numCompletedProjectsNextTurn);

        CalculateNextTurn();
    }

    private void CalculateNextTurn()
    {
        foreach (ResourceDelta delta in upkeepDeltas)
        {
            resourcesStorage.AddDelta(delta);
        }

        var rocket = new Rocket(); // TODO this needs to stick around, really, if only so that the rocket contents can be communicated to the player

        // Upkeep gets dibs on rocket capacity.
        EnsureUpkeepIsMet(rocket);

        // Fill any remaining space with materials for however many projects we can complete this turn.
        TryCompleteProjects(rocket);

        // TODO fill the rest of the rocket's capacity with some player-configurable default resource.
        // TODO or, possibly, include some subset of the resources for the next project, if any.
    }

    private void EnsureUpkeepIsMet(Rocket rocket)
    {
        foreach (KeyValuePair<ResourceType, ResourceStorage> resourceStorage in resourcesStorage.ResourceStorages)
        {
            if (resourceStorage.Value.NextAmount >= 0) continue;

            var shortfall = new ResourceDelta(resourceStorage.Key, -resourceStorage.Value.NextAmount);
            if (!rocket.TryAddCargo(new[] { shortfall }))
            {
                // There's not enough space on the rocket to handle upkeep!
                // TODO it needs to be impossible to get into this state, or something's gotta give.
                throw new Exception();
            }
            resourcesStorage.AddDelta(shortfall);
        }
    }

    private void TryCompleteProjects(Rocket rocket)
    {
        numCompletedProjectsNextTurn = 0;
        foreach (Project project in projects)
        {
            // Figure out what resources still need to be launched.
            IEnumerable<ResourceDelta> remainingCosts = from cost in project.Cost
                let remaining = cost.Amount - resourcesStorage.NextAmount(cost.Type)
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
            if (!resourcesStorage.TryAddDeltas(remainingCosts)) throw new Exception();

            // And they'll be deducted from the storage after that
            resourcesStorage.TryAddDeltas(project.Cost.Select(cost => new ResourceDelta(cost.Type, -cost.Amount)));
        }
    }
}