using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using MockObjects;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;



public class MockTable
{
    // A Test behaves as an ordinary method
    [Test]
    public void MockBehaviorTest()
    {
        // Создание объекта медиатора и столов
        Mediator mediator = new Mediator();
        // Use the Assert class to test conditions
        
        ProtossTableCreator protossTableCreator = new ProtossTableCreator();
        InterfaceObject playerTable = (ProtossTable) protossTableCreator.FactoryMethod();
        playerTable.SetMediator(mediator);
        
        ZergTableCreator zergTableCreator = new ZergTableCreator();
        InterfaceObject opponentTable = zergTableCreator.FactoryMethod();
        opponentTable.SetMediator(mediator);
        // Initializng mock cards
        MockCreatureCard murloc = new MockCreatureCard("Murloc", 1, 2, Team.Opponent);
        MockCreatureCard orc = new MockCreatureCard("Orc", 2, 3, Team.Opponent);
        MockCreatureCard mage = new MockCreatureCard("Mage", 1, 4, Team.Opponent);
        
        MockCreatureCard zergling = new MockCreatureCard("Zergling", 1, 1, Team.Player);
        MockCreatureCard hydralisk = new MockCreatureCard("hydralisk", 3, 3, Team.Player);
        MockCreatureCard ultralisk = new MockCreatureCard("Ultralisk", 5, 3, Team.Player);
        
        // 1st turn
        ultralisk.PlayCard(playerTable);
        
        murloc.PlayCard(opponentTable);
        orc.PlayCard(opponentTable);
       
        // 2nd
        playerTable.RefreshAttackAbility();
        ultralisk.DamageCard(murloc);
        
        opponentTable.RefreshAttackAbility();
        orc.DamageCard(ultralisk);
        
        // 3rd
        playerTable.RefreshAttackAbility();
        zergling.PlayCard(playerTable);
        hydralisk.PlayCard(playerTable);
        
        opponentTable.RefreshAttackAbility();
        mage.PlayCard(opponentTable);
        

        Debug.Log("Player Table CardsCount: " + playerTable.GetCards().Count + "; Opponent table CardsCount: " +
                  opponentTable.GetCards().Count);
        Assert.AreEqual((playerTable.GetCards().Count, opponentTable.GetCards().Count), (2, 1));
    }
}
