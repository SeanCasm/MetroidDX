using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpdateKeyUI : MonoBehaviour
{
    [SerializeField] Input inputKey;

    private TextMeshProUGUI keyText;
    private void Awake()
    {
        keyText = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        Pause.OnPause -= Hide;
        GameEvents.OnInputBinded -= UpdateText;
        Pause.OnPause+=Hide;
        GameEvents.OnInputBinded+=UpdateText;
    }
    private void OnDestroy() {
        Pause.OnPause -= Hide;
        GameEvents.OnInputBinded -= UpdateText;
    }
    private void Hide(bool enable){
       gameObject.SetActive(enable);
    }
    private void UpdateText(string inputText, Input inputKey)
    {
        if (this.inputKey == inputKey)
        {
            switch (inputKey)
            {
                case Input.Select:
                    keyText.text = "[" + inputText + "] Select";
                    break;
                case Input.Back:
                    keyText.text = "[" + inputText + "] Back";
                    break;
            }
        }
    }
}
