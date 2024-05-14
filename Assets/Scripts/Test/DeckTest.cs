using System;
using System.Collections.Generic;
using UnityEngine;
namespace DefaultNamespace.Test
{
    public class DeckTest : MonoBehaviour
    {
        private DeckBehaviour deck;
        private string deckString = "MTszOzM7Mzs3Ozc7ODsxMDsxMTs=";
        private List<string> testNames = new List<string>();
        private void Awake()
        {
            // var methods = typeof(CardCostCalculation).GetMethods();
            // Debug.Log(methods[0] + " " + methods[1]);
            // var onCalc= typeof(CardCostCalculation).GetMethod("Transfuse");
            // if (onCalc != null)
            // {
            //     Debug.Log("return extra value");
            // }
            // var gameObject = new GameObject();
            // deck = gameObject.AddComponent<DeckBehaviour>();
            // EventManager.OnDatabaseCreated.AddListener(Check);
            // EventManager.SystemChanged.AddListener(dbg);
        }

        private void dbg()
        {
            Debug.Log("System changed");
        }

        private void Check()
        {
            deck.CreateDeckFromString(deckString);
            foreach (var card in deck.Deck)
            {
                testNames.Add(card.Name);
            }

            Debug.Log(testNames);
        }
    }
}