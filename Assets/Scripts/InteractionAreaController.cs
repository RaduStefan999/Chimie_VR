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
    public Button react;
    public Button clear;

    public ChemicalEquations EquationsData;
    public TextMeshProUGUI WhiteBoardFormula;
    public LayerMask ChemicalElementsLayer;

    private bool justReacted;
    private int lengh;
    private condensedChemicalElement[] containedElements = new condensedChemicalElement[100];

    void Start() 
    {
        react.onClick.AddListener(() => attemptReaction());
        clear.onClick.AddListener(() => destroyReactanti());
    }

    // Update is called once per frame
    void Update()
    {
        if (!justReacted)
        {
            updateWhiteBoard ();
        }
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

    void setFormulaWhiteBoard (int id)
    {
        string reaction = "";

        ReactionComponent[] reactanti = EquationsData.Equations[id].reactanti;
        ReactionComponent[] produsi = EquationsData.Equations[id].produsi;

        for (int i = 0; i < reactanti.Length; i++) 
        {
            if (reactanti[i].Coefficient > 1) 
            {
                reaction = reaction + $"{reactanti[i].Coefficient}({reactanti[i].Element.GetComponent<ChemicalElement>().formula})";
            }
            else 
            {
                reaction = reaction + $"{reactanti[i].Element.GetComponent<ChemicalElement>().formula}";
            }
            
            if (i != reactanti.Length - 1) reaction = reaction + " + ";
        }

        reaction = reaction + " -> ";

        for (int i = 0; i < produsi.Length; i++) 
        {
            if (produsi[i].Coefficient > 1) 
            {
                reaction = reaction + $"{produsi[i].Coefficient}({produsi[i].Element.GetComponent<ChemicalElement>().formula})";
            }
            else 
            {
                reaction = reaction + $"{produsi[i].Element.GetComponent<ChemicalElement>().formula}";
            }
            
            if (i != produsi.Length - 1) reaction = reaction + " + ";
        }

        WhiteBoardFormula.text = reaction;
    } 

    void createProdusi (int id)
    {

    }

    void destroyReactanti ()
    {
        List<Collider> elementsInside = this.GetComponent<ColliderContainer>().GetColliders();

        foreach (Collider elementInside in elementsInside) 
        {
            if (ChemicalElementsLayer == (ChemicalElementsLayer | (1 << elementInside.gameObject.layer)))
            {
                Destroy(elementInside.gameObject);
            }
        }
    }

    void doReaction (int id)
    {
        justReacted = true;
        destroyReactanti();
        createProdusi(id);
        setFormulaWhiteBoard (id);
    }

    bool equivalentFormula (ReactionComponent[] reactanti) 
    {
        float ration = 0;
        int i = 0;
        int j = 0;

        if (reactanti.Length == 0 || lengh == 0) return false;
        if (reactanti.Length != lengh) return false;

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
                float currentRatio = (float)containedElements[i].quantity / reactanti[j].Coefficient;

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

    void addChemicalElement (int id, GameObject ElementRepresentation) 
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
        containedElements[lengh].Element = ElementRepresentation.GetComponent<ChemicalElement>();

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
            justReacted = false;
            addChemicalElement (obj.gameObject.GetComponent<ChemicalElement>().id, obj.gameObject);
        }
    }

    void OnTriggerExit(Collider obj) 
    {  
        if (ChemicalElementsLayer == (ChemicalElementsLayer | (1 << obj.gameObject.layer)))
        {
            justReacted = false;
            removeChemicalElement (obj.gameObject.GetComponent<ChemicalElement>().id);
        }
    }
}
