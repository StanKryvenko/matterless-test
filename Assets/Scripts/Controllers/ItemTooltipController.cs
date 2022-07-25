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
        var cameraPos = GameManager.Instance.GetCameraPos();
        cameraPos.Y = GameManager.Instance.GetObjectPosById(Model.InstanceId).Y;
        GameManager.Instance.LookAt(Model.InstanceId, cameraPos);
    }
    
    public void UpdateDistance()
    {
        var distance = Vector3.Distance(GameManager.Instance.GetCameraPos(), GameManager.Instance.GetObjectPosById(Model.InstanceId));
        Model.Distance = distance;
    }
}