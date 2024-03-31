using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class DeckTest1
{
    private string deckString = "MTszOzM7Mzs3Ozc7ODsxMDsxMTs=";

    private List<string> testNames = new List<string>();

    private bool added = false;
    private string[] resultNames = {"Zerg", "Protoss" };
    
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator DeckTestWithEnumeratorPasses()
    {
        // if (!added)
        // {
        //     EventManager.OnDatabaseCreated.AddListener(Check);
        //     added = true;
        // }
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;

        if (EventManager.IsDatabaseCreated)
        {
            var gameObject = new GameObject();
            var deck = gameObject.AddComponent<DeckBehaviour>();

            deck.CreateDeckFromString(deckString);

            foreach (var card in deck.Deck)
            {
                testNames.Add(card.Name);
            }

            Debug.Log(testNames);
            Assert.AreEqual(resultNames, testNames);
        }
    }

    private void Check()
    {
        var gameObject = new GameObject();
        var deck = gameObject.AddComponent<DeckBehaviour>();

        deck.CreateDeckFromString(deckString);

        foreach (var card in deck.Deck)
        {
            testNames.Add(card.Name);
        }
        Debug.Log(testNames);
        Assert.AreEqual(resultNames, testNames);
    }
}
