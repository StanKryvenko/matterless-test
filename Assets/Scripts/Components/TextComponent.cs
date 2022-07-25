public class TextComponent : ViewComponent
{
    public string Text
    {
        get => _text;
        set
        {
            if (_text == value) return;
            _text = value;
            if (!_isShown)
                GameManager.Instance.SetTextByComponentId(InstanceId, _text);
        }
    }
    private string _text;

    private bool _isShown;

    public TextComponent(int id, IView view = null) : base(id, view) { }

    public override void Initialize()
    {
        Text = "";
    }

    public override void Show()
    {
        if (!_isShown)
            GameManager.Instance.SetTextByComponentId(InstanceId, _text);
        
        _isShown = true;
    }

    public override void Hide()
    {
        _isShown = false;
    }
}