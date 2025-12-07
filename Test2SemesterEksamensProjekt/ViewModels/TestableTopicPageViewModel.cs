using _2SemesterEksamensProjekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test2SemesterEksamensProjekt.ViewModels
{
    public class TestableTopicPageViewModel : TopicPageViewModel
    {
        public TestableTopicPageViewModel(_2SemesterEksamensProjekt.Repository.ITopicRepository topicRepository)
            : base(topicRepository)
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
        protected override System.Windows.MessageBoxResult ShowConfirmation(string message)
        {
            // Default: Lad som om brugeren trykker "Ja"
            return System.Windows.MessageBoxResult.Yes;
        }
    }
}
