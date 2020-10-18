using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicUI : MonoBehaviour
{


    private Camera MainCamera;
    private RectTransform labelRect;

    //Coordonates
    private Vector3[] label_coordonates;
    private Vector3 labelCenter_coordonates;


    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        labelRect = this.GetComponent<RectTransform>();
        label_coordonates = new Vector3[4];
    }

    // Update is called once per frame
    void Update()
    {
        GetCoordonates();
        LabelFaceCamera();
    }

    void GetCoordonates() 
    {

        labelRect.GetWorldCorners(label_coordonates);

        //Get Label Center

        labelCenter_coordonates = Vector3.zero;

        for (var i = 0; i < 4; i++) 
        {
            labelCenter_coordonates.x = labelCenter_coordonates.x + label_coordonates[i].x;
            labelCenter_coordonates.y = labelCenter_coordonates.y + label_coordonates[i].y;
            labelCenter_coordonates.z = labelCenter_coordonates.z + label_coordonates[i].z;
        }

        labelCenter_coordonates.x = labelCenter_coordonates.x / 4;
        labelCenter_coordonates.y = labelCenter_coordonates.y / 4;
        labelCenter_coordonates.z = labelCenter_coordonates.z / 4;
    }

    void LabelFaceCamera() 
    {
        Vector3 TargetVector = labelCenter_coordonates - MainCamera.transform.position;
        Quaternion LookAt = Quaternion.LookRotation(TargetVector, MainCamera.transform.rotation * Vector3.up);

        labelRect.rotation = Quaternion.Euler(LookAt.eulerAngles.x, LookAt.eulerAngles.y, labelRect.rotation.z);
    }
}
