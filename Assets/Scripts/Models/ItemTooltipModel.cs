public class ItemTooltipModel : BaseModel
{
    public float Distance
    {
        get => _distance;
        set => SetProperty(ref _distance, value);
    }

    private float _distance;
}