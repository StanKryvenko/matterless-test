using System.Numerics;

public class ItemTooltipController : BaseController<ItemTooltipModel>
{
    public ItemTooltipController()
    {
        GameManager.Instance.TickUpdate += Update;    
    }

    private void Update()
    {
        UpdateDistance();
        RotateToCamera();
    }

    private void RotateToCamera()
    {
        var cameraPos = GameManager.GetCameraPos();
        cameraPos.Y = GameManager.GetObjectPosById(Model.InstanceId).Y;
        GameManager.LookAt(Model.InstanceId, cameraPos);
    }
    
    public void UpdateDistance()
    {
        var distance = Vector3.Distance(GameManager.GetCameraPos(), GameManager.GetObjectPosById(Model.InstanceId));
        Model.Distance = distance;
    }
}