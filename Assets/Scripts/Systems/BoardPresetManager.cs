using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardPresetManager : MonoBehaviour
{
	public List<BoardPreset> presetsList = new List<BoardPreset>();
	private Dictionary<string, BoardPreset> presetsDict = new Dictionary<string, BoardPreset>();
	public bool reload = false;
	private bool added = false;
	private void OnValidate()
	{
		if (reload) 
		{
			presetsDict = new Dictionary<string, BoardPreset>();
			presetsList = new List<BoardPreset>();
			LoadJsonConfigs();
		}
	}

	public BoardPreset GetPresetByName(string name)
	{
		if (presetsDict.ContainsKey(name))
		{
			return presetsDict[name];
		}
		else
		{
			return null;
		}
	}
    public void LoadJsonConfigs()
	{
		var boardPresets = Resources.LoadAll<TextAsset>("BoardPresets");
		foreach (var preset in boardPresets)
		{
			var readyPreset = JsonUtility.FromJson<BoardPreset>(preset.ToString());
			if (presetsDict.ContainsKey(readyPreset.Name)) continue;
			presetsDict.Add(readyPreset.Name, readyPreset);
			added = true;
		}
		if (added)
		{
			presetsList = presetsDict.Values.ToList();
		}

	}
}
