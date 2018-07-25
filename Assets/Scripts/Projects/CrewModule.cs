using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CrewModule : IStationModule
{
    private Project availableCrewProject;
    private readonly List<Project> completedCrewProjects;

    private readonly List<Project> availableRecyclingProjects;
    private readonly List<Project> completedRecyclingProjects;
    
    public CrewModule()
    {
        availableCrewProject = CreateCrewProject();
        completedCrewProjects = new List<Project>();
        availableRecyclingProjects = new List<Project>();
        completedRecyclingProjects = new List<Project>();
    }

    public IEnumerable<Project> AvailableProjects()
    {
        return new[] {availableCrewProject}.Concat(availableRecyclingProjects);
    }

    public IEnumerable<Project> CompletedProjects()
    {
        return completedCrewProjects.Concat(completedRecyclingProjects);
    }

    private Project CreateCrewProject()
    {
        return new CrewProject(CompleteCrewProject);
    }

    private void CompleteCrewProject(CrewProject project)
    {
        completedCrewProjects.Add(project);
        availableCrewProject = CreateCrewProject();

        availableRecyclingProjects.Add(CreateRecyclingProject());
    }

    private Project CreateRecyclingProject()
    {
        return new RecyclingSystemProject(CompleteRecyclingProject);
    }

    private void CompleteRecyclingProject(RecyclingSystemProject project)
    {
        completedRecyclingProjects.Add(project);
        availableRecyclingProjects.Remove(project);
    }

    private class RecyclingSystemProject : Project
    {
        private readonly Action<RecyclingSystemProject> projectCompletedCallback;

        public RecyclingSystemProject(Action<RecyclingSystemProject> projectCompletedCallback) : base("Upgrade Recycling Systems",
            "Reduces a crew member's upkeep cost.", new[] {new ResourceDelta(ResourceType.RecyclingSystemComponent, 50) })
        {
            this.projectCompletedCallback = projectCompletedCallback;
        }

        public override void OnProjectCompleted()
        {
            projectCompletedCallback(this);
        }
    }

    private class CrewProject : Project
    {
        private readonly Action<CrewProject> projectCompletedCallback;

        public CrewProject(Action<CrewProject> projectCompletedCallback) : base("Add CrewQuartersComponent Member", "Launch another person into space.", new[] { new ResourceDelta(ResourceType.CrewQuartersComponent, 25) })
        {
            this.projectCompletedCallback = projectCompletedCallback;
        }

        public override void OnProjectCompleted()
        {
            projectCompletedCallback(this);
        }
    }
}

