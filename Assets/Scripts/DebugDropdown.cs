using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


    public class DebugDropdown : MonoBehaviour
    {
        public TMP_Dropdown Dropdown;
        public Card Card;

        public void Awake()
        {
            EventManager.OnCardLoaded.AddListener(AddOption);
        }

        public void AddOption(CardStats cardStats)
        {
            Dropdown.AddOptions(new List<string>(){cardStats.Name});
        }

        // public void UpdateCard()
        // {
        //     Card.ApplyStats(CardStatsManager.instance.AllCards[Dropdown.value]);
        // }
        
        
    }