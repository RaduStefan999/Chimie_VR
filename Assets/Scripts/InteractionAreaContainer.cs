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
    public Vector3 GetFreeSpot () { return FreeSpot(); }

    // Variables

    public LayerMask ChemicalElementsLayer;
    public Transform SpawnArea;
    private List<InteractionChemicalElement> elements = new List<InteractionChemicalElement>();

    // Mothodes

    private Vector3 FreeSpot ()
    {
        Vector3 corner = new Vector3(SpawnArea.position.x + SpawnArea.localScale.x/2, SpawnArea.position.y - SpawnArea.localScale.y/2, SpawnArea.position.z + SpawnArea.localScale.z/2);

        for (int y = 0; y < SpawnArea.localScale.y; y++)
        {
            for (int z = 0; z < SpawnArea.localScale.z; z++)
            {
                for (int x = 0; x < SpawnArea.localScale.z; x++)
                {
                    Vector3 spot = new Vector3(corner.x - x, corner.y + y, corner.z - z);

                    bool Clear = true;

                    foreach (Collider obstacle in Physics.OverlapSphere(spot, 0.5f))
                    {
                        if (ChemicalElementsLayer == (ChemicalElementsLayer | (1 << obstacle.gameObject.layer)))
                        {
                            Clear = false;
                        }
                    }

                    if (Clear) return spot;
                }
            }
        }

        return corner;
    }

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