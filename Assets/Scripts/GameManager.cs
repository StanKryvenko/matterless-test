using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Vector3 = System.Numerics.Vector3;

[RequireComponent(typeof(ARRaycastManager))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public event Action TickUpdate;

    public List<GameObject> PrefabsCollection = new ();
    private Dictionary<string, GameObject> _prefabsCollectionMap = new();
    
    private Dictionary<int, (IView view, GameObject instance)> _spawnedPrefabsMap = new();

    private GameObject _spawnedObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 _touchPosition;
    
    private static List<ARRaycastHit> _hits = new();

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
    }

#region Manage Views and Objects
    /// <summary> Create new prefab and dynamically set its unique number </summary>
    public View<TController, TModel> CreatePrefab<TView, TController, TModel>(string prefabName, int parentId = 0)
        where TModel : BaseModel, new()
        where TController : BaseController<TModel>, new()
        where TView : View<TController, TModel>, new()
    {
        if (string.IsNullOrEmpty(prefabName)) return null;

        Transform parentTransform = null;
        if (parentId != 0)
        {
            if (_spawnedPrefabsMap.TryGetValue(parentId, out var parent))
                parentTransform = parent.instance.transform;
        }

        var newPrefab = Instantiate(_prefabsCollectionMap[prefabName], parentTransform);
        var newView = new TView();
        
        // Register newly created prefab and its view
        _spawnedPrefabsMap.Add(newPrefab.GetInstanceID(), (newView, newPrefab));
        
        newView.Initialize(newPrefab.GetInstanceID());
        return newView;
    }
    
    public void SetTextByComponentId(int id, string text)
    {
        if (!_spawnedPrefabsMap.TryGetValue(id, out var map)) return;
        
        var textComp = map.instance.GetComponentInChildren<TMP_Text>();
        if (textComp != null)
            textComp.text = text;
    }
    
    public Vector3 GetObjectPosById(int id)
    {
        if (!_spawnedPrefabsMap.TryGetValue(id, out var obj)) return Vector3.Zero;

        var objectPos = obj.instance.transform.position;
        return new Vector3(objectPos.x, objectPos.y, objectPos.z);
    }
#endregion

#region API
    public void LookAt(int id, Vector3 direction) => Helpers.Physics.LookAt(_spawnedPrefabsMap, id, direction);
    public Vector3 GetCameraPos()                 => Helpers.Physics.GetCameraPos();
#endregion

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;
            touchPosition = new Vector2(mousePos.x, mousePos.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif
        touchPosition = default;
        return false;
    }

    private void Update()
    {
        TickUpdate?.Invoke();
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

#if UNITY_EDITOR
        // Get raycast hit on plane object
        if (Physics.Raycast(Camera.main.ScreenPointToRay(touchPosition), out var hit))
        {
            var hitPose = new Pose(hit.point, Quaternion.identity);
#else
        if (_arRaycastManager.Raycast(touchPosition, _hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = _hits[0].pose;
#endif
            if (_spawnedObject == null)
            {
                _spawnedObject = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), hitPose.position, hitPose.rotation);
                _spawnedPrefabsMap[_spawnedObject.GetInstanceID()] = (null, _spawnedObject);
                
                CreatePrefab<PrimitiveObjectView, PrimitiveObjectController, PrimitiveObjectModel>("Empty", _spawnedObject.GetInstanceID());
            }
            else
            {
                _spawnedObject.transform.position = hitPose.position;
            }
        }
    }
}
