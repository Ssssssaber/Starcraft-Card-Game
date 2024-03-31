using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    [System.Serializable]
    public class CardStatsTest
    {
        public int id;
        // text data
        public string Name;
        public string Desc;

        // public string ImagePath;
        //
        // // parameters
        // public int ManaCost;
        // public int Health;
        // public int Damage;

        // public Race Race;

        // public CardType Type;

        public CardStatsTest(int id, string name, string desc)
        {
            this.id = id;
            this.Name = name;
            this.Desc = desc;
        }

        public void PrintStats()
        {
            Debug.Log(Name + " :: " + Desc);
        }
    }
}