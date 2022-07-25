public class TextComponent : ViewComponent
{
    public string Text
    {
        get => _text;
        set
        {
            if (_text == value) return;
            _text = value;
            GameManager.SetTextByComponentId(InstanceId, _text);
        }
    }
    private string _text;

    public TextComponent(int id, IView view = null) : base(id, view)
    {
        Text = "";
    }
}