/// <summary> Interface of IView gives method for components to notify View about different actions. View could then send it next to Controller or change outer look </summary>
public interface IView
{
    void Show();
    void Hide();
}