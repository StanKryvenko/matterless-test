using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Vector3 = System.Numerics.Vector3;

[RequireComponent(typeof(ARRaycastManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public event Action TickUpdate;

    public List<GameObject> PrefabsCollection = new ();
    private Dictionary<string, GameObject> _prefabsCollectionMap = new();
    
    private Dictionary<int, (IView view, GameObject instance)> _spawnedPrefabsMap = new();
    
    private ARRaycastManager _arRaycastManager;

    void Awake()
    {
        Instance = this;
        
        // Initialize cache for prefabs for faster access
        foreach (var prefab in PrefabsCollection)
        {
            if (_prefabsCollectionMap.ContainsKey(prefab.name))
            {
                Debug.LogError($"Prefab with name {prefab.name} already exists");
                continue;
            }
            _prefabsCollectionMap.Add(prefab.name, prefab);
        }
        
        _arRaycastManager = GetComponent<ARRaycastManager>();
        
        // Create main game world view
        CreatePrefab<GamespaceView, GamespaceController, GamespaceModel>("Empty");
    }
    
    private void Update() => TickUpdate?.Invoke();

#region Manage Views and Objects
    /// <summary> Create new prefab and dynamically set its unique number </summary>
    public static int CreatePrefab<TView, TController, TModel>(string prefabName, int parentId = 0)
        where TModel : BaseModel, new()
        where TController : BaseController<TModel>, new()
        where TView : View<TController, TModel>, new()
    {
        if (string.IsNullOrEmpty(prefabName)) return 0;

        Transform parentTransform = null;
        if (parentId != 0)
        {
            if (Instance._spawnedPrefabsMap.TryGetValue(parentId, out var parent))
                parentTransform = parent.instance.transform;
        }

        var newPrefab = prefabName switch
        {
            "Empty"  => new GameObject("Empty"),
            "Cube"   => GameObject.CreatePrimitive(PrimitiveType.Cube),
            "Sphere" => GameObject.CreatePrimitive(PrimitiveType.Sphere),
            _ => Instantiate(Instance._prefabsCollectionMap[prefabName])
        };
        newPrefab.transform.SetParent(parentTransform);
        
        var newView = new TView();

        var id = newPrefab.GetInstanceID();
        // Register newly created prefab and its view
        Instance._spawnedPrefabsMap.Add(id, (newView, newPrefab));
        
        newView.Initialize(id);
        return id;
    }
    
    public static void SetTextByComponentId(int id, string text)
    {
        if (!Instance._spawnedPrefabsMap.TryGetValue(id, out var map)) return;
        
        var textComp = map.instance.GetComponentInChildren<TMP_Text>();
        if (textComp != null)
            textComp.text = text;
    }
    
    public static Vector3 GetObjectPosById(int id)
    {
        if (!Instance._spawnedPrefabsMap.TryGetValue(id, out var obj)) return Vector3.Zero;

        var objectPos = obj.instance.transform.position;
        return new Vector3(objectPos.x, objectPos.y, objectPos.z);
    }
#endregion

#region API
    public static void SetObjectScaleById(int id, Vector3 scale) => Helpers.Physics.SetObjectScaleById(Instance._spawnedPrefabsMap, id, scale);
    public static void SetObjectColorById(int id, int r, int g, int b) => Helpers.Physics.SetObjectColorById(Instance._spawnedPrefabsMap, id, new Color(r / 255f, g / 255f, b / 255f));
    public static void SetObjectPosById(int primitiveId, Vector3 hitPoint) => Helpers.Physics.SetObjectPosById(Instance._spawnedPrefabsMap, primitiveId, hitPoint);
    public static bool ARRaycast(System.Numerics.Vector2 screenPoint, out Vector3 hitPoint) => Helpers.Physics.ARRaycast(screenPoint, out hitPoint, Instance._arRaycastManager);
    public static bool TryGetTouchPosition(out System.Numerics.Vector2 touchPosition) => Helpers.Input.TryGetTouchedPosition(out touchPosition);
    public static void LookAt(int id, Vector3 direction) => Helpers.Physics.LookAt(Instance._spawnedPrefabsMap, id, direction);
    public static Vector3 GetCameraPos()                 => Helpers.Physics.GetCameraPos();
#endregion
}
