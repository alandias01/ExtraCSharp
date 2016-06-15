using System;
using System.ComponentModel;
namespace ProjectBilling.Application
{
    interface IProjectsViewModel : INotifyPropertyChanged
    {
        IProjectViewModel SelectedProject { get; set; }
        void UpdateProject();
    }
}
