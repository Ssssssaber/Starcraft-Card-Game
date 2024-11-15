using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BoardPreset
{
	public string Name;
   	public int playerHeroId;
	public bool playerGOAP;
	public string playerDeckCode;
	public int opponentHeroId;
	public bool opponentGOAP;
	public string opponentDeckCode;
	public bool encoded;
}
