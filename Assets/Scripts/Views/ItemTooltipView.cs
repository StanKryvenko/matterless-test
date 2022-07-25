using System.ComponentModel;
using System.Numerics;

public class ItemTooltipView : View<ItemTooltipController, ItemTooltipModel>
{
    public TextComponent DistanceText;

    public override void Initialize(int id)
    {
        base.Initialize(id);
        
        DistanceText = new TextComponent(Controller.Model.InstanceId, this);
    }

    protected override void ModelChangedHandler(PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ItemTooltipModel.Distance))
        {
            var distance = Controller.Model.Distance < 1 ? Controller.Model.Distance :Controller.Model.Distance * 100;
            var distanceLabel = Controller.Model.Distance < 1 ? "cm" : "m";
            DistanceText.Text = $"{distance:0.00} {distanceLabel}";
        }
    }
}
