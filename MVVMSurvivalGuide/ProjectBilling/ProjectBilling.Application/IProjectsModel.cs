using System;
using System.Collections.ObjectModel;
using ProjectBilling.DataAccess;
namespace ProjectBilling.Application
{
    public interface IProjectsModel
    {
        ObservableCollection<Project> Projects { get; set; }
        event EventHandler<ProjectEventArgs> ProjectUpdated;
        void UpdateProject(IProject updatedProject);
    }
}
