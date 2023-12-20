using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_Text sunDisp;
    public int startingSunAmnt;
    public int SunAmount = 0;

    public Transform cardSlotsHolder;

    private void Start()
    {
        AddSun(startingSunAmnt);

        foreach (Transform card in cardSlotsHolder)
        {
            try
            {
                card.GetComponent<CardManager>().StartRefresh();
            }
            catch (System.Exception)
            {
                Debug.LogError("Card does not contain CardManager script!");
            }
        }
    }

    public void AddSun(int amnt)
    {
        SunAmount += amnt;
        sunDisp.text = "" + SunAmount;
    }

    public void DeductSun(int amnt)
    {
        SunAmount -= amnt;
        sunDisp.text = "" + SunAmount;
    }
}
