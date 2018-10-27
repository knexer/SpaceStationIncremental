using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IStationModule
{
    string Name { get; }
    IEnumerable<Project> AvailableProjects();
    IEnumerable<Project> CompletedProjects();
}
