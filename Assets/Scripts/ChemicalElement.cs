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

    void Start () 
    {
        elementdName.text = formula;
    }
}
