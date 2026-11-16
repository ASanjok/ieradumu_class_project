using maui_app_ieradumi_2.Services;
using maui_app_ieradumi_2.ViewModels;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

//Aleksandrs Sloka as25552
//Daņiils Luka Miškins dm25047 
// 2 md vienībtestēšana


public class AddHabitViewModelTests
{
    [Theory]
    [InlineData(null, "Dzert ūdeni katru dienu")]
    [InlineData("Ūdens", null)]
    public async Task CreateCommand_DoesNotSave_WhenFieldIsNull(
    string? name, string? description)
    {
        var service = new Mock<IDataService>();
        var vm = new AddHabitViewModel(service.Object);
        vm.CurrentHabit.HabitName = name!;
        vm.CurrentHabit.HabitDescription = description!;

        await vm.CreateButtonCommand.ExecuteAsync(null);

        // Pārbaudām, ka datu serviss netika izsaukts.
        service.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateCommand_WhenSaveFails_PassesHabitDataAndPropagatesError()
    {
        var expectedError = new InvalidOperationException("Neizdevās saglabāt ieradumu");
        var dataService = new Mock<IDataService>();
        dataService
            .Setup(service => service.SaveHabitAsync(
                "Ūdens", "Dzert ūdeni katru dienu", false))
            .ThrowsAsync(expectedError);

        var viewModel = new AddHabitViewModel(dataService.Object);
        viewModel.CurrentHabit.HabitName = "Ūdens";
        viewModel.CurrentHabit.HabitDescription = "Dzert ūdeni katru dienu";
        viewModel.CurrentHabit.Archived = false;

        var actualError = await Assert.ThrowsAsync<InvalidOperationException>(
            () => viewModel.CreateButtonCommand.ExecuteAsync(null));

        Assert.Same(expectedError, actualError);

        dataService.Verify(
            service => service.SaveHabitAsync(
                "Ūdens", "Dzert ūdeni katru dienu", false),
            Times.Once());
    }


    [Fact]
    public void Constructor_InitializesCurrentHabit()
    {
        var dataService = new Mock<IDataService>();
        var viewModel = new AddHabitViewModel(dataService.Object);

        // Pārbaudām, ka konstruktors izveido jaunu ieradumu.
        Assert.NotNull(viewModel.CurrentHabit);
    }
}