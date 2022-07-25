using System.Collections.Generic;
using System.ComponentModel;

/// <summary> Event that could be handled in View by notifying from components </summary>
public abstract class BaseComponentEvent { }

/// <summary> Base view class for describing windows/popups/etc.
/// Execution order: Awake (once per session) -> OnShowing -> OnEnable -> OnHiding -> OnDisable</summary>
public class View<TController, TModel> : IView
    where TModel : BaseModel, new()
    where TController : BaseController<TModel>, new()
{
    protected TController Controller { get; set; }

    public virtual void Initialize(int id)
    {
        Controller = new TController();
        Controller.Model.InstanceId = id;
        Controller.Initialize(ModelChangedHandler);
    }

    protected virtual void ModelChangedHandler(PropertyChangedEventArgs e) { }

    public virtual void Show() {}
    public virtual void Hide() {}
    public virtual void Notify(BaseComponentEvent ev) {}
}