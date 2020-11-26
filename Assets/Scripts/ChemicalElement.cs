using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChemicalElement : MonoBehaviour
{
    public int id;
    public string nume;
    public string formula;
    public TextMeshProUGUI elementdName;
    public bool isDisplay;

    void Awake ()
    {
        if (isDisplay) 
        {
            this.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Start () 
    {
        elementdName.text = formula;
    }

    public void onDisplayGrabbed ()
    {
        if (isDisplay)
        {
            GameObject DisplayElement = GameObject.Instantiate(this.gameObject);
            DisplayElement.transform.position = this.transform.position;

            isDisplay = false;
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
      
    }
}
