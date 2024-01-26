using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookshelfPrefabScene2 : MonoBehaviour
{
    public GameObject bookshelfMesh;
    public MeshRenderer bookshelfMeshRenderer;
    
    
    [SerializeField] private bool displayBookshelfModel;


    private void Awake()
    {
        bookshelfMeshRenderer = bookshelfMesh.GetComponent<MeshRenderer>();
    }
    
    void Start()
    {
        if (!displayBookshelfModel)
        {
            bookshelfMeshRenderer.enabled = false;
        }
        else
        {
            bookshelfMeshRenderer.enabled = true; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
