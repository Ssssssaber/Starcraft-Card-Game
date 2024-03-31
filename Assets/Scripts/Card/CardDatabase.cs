using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
// using DefaultNamespace;


[System.Serializable]
public class CardDatabase : MonoBehaviour
{
    public static List<CardStats> CardsList = new List<CardStats>();
    public static List<CardStats> ZergCards = new List<CardStats>();
    public static List<CardStats> ProtossCards = new List<CardStats>();
    [SerializeField] private CardStats[] _allCards;
    
    [SerializeField] private CardStats[] _allZergCards;
    [SerializeField] private CardStats[] _allProtossCards;
    
    public static Dictionary<int, CardStats> CardsDict = new Dictionary<int, CardStats>();
    public static Dictionary<int, string> CardNamesDict = new Dictionary<int, string>();
    public static bool DatabaseCreated = false;
    
    
    private void Start()
    {
        EventManager.OnAllCardsLoaded.AddListener(CreateDatabase);
    }
    
    private void CreateDatabase(List<CardStats> loadedCards)
    {
        _allCards = loadedCards.ToArray();
        GiveIds();
        
        _allProtossCards = _allCards.Where(i => i.Race == Race.Protoss).ToArray();
        _allZergCards = _allCards.Where(i => i.Race == Race.Zerg).ToArray();
        CardsList = new List<CardStats>(_allCards);
        ZergCards = new List<CardStats>(_allZergCards);
        ProtossCards = new List<CardStats>(_allProtossCards);
        string res = "{";
        foreach (var item in CardNamesDict)
        {
            res += "{" + item.Key + ", " + item.Value + "}, ";
        }

        res += "}";
        // Debug.Log(res);
        EventManager.DatabaseCreated();
    }
    //
    private void GiveIds()
    {
        if (!DatabaseCreated)
        {
            foreach (var cardStats in _allCards)
            {
                CardsDict.Add(cardStats.id, cardStats);
                CardNamesDict.Add(cardStats.id, cardStats.Name);
            }
            DatabaseCreated = true;
        }
    }
}

