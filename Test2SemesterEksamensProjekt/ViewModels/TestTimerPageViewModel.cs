using Test2SemesterEksamensProjekt.ViewModels;
using Timer = _2SemesterEksamensProjekt.Models.TimeRecord;

namespace Test2SemesterEksamensProjekt.ViewModels;

[TestClass]
public class TestTimerPageViewModel
{
    // I denne klasse laver vi unit tests for TimerPageViewModel
    // Vi har: Test af constructor, CreateTimer, StartTimer, StopTimer, AddManualTime, SubtractManualTime, DeleteTimer, SaveTimer


    private TestableTimerPageViewModel vm;

    [TestInitialize]
    public void Init()
    {
        vm = new TestableTimerPageViewModel();
    }

    [TestMethod]
    public void Constructor_InitializesEmptyTimerList()
    {
        Assert.AreEqual(0, vm.Timers.Count);
    }

    [TestMethod]
    public void CreateTimer_SavesNewTimer_AddsTimerToCollection()
    {
        vm.TimerName = "TestTimer";

        vm.CreateTimerCommand.Execute(null);

        Assert.AreEqual(1, vm.Timers.Count);
        Assert.AreEqual("TestTimer", vm.Timers[0].TimerName);
    }

    [TestMethod]
    public void StartTimer_SetsIsRunningAndStartTime()
    {
        var timer = new Timer();

        vm.StartTimerCommand.Execute(timer);

        Assert.IsTrue(timer.IsRunning);
        Assert.IsNotNull(timer.StartTime);
    }


    [TestMethod]
    public void StopTimer_SetsIsRunningFalse()
    {
        var timer = new Timer { IsRunning = true };

        vm.StopTimerCommand.Execute(timer);

        Assert.IsFalse(timer.IsRunning);
    }

    [TestMethod]
    public void AddManualTime_AddsCorrectAmount()
    {
        var timer = new Timer
        {
            ElapsedTime = TimeSpan.Zero,
            ManualHours = 1,
            ManualMinutes = 30
        };

        vm.AddManualTimeCommand.Execute(timer);

        Assert.AreEqual(TimeSpan.FromMinutes(90), timer.ElapsedTime);
        Assert.AreEqual(0, timer.ManualHours);
        Assert.AreEqual(0, timer.ManualMinutes);
    }

    [TestMethod]
    public void SubtractManualTime_SubtractsCorrectAmount()
    {
        var timer = new Timer
        {
            ElapsedTime = TimeSpan.FromHours(2),
            ManualHours = 1,
            ManualMinutes = 0
        };

        vm.SubtractManualTimeCommand.Execute(timer);

        Assert.AreEqual(TimeSpan.FromHours(1), timer.ElapsedTime);
    }

    [TestMethod]
    public void SubtractManualTime_NeverBelowZero()
    {
        var timer = new Timer
        {
            ElapsedTime = TimeSpan.FromMinutes(10),
            ManualHours = 1,
            ManualMinutes = 0
        };

        vm.SubtractManualTimeCommand.Execute(timer);

        Assert.AreEqual(TimeSpan.Zero, timer.ElapsedTime);
    }

    [TestMethod]
    public void DeleteTimer_RemovesTimerFromCollection()
    {
        var timer = new Timer { TimerName = "ToDelete" };
        vm.Timers.Add(timer);

        vm.DeleteTimerCommand.Execute(timer);

        Assert.AreEqual(0, vm.Timers.Count);
    }

    [TestMethod]
    public void SaveTimer_RemovesTimerFromList()
    {
        var timer = new Timer { TimerName = "ToSave" };
        vm.Timers.Add(timer);

        vm.SaveTimerCommand.Execute(timer);

        Assert.AreEqual(0, vm.Timers.Count);
    }
}
