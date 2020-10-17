using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class condensedChemicalElement 
{
    public int id;
    public int quantity;
    public ChemicalElement Element;
}

public class InteractionAreaController : MonoBehaviour
{
    public ChemicalEquations EquationsData;
    public TextMeshProUGUI WhiteBoardFormula;
    public LayerMask ChemicalElementsLayer;

    private int lengh;
    private condensedChemicalElement[] containedElements = new condensedChemicalElement[100];

    // Update is called once per frame
    void Update()
    {
        updateWhiteBoard ();
    }

    void updateWhiteBoard () 
    {
        string reaction = "";

        for (int i = 0; i < lengh; i++) 
        {
            if (containedElements[i].quantity > 1) 
            {
                reaction = reaction + $"{containedElements[i].quantity}({containedElements[i].Element.formula})";
            }
            else 
            {
                reaction = reaction + $"{containedElements[i].Element.formula}";
            }
            
            if (i != lengh - 1) reaction = reaction + " + ";
        }

        WhiteBoardFormula.text = reaction;
    }

    void doReaction (int id)
    {

    }

    bool equivalentFormula (ReactionComponent[] reactanti) 
    {
        float ration = 0;
        int i = 0;
        int j = 0;

        for (i = 0; i < lengh; i++) 
        {
            bool OK = false;

            for (j = 0; j < reactanti.Length; j++) 
            {
                if (containedElements[i].id == reactanti[j].Element.GetComponent<ChemicalElement>().id) {
                    OK = true;
                    break;
                }
            }

            if (OK == false) return false;

            if (OK == true) 
            {
                float currentRatio = containedElements[i].quantity / reactanti[j].Coefficient;

                if (ration == 0) 
                {
                    ration = currentRatio;
                }
                else 
                {
                    if (ration != currentRatio) 
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }

    void attemptReaction () 
    {
        int i = 0;

        for (i = 0; i < EquationsData.Equations.Length; i++) 
        {
            if (equivalentFormula(EquationsData.Equations[0].reactanti)) 
            {
                doReaction(i);
                return;
            }
        }
    }

    void addChemicalElement (int id, ChemicalElement Element) 
    {
        int i = 0;

        for (i = 0; i < lengh; i++) 
        {
            if (containedElements[i].id == id) 
            {
                containedElements[i].quantity++;  
                return;  
            }
        }    

        containedElements[lengh] = new condensedChemicalElement();

        containedElements[lengh].id = id;
        containedElements[lengh].quantity = 1;
        containedElements[lengh].Element = Element;

        lengh++;
    }

    void removeChemicalElement (int id) 
    {
        int i = 0;

        for (i = 0; i < lengh; i++) 
        {
            if (containedElements[i].id == id) 
            {
                containedElements[i].quantity--;
                break;
            }
        }   

        if (containedElements[i].quantity == 0) 
        {
            for (int j = i; j < lengh - 1; j++)
            {
                containedElements[j] = containedElements[j + 1];
            }
            lengh--;
        }
    }

    void OnTriggerEnter(Collider obj) 
    {
        if (ChemicalElementsLayer == (ChemicalElementsLayer | (1 << obj.gameObject.layer)))
        {
            addChemicalElement (obj.gameObject.GetComponent<ChemicalElement>().id, obj.gameObject.GetComponent<ChemicalElement>());
        }
    }

    void OnTriggerExit(Collider obj) 
    {  
        if (ChemicalElementsLayer == (ChemicalElementsLayer | (1 << obj.gameObject.layer)))
        {
            removeChemicalElement (obj.gameObject.GetComponent<ChemicalElement>().id);
        }
    }
}
