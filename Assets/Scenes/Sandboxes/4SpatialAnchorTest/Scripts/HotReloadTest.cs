using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotReloadTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        GetComponent<MeshRenderer>().material.color = Color.blue;
        
        GetComponent<MeshRenderer>().material.color = Color.green;
        
        //rotate transform 5 degrees a second

        transform.Rotate(transform.position, 2f);
        
    }
}
