using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _2SemesterEksamensProjekt.ViewModels;

namespace _2SemesterEksamensProjekt.Models
{
    public class Company : BaseViewModel
    {
        private string _companyName;
        private int _companyId;

        public string CompanyName 
        { 
            get { return _companyName; }
            set => SetProperty(ref _companyName, value);
        }

        public int CompanyId 
        { 
            get { return _companyId; }
            set => SetProperty(ref _companyId, value);
        }

        public Company (string companyName, int companyId)
        {
            _companyName = companyName;
            _companyId = companyId;

        }

        public Company() //parameterløs constructor
        {

        }
    }
}
