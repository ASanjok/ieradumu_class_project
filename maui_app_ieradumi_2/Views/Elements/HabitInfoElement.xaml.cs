using class_project_ieradumi;
using CommunityToolkit.Mvvm.Messaging;

namespace maui_app_ieradumi_2.Views.Elements;

public partial class HabitInfoElement : ContentView
{
	public HabitInfoElement()
	{
		InitializeComponent();
    }
    private void OnEditClicked(object sender, EventArgs e)
    {
        if (BindingContext is Habit habit)
        {
            WeakReferenceMessenger.Default.Send(new EditHabitMessage(habit));
        }
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        if (BindingContext is Habit habit)
        {
            WeakReferenceMessenger.Default.Send(new DeleteHabitMessage(habit)); // отпровляет сообщение о удалении привычки 
        }
    }
}
public class DeleteHabitMessage : CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<Habit> // само сообщение о удалении привычки
{
    public DeleteHabitMessage(Habit habit) : base(habit) { }
}

public class EditHabitMessage : CommunityToolkit.Mvvm.Messaging.Messages.ValueChangedMessage<Habit>
{
    public EditHabitMessage(Habit habit) : base(habit) { }
}