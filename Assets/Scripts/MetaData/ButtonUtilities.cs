using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ButtonUtilities : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField]ButtonEssentials buttonEssentials;
    public Dictionary<int, Button> buttons { get; set; }
    public List<Button> itemButtons { get; set; }
    private void OnEnable() {
        Retry.Selected -= OnRetry;
        Retry.Selected +=OnRetry; 
        GameEvents.setItemButton+=SetButton;
    }
    void Awake()
    {
        buttons=new Dictionary<int, Button>();
        itemButtons=new List<Button>();
        for (int i = 0; i < 4; i++)//panels
        {
            for (int j = 0; j < menu.transform.GetChild(i).childCount; j++)//panel buttons
            {
                var panelChilds = menu.transform.GetChild(i).GetChild(j);
                int iD = panelChilds.GetComponent<ButtonID>().ID;
                Button button=panelChilds.GetComponent<Button>();
                buttons.Add(iD,button);
                itemButtons.Add(button);
            }
        }
    }
    private void OnRetry(){
        for(int i=0;i<buttons.Count;i++){
            buttons[i].gameObject.SetActive(false);
        }
    }
    public void SetButton(int iD, bool isSelected)
    {
        Button button = buttons[iD];
        button.gameObject.SetActive(true);
        if (isSelected)
        {
            button.image.color = buttonEssentials.selected;
            button.gameObject.GetChild(0).GetComponent<Image>().sprite = buttonEssentials.check;
            button.GetComponent<ButtonID>().selected = true;
        }
        else
        {
            button.image.color = buttonEssentials.unselected;
            button.gameObject.GetChild(0).GetComponent<Image>().sprite = buttonEssentials.uncheck;
            button.GetComponent<ButtonID>().selected = false;
        }
    }
    public Button GetButton(int iD)
    {
        if (buttons.ContainsKey(iD))
        {
            return buttons[iD];
        }
        else return null;
    }
}