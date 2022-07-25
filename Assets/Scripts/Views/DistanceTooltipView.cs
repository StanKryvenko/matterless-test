using System.ComponentModel;
using System.Numerics;

public class DistanceTooltipView : ItemTooltipView
{
    protected override void ModelChangedHandler(PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(ItemTooltipModel.Distance))
        {
            var distance = Controller.Model.Distance < 1 ? Controller.Model.Distance * 100 : Controller.Model.Distance;
            var distanceLabel = Controller.Model.Distance < 1 ? "сm" : "m";
            TooltipText.Text = $"{distance:0.00} {distanceLabel}";
        }
    }
}
