using _2SemesterEksamensProjekt.Repository;
using _2SemesterEksamensProjekt.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test2SemesterEksamensProjekt.ViewModels.TestableViewModels
{
    public class TestableOverViewPageViewModel : OverViewPageViewModel
    {
        // Lader testen styre om "Gem fil"-dialogen skal returnere OK eller Annuller
        public bool DialogResult { get; set; } = true;

        // Giver testen muligheden for at se hvilket filnavn der blev "gemt"
        public string? SavedFileName { get; private set; }

        // Giver testen mulighed for at tjekke om CsvExport blev kaldt
        public bool CsvExportCalled { get; private set; }

        public TestableOverViewPageViewModel(
            ITimeRecordRepository timeRecordRepository,
            ICompanyRepository companyRepository,
            IProjectRepository projectRepository,
            ITopicRepository topicRepository,
            ICsvExportService csvExportService)
            : base(timeRecordRepository, companyRepository, projectRepository, topicRepository, csvExportService)
        {
        }

        // ---------------------------------------------------------
        //                 OVERRIDES TIL UNIT TESTS
        //     Fjerner UI-dialoger så tests ikke viser popups
        // ---------------------------------------------------------

        protected override bool? ShowDialog(out string fileName)
        {
            fileName = "test.csv";
            SavedFileName = fileName;
            return DialogResult;
        }

        protected void MarkCsvCalled()
        {
            CsvExportCalled = true;
        }
    }
}
