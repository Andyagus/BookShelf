using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatialAnchorManagerScene1_2 : MonoBehaviour
{
    private const string NumUuidsPlayerPrefs = "numUuids";
    public GameObject anchorPrefab;
    public Transform placementTransform;
    private OVRSpatialAnchor _workingAnchor;

    // private List<OVRSpatialAnchor> savedAnchors;
    private List<OVRSpatialAnchor> _runningAnchors;

    private Action<OVRSpatialAnchor.UnboundAnchor, bool> _onLoadAnchor;
    // [SerializeField] private bool addNewAnchor;

    [SerializeField] private bool allowEditAnchors;
    
    //only work if allow edit anchors is selected
    [SerializeField] private bool displayAnchors; 
    
    private void Awake()
    {
        // savedAnchors = new List<OVRSpatialAnchor>();
        _runningAnchors = new List<OVRSpatialAnchor>();
        _onLoadAnchor += OnLocalized;


        if (allowEditAnchors)
        {
            placementTransform.parent.gameObject.SetActive(true);
        }

        if (!allowEditAnchors && displayAnchors)
        {
            var anchorCount = PlayerPrefs.GetInt(NumUuidsPlayerPrefs);
            if (anchorCount == 0)
            {
                Debug.LogWarning("No Anchors, initializing allow edit anchor setup");
                allowEditAnchors = true;
                placementTransform.parent.gameObject.SetActive(true);
                return;
            }
            LoadSavedAnchors();
            Debug.Log($"Loading Saved Anchors Count is: {anchorCount}");
        }
    }

    private void Update()
    {
        //establish a new anchor
        if (allowEditAnchors)
        {
            if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            {
                GameObject go = PlaceAndInstantiateAnchor(anchorPrefab, placementTransform.position, placementTransform.rotation);
                _workingAnchor = go.AddComponent<OVRSpatialAnchor>();
                CreateAnchor(_workingAnchor);
            }
            
            if (OVRInput.GetDown(OVRInput.Button.Three))
            {
                HardEraseAnchors();
            }
            
            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                LoadSavedAnchors();
            }
        }
    }
    
    private GameObject PlaceAndInstantiateAnchor(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        return Instantiate(prefab, position, rotation);
    }

    private void CreateAnchor(OVRSpatialAnchor workingAnchor)
    {
        placementTransform.parent.gameObject.SetActive(false);
        StartCoroutine(IAnchorCreate(workingAnchor));
    }

    private IEnumerator IAnchorCreate(OVRSpatialAnchor anchor)
    {
        while (!anchor.Created && !anchor.Localized)
        {
            yield return new WaitForEndOfFrame();
        }
        anchor.Save((spatialAnchor, success) =>
        {
            if (!success) return;
            _runningAnchors.Add(anchor);

            SaveUuidToPlayerPrefs(anchor.Uuid);
        });
    }

    private void SaveUuidToPlayerPrefs(Guid uuid)
    {
        if (!PlayerPrefs.HasKey(NumUuidsPlayerPrefs))
        {
            PlayerPrefs.SetInt(NumUuidsPlayerPrefs, 0);
        }
        int playerNumUuids = PlayerPrefs.GetInt(NumUuidsPlayerPrefs);
        PlayerPrefs.SetString($"uuid{playerNumUuids}", uuid.ToString());
        PlayerPrefs.SetInt(NumUuidsPlayerPrefs, ++playerNumUuids);
    }

    private void LoadSavedAnchors()
    {
        OVRSpatialAnchor.LoadOptions options = new OVRSpatialAnchor.LoadOptions()
        {
            Timeout = 0,
            StorageLocation = OVRSpace.StorageLocation.Local,
            Uuids = GetSavedUUIDs()
        };
        OVRSpatialAnchor.LoadUnboundAnchors(options, anchorUUIDs =>
        {
            //this method does not accept anchor UUID's to be null,
            //but  not sure why its sending as null, in the debugger, it looks
            //like its sending an empty array
            //dont know exactly where to put the null check -- or why its returning null in GetSavedUUIDs Method
            //Not urgent because error is still allowing to run, and don't anticipate running app with no anchors
            if (anchorUUIDs.Length <= 0) return;
            foreach (var anchor in anchorUUIDs)
            {
                if (anchor.Localized)
                {
                    _onLoadAnchor(anchor, true);
                }else if (!anchor.Localizing)
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

        for (var i = 0; i < anchorCount; i++)
        {
            var uuidKey = $"uuid{i}";
            var currentUuid = PlayerPrefs.GetString(uuidKey);

            uuids[i] = new Guid(currentUuid);
        }
        
        return uuids;
    }

    private void HardEraseAnchors()
    {
        foreach (var anchor in _runningAnchors)
        {
            Destroy(anchor.gameObject);
        }

        var anchorCount = PlayerPrefs.GetInt(NumUuidsPlayerPrefs);
        for(var i = 0; i < anchorCount; i++)
        {
            var uuidKey = $"uuid{i}";
            var currentUUID = PlayerPrefs.GetString(uuidKey);
            PlayerPrefs.DeleteKey(currentUUID);
        }
        PlayerPrefs.DeleteKey(NumUuidsPlayerPrefs);
    }
    
    private void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool saveAnchor)
    {
        var pose = unboundAnchor.Pose;
        GameObject go = PlaceAndInstantiateAnchor(anchorPrefab, pose.position, pose.rotation);
        _workingAnchor = go.AddComponent<OVRSpatialAnchor>();
        unboundAnchor.BindTo(_workingAnchor);
        _runningAnchors.Add(_workingAnchor);
    }
}
