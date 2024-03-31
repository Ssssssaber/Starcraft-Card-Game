using System.Collections;
using System.Collections.Generic;
using MockObjects;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;





public class GOAPtest
{
    // A Test behaves as an ordinary method
    [Test]
    public void GOAPtestSimplePasses()
    {
        MockTableBehavior playerTable = new MockTableBehavior("player", Team.Player);
        MockTableBehavior opponentTable = new MockTableBehavior("opponent", Team.Opponent);
        
        MockCreatureCard murloc = new MockCreatureCard("Murloc", 1, 2, Team.Opponent);
        MockCreatureCard orc = new MockCreatureCard("Orc", 2, 3, Team.Opponent);
        MockCreatureCard mage = new MockCreatureCard("Mage", 1, 4, Team.Opponent);
        List<MockCreatureCard> pCards = new List<MockCreatureCard>() { murloc, orc, mage };
        
        MockCreatureCard zergling = new MockCreatureCard("Zergling", 1, 1, Team.Player);
        MockCreatureCard hydralisk = new MockCreatureCard("hydralisk", 3, 3, Team.Player);
        MockCreatureCard ultralisk = new MockCreatureCard("Ultralisk", 5, 3, Team.Player);
        List<MockCreatureCard> oCards = new List<MockCreatureCard>() { zergling, hydralisk, ultralisk };
        
        playerTable.SetCards(pCards);
        opponentTable.SetCards(oCards);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GOAPtestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
