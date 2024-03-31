using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LoadCard : MonoBehaviour
{
    public TMP_InputField idInput;
    public TMP_InputField nameInput;
    public TMP_InputField descInput;

    public Card Card;

    public void GetCardsCount()
    {
        CardStatsManager.instance.CheckCardsCount();
    }

    public void UploadCard()
    {
        // CardStatsManager.instance.UploadCard(nameInput.text, descInput.text,  1, 1, 1, Race.Zerg, CardType.Creature);
    }

    // public void DownloadCard()
    // {
    //     CardStatsManager.instance.DownloadCard(Int32.Parse(idInput.text));
    // }
}
