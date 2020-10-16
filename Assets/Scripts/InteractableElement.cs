using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class InteractableElement 
{
    public GameObject element;
    public Material[] normalMaterials;
    public Material[] highLightMaterials;
    public GameObject description;

    public bool highLighted = false;
    public bool selected = false;


    public void select () {

    }

    public void highLight () {

    }

    public void removeHighLight () {
        
    }
}