using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MainUI : MonoBehaviour {

	public Text labelSearchDesc;
	public Text labelConsole;
	public Text startEndCoordLabel;
	TileType selectedTileType;

	public System.Action<TileType> onSelectedTile;
	public Toggle[] toggles;

	void Start()
	{
		for(int i = 0; i < toggles.Length; i++)
		{	
			Toggle toggle = toggles[i];
			toggle.onValueChanged.AddListener(delegate(bool selected){
				if(selected)ToogleSelcted(toggle);
			});
		}
	}

	void ToogleSelcted(Toggle toggle)
	{
		int value = toggle.GetComponent<DataComponent>().value1;
		if(onSelectedTile != null)onSelectedTile((TileType)value);
	}

	public void UpdateCoordLable(Point start, Point end)
	{
		startEndCoordLabel.text = "start : (" + start.x + ", " + start.y + "), end: (" + end.x + ", " + end.y + ")";
	}

	public string SearchConsoleTxt
	{
		set
		{
			labelConsole.text = value;
			if(!string.IsNullOrEmpty(value))StartCoroutine(HideSearchConsole());
		}
	}

	IEnumerator HideSearchConsole()
	{
		float totalTime = 0;
		while(totalTime < 1)
		{
			totalTime += Time.deltaTime;
			yield return null;
		}
		labelConsole.text = "";
	}
}

public enum TileType
{
	Grass,
	Obstacle,
	Start,
	End
}
