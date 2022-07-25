
public class PrimitiveObjectView : View<PrimitiveObjectController, PrimitiveObjectModel>
{
    public override void Initialize(int id)
    {
        base.Initialize(id);
        
        GameManager.Instance.CreatePrefab<ItemTooltipView, ItemTooltipController, ItemTooltipModel>("ItemTooltip", id);
    }
}