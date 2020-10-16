using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Label : MonoBehaviour
{
    //Gameobject towards Label is pointing
    public GameObject pointingTo;

    //Arrow Specifics
    public Material ArrowMaterial;
    public float width;


    private Camera MainCamera;
    private RectTransform labelRect;

    //Coordonates
    private Vector3[] label_coordonates;
    private Vector3 labelCenter_coordonates;
    private Vector3 pointingTo_coordonates;
    private Vector3 pointingFrom_coordonates;


    void DefineArrow() 
    {
        LineRenderer Arrow = gameObject.AddComponent<LineRenderer>();

        Arrow.material = ArrowMaterial;
        Arrow.widthMultiplier = width;
    }

    // Start is called before the first frame update
    void Start()
    {
        MainCamera = Camera.main;
        labelRect = this.GetComponent<RectTransform>();
        label_coordonates = new Vector3[4];

        DefineArrow();
    }

    // Update is called once per frame
    void Update()
    {
        GetCoordonates();
        DrawLabelArrow();
        LabelFaceCamera();
    }

    void GetCoordonates() 
    {
        pointingTo_coordonates = pointingTo.transform.position;

        labelRect.GetWorldCorners(label_coordonates);

        //Get Closest Label Corner

        float closestDistance = Vector3.Distance(label_coordonates[0], pointingTo_coordonates);
        pointingFrom_coordonates = label_coordonates[0];

        for (var i = 0; i < 4; i++) 
        {
            float distance = Vector3.Distance(label_coordonates[i], pointingTo_coordonates);

            if (distance < closestDistance) 
            {
                closestDistance = distance;
                pointingFrom_coordonates = label_coordonates[i];
            }
        }

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

    void DrawLabelArrow() 
    {
        LineRenderer Arrow = GetComponent<LineRenderer>();

        var points = new Vector3[2];
        points[0] = pointingFrom_coordonates;
        points[1] = pointingTo_coordonates;

        Arrow.SetPositions(points);
    }

    void LabelFaceCamera() 
    {
        Vector3 TargetVector = labelCenter_coordonates - MainCamera.transform.position;
        Quaternion LookAt = Quaternion.LookRotation(TargetVector, MainCamera.transform.rotation * Vector3.up);

        labelRect.rotation = Quaternion.Euler(LookAt.eulerAngles.x, LookAt.eulerAngles.y, labelRect.rotation.z);
    }
}
