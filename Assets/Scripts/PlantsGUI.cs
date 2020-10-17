using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlantsGUI : MonoBehaviour
{
    [SerializeField]
    public Button[] plantButons;
    
    [SerializeField]
    public TextMeshProUGUI DisplayedTitle;
    
    [SerializeField]
    public TextMeshProUGUI DisplayedText;
    
    [SerializeField]
    public RawImage DisplayedImage;

    public GameObject InfoArea;

    //public LessonInfo Info;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < plantButons.Length; i++) {

            int index = i;

            plantButons[i].onClick.AddListener(() => {

                //DisplayedTitle.text = Info.plantElements[index].Name;
                //DisplayedText.text = Info.plantElements[index].Information;
                //DisplayedImage.texture = Info.plantElements[index].Image;

                if (!InfoArea.activeSelf) {
                    InfoArea.SetActive(true);
                } 
            });
        }
    }
}
