using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KeySettingsController : MonoBehaviour
{
    public string keyName;

    [SerializeField] private TextMeshProUGUI keyNameText;
    [SerializeField] private TextMeshProUGUI keyCodeText;

    public void ChangeName(string s)
    {
        keyName = s;
        keyNameText.text = s;
    }

    public void ChangeKey(string keyCode)
    {
        keyCodeText.text = keyCode;
    }

    public void OnClick()
    {
        SettingsManager.Instance.ChangeKey(keyName, this);
    }
}
