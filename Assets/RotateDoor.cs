using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Dondur();
    }
    public GameObject GO_ToRotate;
    public float givenValue = 0;
    void Dondur()
    {
        //GO_ToRotate.transform.RotateAround(Vector3.up, givenValue * Time.deltaTime);
    }
}
