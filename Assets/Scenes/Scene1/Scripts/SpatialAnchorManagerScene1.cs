using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class SpatialAnchorManagerScene1 : MonoBehaviour
{
    private const string NumUuidsPlayerPrefs = "numUuids";
    
    public GameObject spherePrefab;
    public Transform saveableTransform;
    private OVRSpatialAnchor _workingAnchor;

    private List<OVRSpatialAnchor> _savedAnchors;
    private List<OVRSpatialAnchor> _runningAnchors;
    
    private Action<OVRSpatialAnchor.UnboundAnchor, bool> _onLoadAnchor;

    [SerializeField] private bool addNewAnchor;
    
    private void Awake()
    {
        _savedAnchors = new List<OVRSpatialAnchor>();
        _runningAnchors = new List<OVRSpatialAnchor>();
        _onLoadAnchor = OnLocalized;

        if (addNewAnchor)
        {
            saveableTransform.parent.gameObject.SetActive(true);
        }
        else
        {
            LoadAllAnchors();
        }
    }

    private void Update()
    {
        if (addNewAnchor)
        {
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                var gs = PlaceInstantiateAnchor(spherePrefab, saveableTransform.position, saveableTransform.rotation);
                _workingAnchor = gs.AddComponent<OVRSpatialAnchor>();
                CreateAnchor(_workingAnchor);
            }

            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                LoadAllAnchors();
            }
            
            if (OVRInput.GetDown(OVRInput.Button.Three))
            {
                foreach (var anchor in _runningAnchors)
                {
                    Destroy(anchor.gameObject);
                }
                _runningAnchors.Clear();

                HardEraseAnchors();
            }
        }
    }
    
    
    private void CreateAnchor(OVRSpatialAnchor workingAnchor)
    {
        StartCoroutine(AnchorCreated(workingAnchor));
    }

    private IEnumerator AnchorCreated(OVRSpatialAnchor anchor)
    {
        while (!anchor.Created && !anchor.Localized)
        {
            yield return new WaitForEndOfFrame();
        }
        
        
        _runningAnchors.Add(anchor);
        
        anchor.Save((spatialAnchor, success) =>
        {
            if (!success) return;
            
            _savedAnchors.Add(anchor);
            SaveUuidToPlayerPrefs(anchor.Uuid);
        });
    }

    private void SaveUuidToPlayerPrefs(Guid uuid)
    {
        //num uuids persists accross sessions so you can accuratly
        //keep track of how many uuids you have
        
        if (!PlayerPrefs.HasKey(NumUuidsPlayerPrefs))
        {
            PlayerPrefs.SetInt(NumUuidsPlayerPrefs, 0);
        }
        int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPrefs);
        //playerNumUUIDs is just one way to give each uuid a unique key,
        //but it keeps them in order across sessions, so its a great solution
        PlayerPrefs.SetString($"uuid{playerNumUuids}", uuid.ToString());
        PlayerPrefs.SetInt(NumUuidsPlayerPrefs, ++playerNumUuids);
    }
    
    private void LoadAllAnchors()
    {
        OVRSpatialAnchor.LoadOptions options = new OVRSpatialAnchor.LoadOptions()
        {
            Timeout = 0,
            StorageLocation = OVRSpace.StorageLocation.Local,
            Uuids = GetSavedUUIDs()
        };

        OVRSpatialAnchor.LoadUnboundAnchors(options, anchorUUIDs =>
        {
            foreach (var anchor in anchorUUIDs)
            {
                if (anchor.Localized)
                {
                    _onLoadAnchor(anchor, true);
                }
                else if (!anchor.Localizing)
                {
                    anchor.Localize(_onLoadAnchor);
                }
                
            }
        });
    }

    private System.Guid[] GetSavedUUIDs()
    {
        var anchorCount = PlayerPrefs.GetInt(NumUuidsPlayerPrefs);
        var uuids = new Guid[anchorCount];

        for (var i = 0; i < uuids.Length; i++)
        {
            var uuidKey = "uuid" + i;
            var currentUuid = PlayerPrefs.GetString(uuidKey);

            uuids[i] = new Guid(currentUuid);
        }
        
        return uuids;
    }

    private void HardEraseAnchors()
    {

        var savedAnchorCount = PlayerPrefs.GetInt(NumUuidsPlayerPrefs);
        
        for (var i = 0; i <savedAnchorCount; i++)
        {
            PlayerPrefs.DeleteKey("uuid" + i);
            PlayerPrefs.DeleteKey(NumUuidsPlayerPrefs);
        }
        
        foreach (var anchor in _savedAnchors)
        {
            StartCoroutine(EraseAnchors(anchor));
        }
    }

    private IEnumerator EraseAnchors(OVRSpatialAnchor anchor)
    {
        while (!anchor.Created)
        {
            yield return new WaitForEndOfFrame();
        }
        
        anchor.Erase(((spatialAnchor, success) =>
        {
            if (!success) return;
            
            
        }));
    }
    
    private GameObject PlaceInstantiateAnchor(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var go = Instantiate(prefab, position, rotation);
        return go;
    }

    private void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool saveAnchor)
    {
        var pose = unboundAnchor.Pose;
        GameObject go = PlaceInstantiateAnchor(spherePrefab, pose.position, pose.rotation);
        _workingAnchor = go.AddComponent<OVRSpatialAnchor>();
        unboundAnchor.BindTo(_workingAnchor);
        _runningAnchors.Add(_workingAnchor);
    }
}
