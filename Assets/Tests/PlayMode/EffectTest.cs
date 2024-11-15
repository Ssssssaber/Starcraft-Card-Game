using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EffectTest
{
    [UnityTest]
    public IEnumerator EffectTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
		var presetGO = new GameObject("preset");
		var presetManager = presetGO.AddComponent<BoardPresetManager>();
		presetManager.LoadJsonConfigs();

		var boardPrefab = Resources.Load<GameObject>("board");
        var boardGO = GameObject.Instantiate(boardPrefab);
		var boardAwareness = boardGO.GetComponent<BoardAwareness>();
		// boardAwareness.Init(0, 0, "0;1;1;1;1;1;1;1;", "0;0;0;0;0;0;", false, false);
		GameUtilities.setActionWaitTime(0);
		boardAwareness.Init(presetManager.GetPresetByName("effectTest"));
			

		PlayerManager player = boardAwareness.player;
		PlayerManager opponent = boardAwareness.opponent;
		
		yield return new WaitForSeconds(1);
		

		var ts = boardAwareness.TurnSystem;
		for (int i = 0; i < 5; i++)
		{
			ts.EndPlayerTurn();
			yield return new WaitForSeconds(1);
			ts.EndOpponentTurn();
			yield return new WaitForSeconds(1);
		}

		Debug.Log("skipping turns... ");

		player.TryPlayCardById(0);
		Debug.Log("playing mutalisk");
		ts.EndPlayerTurn();
		yield return new WaitForSeconds(1);
		

		opponent.TryPlayCardById(0);
		Debug.Log("playing hydralisk");
		ts.EndOpponentTurn();
		yield return new WaitForSeconds(1);
		player.TryAttackTarget(0, 0);
		yield return new WaitForSeconds(1);
		
		Debug.Log("check if hydralisk is dead");
		
		Assert.AreEqual(opponent.Table.GetCardsList().Count == 0, true);

		yield return null;
    }
}
