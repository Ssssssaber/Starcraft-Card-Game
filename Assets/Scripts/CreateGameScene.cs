using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CreateGameScene : MonoBehaviour
{
	
	public BoardPresetManager manager;
	public string activePresetName;
    void Start()
    {
		GameObject boardGO = GameObject.Find("board");
		if (boardGO == null)
		{
			var boardPrefab = Resources.Load<GameObject>("board");
			boardGO = GameObject.Instantiate(boardPrefab);
		}

		var boardAwareness = boardGO.GetComponent<BoardAwareness>();
		var preset = manager.GetPresetByName(activePresetName);
		if (preset != null)
		{
			boardAwareness.Init(preset);
		}
		else 
		{
			boardAwareness.Init(0, true, 0, true, "1;1;1;1;1;1;1;1;", "2;2;2;2;2;2;2;", false, false);
		}
    }

	
}
