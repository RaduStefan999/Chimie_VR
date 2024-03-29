﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InteractionAreaController : MonoBehaviour
{
    public Button react;
    public Button clear;

    public ChemicalEquations EquationsData;
    public TextMeshProUGUI WhiteBoardFormula;

    private int innactiveFrames;
    private InteractionAreaContainer helpers;

    public void contentChanged () { if (innactiveFrames >= 10) innactiveFrames = 0; }

    void Start() 
    {
        helpers = this.GetComponent<InteractionAreaContainer>();

        react.onClick.AddListener(() => attemptReaction());
        clear.onClick.AddListener(() => helpers.destroyReactanti());
    }

    void Update()
    {
        if (innactiveFrames == 0) updateWhiteBoard ();
        else if (innactiveFrames < 10) innactiveFrames++;
    }
    
    void createProdusi (int id, int Coefficient)
    {
        ReactionComponent[] produsi = EquationsData.Equations[id].produsi;

        for (int i = 0; i < produsi.Length; i++)
        {
            for (int j = 0; j < produsi[i].Coefficient * Coefficient; j++)
            {
                Instantiate(produsi[i].Element, helpers.GetFreeSpot(), Quaternion.Euler(0, 180, 0));
            }
        }
    }

    void doReaction (int id, int Coefficient)
    {
        helpers.destroyReactanti();
        innactiveFrames = 1;
        setFormulaWhiteBoard (id, Coefficient);
        createProdusi(id, Coefficient);
    }

    void attemptReaction () 
    {
        int i = 0;

        for (i = 0; i < EquationsData.Equations.Length; i++) 
        {
            int Coefficient = (int)helpers.CheckEquivalentFormula(EquationsData.Equations[i]);

            if (Coefficient != 0) 
            {
                doReaction(i, Coefficient);
                return;
            }
        }
    }


    // WhiteBoard

    void updateWhiteBoard () 
    {
        string reaction = "";

        List<InteractionChemicalElement> elements = helpers.GetElements();

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Coefficient > 1) 
            {
                reaction = reaction + $"{elements[i].Coefficient}({elements[i].Data.formula})";
            }
            else 
            {
                reaction = reaction + $"{elements[i].Data.formula}";
            }
            
            if (i != elements.Count - 1) reaction = reaction + " + ";
        }

        WhiteBoardFormula.text = reaction;
    }

    void setFormulaWhiteBoard (int id, int Coefficient)
    {
        string reaction = "";

        ReactionComponent[] reactanti = EquationsData.Equations[id].reactanti;
        ReactionComponent[] produsi = EquationsData.Equations[id].produsi;

        for (int i = 0; i < reactanti.Length; i++) 
        {
            if (reactanti[i].Coefficient * Coefficient > 1) 
            {
                reaction = reaction + $"{reactanti[i].Coefficient * Coefficient}({reactanti[i].Element.GetComponent<ChemicalElement>().formula})";
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
            if (produsi[i].Coefficient*Coefficient > 1) 
            {
                reaction = reaction + $"{produsi[i].Coefficient * Coefficient}({produsi[i].Element.GetComponent<ChemicalElement>().formula})";
            }
            else 
            {
                reaction = reaction + $"{produsi[i].Element.GetComponent<ChemicalElement>().formula}";
            }
            
            if (i != produsi.Length - 1) reaction = reaction + " + ";
        }

        WhiteBoardFormula.text = reaction;
    } 

}
