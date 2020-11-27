using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionChemicalElement 
{
    public int Coefficient;
    public ChemicalElement Data;
    public GameObject[] obj;
}

public class InteractionAreaContainer : MonoBehaviour 
{
    // Declarations

    public List<InteractionChemicalElement> GetElements () { return elements; }
    public float CheckEquivalentFormula (ChemicalEquation Equation) { return equivalentFormula(Equation); }
    public void destroyReactanti () { clearArea(); }

    // Variables

    public LayerMask ChemicalElementsLayer;
    private List<InteractionChemicalElement> elements = new List<InteractionChemicalElement>();

    // Mothodes

    private float equivalentFormula (ChemicalEquation Equation) 
    {
        float ratio = 0;
        ReactionComponent[] reactanti = Equation.reactanti;

        if (reactanti.Length == 0 || elements.Count == 0 || reactanti.Length != elements.Count) return 0;

        foreach (ReactionComponent reactant in reactanti)
        {
            if (elements.Exists(item => item.Data.id == reactant.id()))
            {
                InteractionChemicalElement element = elements.Find(item => item.Data.id == reactant.id());

                if (ratio == 0)
                {
                    ratio = (float)element.Coefficient / reactant.Coefficient;
                }
                else
                {
                    if (ratio != (float)element.Coefficient / reactant.Coefficient) return 0;
                }
            }
            else 
            {
                return 0;
            }
        }

        return ratio;
    }

    private void clearArea ()
    {
        foreach (InteractionChemicalElement element in elements)
        {
            for (int i = 0;  i < element.Coefficient; i++) Destroy(element.obj[i]);
        }
        elements.Clear();
    }

    private void removeElement (ChemicalElement elementData) 
    {
        if (elements.Exists(item => item.Data.id == elementData.id))
        {
            InteractionChemicalElement element = elements.Find(item => item.Data.id == elementData.id);

            element.Coefficient--;

            if (element.Coefficient == 0) elements.RemoveAll(item => item.Data.id == elementData.id);
        }
    }

    private void addElement (GameObject obj, ChemicalElement elementData) 
    {   
        if (elements.Exists(item => item.Data.id == elementData.id))
        {
            InteractionChemicalElement element = elements.Find(item => item.Data.id == elementData.id);

            element.Coefficient++;
            element.obj[element.Coefficient - 1] = obj;
        }
        else
        {
            InteractionChemicalElement element = new InteractionChemicalElement();
            element.obj = new GameObject[100];

            element.Coefficient = 1;
            element.Data = elementData;
            element.obj[element.Coefficient - 1] = obj;

            elements.Add(element);
        }
    }

    private void OnTriggerEnter (Collider obj) 
    {
        if (ChemicalElementsLayer == (ChemicalElementsLayer | (1 << obj.gameObject.layer)))
        { 
            addElement(obj.gameObject, obj.GetComponent<ChemicalElement>()); 
            this.GetComponent<InteractionAreaController>().contentChanged();
        }
    }

    private void OnTriggerExit (Collider obj) 
    {
        if (ChemicalElementsLayer == (ChemicalElementsLayer | (1 << obj.gameObject.layer)))
        {
            removeElement(obj.GetComponent<ChemicalElement>()); 
            this.GetComponent<InteractionAreaController>().contentChanged();
        }
    }

}