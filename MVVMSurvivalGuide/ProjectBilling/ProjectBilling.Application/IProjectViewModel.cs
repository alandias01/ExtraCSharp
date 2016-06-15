using System;
using ProjectBilling.DataAccess;
namespace ProjectBilling.Application
{
    public interface IProjectViewModel : IProject
    {
        Status EstimateStatus { get; set; }
    }

    public enum Status
    {
        None,
        Good,
        Bad
    }
}
