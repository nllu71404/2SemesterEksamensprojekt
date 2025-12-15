using _2SemesterEksamensProjekt.Repository;
using _2SemesterEksamensProjekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Test2SemesterEksamensProjekt.ViewModels.TestableViewModels
{
    public class TestableCompanyPageViewModel : CompanyPageViewModel
    {
        public TestableCompanyPageViewModel(ICompanyRepository companyRepository)
            : base(companyRepository)
        {
        }

        // ---------------------------------------------------------
        //                 OVERRIDES TIL UNIT TESTS
        //     Fjerner UI-dialoger så tests ikke viser popups
        // ---------------------------------------------------------

        // Fjerner normale besked-popups i tests
        protected override void ShowMessage(string msg)
        {
            // Ingen UI under tests
        }

        // Fjerner bekræftelsesdialogen ved sletning
        protected override MessageBoxResult ShowConfirmation(string message)
        {
            // Default: Lad som om brugeren trykker "Ja"
            return MessageBoxResult.Yes;
        }
    }
}
