public class PrimitiveObjectView : View<PrimitiveObjectController, PrimitiveObjectModel>
{
    public override void Initialize(int id)
    {
        base.Initialize(id);
        
        GameManager.CreatePrefab<DistanceTooltipView, ItemTooltipController, ItemTooltipModel>("ItemTooltip", id);
    }
}