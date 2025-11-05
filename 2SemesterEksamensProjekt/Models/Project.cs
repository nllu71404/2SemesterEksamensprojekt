using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2SemesterEksamensProjekt.Models
{
    public class Project
    {
        private string _projectTitle;
        private int _projectId;
        private bool _projectStatus;
        public Company Company { get; set; }

        public string ProjectTitle { get { return _projectTitle; } set {  _projectTitle = value; } }
        public int ProjectId { get { return _projectId; } set { _projectId = value; } }
        public bool ProjectStatus { get { return _projectStatus; } set { _projectStatus = value; } }

        public Project(string projectTitle, int projectId, bool projectStatus, Company company) 
        {
            _projectTitle = projectTitle;
            _projectId = projectId;
            _projectStatus = projectStatus;
            Company = company;
        }

        public Project() //parameterløs constructor
        {

        }
    }
}
