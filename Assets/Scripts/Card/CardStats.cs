using DefaultNamespace;
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;


    [System.Serializable]
    public class CardStats
    {
        public int id;
        // text data
        public string Name;
        public string Desc;

        // parameters
        public int ManaCost;
        public int Health;
        public int Attack;

        public Race Race;
        public CardType Type;
        public PlayStyle PlayStyle;
        
        public string PrintStats()
        {
            return Name + ": " + Desc + ManaCost + Health + Attack;
        }
    }