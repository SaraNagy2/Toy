using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using static UnityEditor.PlayerSettings;

public class Card : MonoBehaviour
{
    public bool selected = false;


    public void OnClick(BaseEventData data)
    {
        SetSelected(MyController.instance.CardSelected(int.Parse(name), !selected));
    }

    public void SetSelected(bool sel)
    {
        selected = sel;
        GetComponent<Image>().color = selected ? new Color(.75f, .75f, .75f) : Color.white;
    }
}
