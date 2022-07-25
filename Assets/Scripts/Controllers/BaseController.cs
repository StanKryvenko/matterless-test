using System;

public abstract class BaseController<TModel> : IDisposable
    where TModel : BaseModel, new()
{
    public event PropertyChangedEventHandler OnModelChanged;
    public TModel Model { get; }

    public BaseController()
    {
        Model = new TModel();
    }

    public void Initialize(PropertyChangedEventHandler modelChangedHandler = default)
    {
        if (modelChangedHandler != default)
            Model.PropertyChanged += modelChangedHandler;
    }

    public void Dispose()
    {
        if (Model is IDisposable disposableModel)
            disposableModel.Dispose();
    }
}