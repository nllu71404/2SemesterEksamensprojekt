using Test2SemesterEksamensProjekt.ViewModels.TestableViewModels;
using Timer = _2SemesterEksamensProjekt.Models.TimeRecord;

namespace Test2SemesterEksamensProjekt.ViewModels.Unittests;

[TestClass]
public class TestTimerPageViewModel
{
    // I denne klasse laver vi unit tests for TimerPageViewModel
    // Vi tester: constructor, oprettelse, start, stop, manuel tidsjustering,
    // sletning og gemning af timere

    // Testbar version af ViewModel uden UI-dialoger og timers
    private TestableTimerPageViewModel testingViewModel;

    [TestInitialize]
    public void Init()
    {
        // Arrange (fælles setup)
        // Opretter en ny ViewModel før hver test for at sikre test-isolation
        testingViewModel = new TestableTimerPageViewModel();
    }

    [TestMethod]
    public void Constructor_InitializesEmptyTimerList()
    {
        // Assert
        // Ved oprettelse må der ikke findes nogen timere
        Assert.AreEqual(0, testingViewModel.Timers.Count);
    }

    [TestMethod]
    public void CreateTimer_SavesNewTimer_AddsTimerToCollection()
    {
        // Arrange
        // Brugeren indtaster et navn på timeren
        testingViewModel.TimerName = "TestTimer";

        // Act
        // Brugeren opretter en ny timer via UI-kommandoen
        testingViewModel.CreateTimerCommand.Execute(null);

        // Assert
        // Timeren skal være tilføjet ViewModel
        Assert.AreEqual(1, testingViewModel.Timers.Count);
        Assert.AreEqual("TestTimer", testingViewModel.Timers[0].TimerName);
    }

    [TestMethod]
    public void StartTimer_SetsIsRunningAndStartTime()
    {
        // Arrange
        // Opretter en ny timer uden starttid
        var timer = new Timer();

        // Act
        // Brugeren starter timeren
        testingViewModel.StartTimerCommand.Execute(timer);

        // Assert
        // Timeren skal nu være markeret som kørende
        Assert.IsTrue(timer.IsRunning);

        // Starttidspunkt skal være sat
        Assert.IsNotNull(timer.StartTime);
    }

    [TestMethod]
    public void StopTimer_SetsIsRunningFalse()
    {
        // Arrange
        // Opretter en timer der allerede kører
        var timer = new Timer { IsRunning = true };

        // Act
        // Brugeren stopper timeren
        testingViewModel.StopTimerCommand.Execute(timer);

        // Assert
        // Timeren må ikke længere være kørende
        Assert.IsFalse(timer.IsRunning);
    }

    [TestMethod]
    public void AddManualTime_AddsCorrectAmount()
    {
        // Arrange
        // Timer med 1 time og 30 minutter klar til manuel tilføjelse
        var timer = new Timer
        {
            ElapsedTime = TimeSpan.Zero,
            ManualHours = 1,
            ManualMinutes = 30
        };

        // Act
        // Brugeren tilføjer manuel tid
        testingViewModel.AddManualTimeCommand.Execute(timer);

        // Assert
        // Tiden skal være lagt korrekt til
        Assert.AreEqual(TimeSpan.FromMinutes(90), timer.ElapsedTime);

        // Inputfelter skal nulstilles efter brug
        Assert.AreEqual(0, timer.ManualHours);
        Assert.AreEqual(0, timer.ManualMinutes);
    }

    [TestMethod]
    public void SubtractManualTime_SubtractsCorrectAmount()
    {
        // Arrange
        // Timer med 2 timers registreret tid
        var timer = new Timer
        {
            ElapsedTime = TimeSpan.FromHours(2),
            ManualHours = 1,
            ManualMinutes = 0
        };

        // Act
        // Brugeren trækker 1 time fra manuelt
        testingViewModel.SubtractManualTimeCommand.Execute(timer);

        // Assert
        // Den resterende tid skal være korrekt
        Assert.AreEqual(TimeSpan.FromHours(1), timer.ElapsedTime);
    }

    [TestMethod]
    public void SubtractManualTime_NeverBelowZero()
    {
        // Arrange
        // Timer med mindre tid end det, der forsøges trukket fra
        var timer = new Timer
        {
            ElapsedTime = TimeSpan.FromMinutes(10),
            ManualHours = 1,
            ManualMinutes = 0
        };

        // Act
        // Brugeren forsøger at trække mere tid fra end muligt
        testingViewModel.SubtractManualTimeCommand.Execute(timer);

        // Assert
        // Tiden må aldrig blive negativ
        Assert.AreEqual(TimeSpan.Zero, timer.ElapsedTime);
    }

    [TestMethod]
    public void DeleteTimer_RemovesTimerFromCollection()
    {
        // Arrange
        // Opretter og tilføjer en timer til ViewModel
        var timer = new Timer { TimerName = "ToDelete" };
        testingViewModel.Timers.Add(timer);

        // Act
        // Brugeren sletter timeren
        testingViewModel.DeleteTimerCommand.Execute(timer);

        // Assert
        // Timeren skal være fjernet fra ViewModel
        Assert.AreEqual(0, testingViewModel.Timers.Count);
    }

    [TestMethod]
    public void SaveTimer_RemovesTimerFromList()
    {
        // Arrange
        // Opretter og tilføjer en timer, som skal gemmes
        var timer = new Timer { TimerName = "ToSave" };
        testingViewModel.Timers.Add(timer);

        // Act
        // Brugeren gemmer timeren
        testingViewModel.SaveTimerCommand.Execute(timer);

        // Assert
        // Timeren fjernes fra listen efter gemning
        Assert.AreEqual(0, testingViewModel.Timers.Count);
    }
}
