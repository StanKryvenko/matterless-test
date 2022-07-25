public class ItemTooltipView : View<ItemTooltipController, ItemTooltipModel>
{
    public TextComponent TooltipText;

    public override void Initialize(int id)
    {
        base.Initialize(id);
        
        TooltipText = new TextComponent(Controller.Model.InstanceId, this);
    }
}
