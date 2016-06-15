using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ProjectBilling.DataAccess;
using System.Collections.ObjectModel;


namespace ProjectBilling.Application
{
    public class ProjectViewModel : Notifier, IProjectViewModel
    {       
        
        #region IProject Properties
        
        private int _id;
        public int ID
        {
            get { return _id; }
            set
            {
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        private double _estimate;
        public double Estimate
        {
            get { return _estimate; }
            set
            {
                _estimate = value;
                NotifyPropertyChanged("Estimate");
            }

        }

        private double _actual;
        public double Actual
        {
            get { return _actual; }
            set
            {
                _actual = value;
                UpdateEstimateStatus();
                NotifyPropertyChanged("Actual");
            }
        }

        #endregion


        private Status _estimateStatus = Status.None;
        
        public Status EstimateStatus
        {
            get { return _estimateStatus; }
            set
            {
                _estimateStatus = value;
                NotifyPropertyChanged("EstimateStatus");
            }
        }

        
        public ProjectViewModel()
        { }

        public ProjectViewModel(IProject project)
        {
            if (project == null)
                return;
            
            ID = project.ID;
            Update(project);

        }

        public void Update(IProject project)
        {
            ID = project.ID;
            Name = project.Name;
            Estimate = project.Estimate;
            Actual = project.Actual;
        }

        private void UpdateEstimateStatus()
        {
            if (Actual == 0)
                EstimateStatus = Status.None;
            else if (Actual <= Estimate)
                EstimateStatus = Status.Good;
            else
                EstimateStatus = Status.Bad;
        }
    }

    

    
}
