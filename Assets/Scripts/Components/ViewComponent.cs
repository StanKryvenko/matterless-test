/// <summary> General class for components used on views </summary>
public abstract class ViewComponent : IView
{
    public int InstanceId { get; private set; }
    public IView ParentView { get; }

    protected ViewComponent(int id, IView parentView)
    {
        InstanceId = id;
        ParentView = parentView;
    }
}