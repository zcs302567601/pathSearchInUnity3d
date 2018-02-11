using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct Point
{
	public int x;
	public int y;

	public Point(int x, int y)
	{
		this.x = x;
		this.y = y;
	}
}

public class MapView : MonoBehaviour {

	public const int GRASS = 0;
	public const int OBSTACLE = 1;
	public const int STARTPOINT = 2;
	public const int ENDPOINT = 3;
	public const int PATHNODE = 4;
	public int map_width = 40;
	public int map_height = 20;
	public const float tile_width = 0.32f;

	public SpriteRenderer spritePrefab;
	public Sprite[] sprites;

	[HideInInspector]
	public int[, ] tiles;		//0 green ; 1 obstacle; 2 start; 3 end; 4 path

	public SpriteRenderer[, ] tilesSprite;
	public NativeNode[, ] nativeNodes;
	public Point startPos = new Point(0, 0);
	public Point endPos = new Point(15, 15);

	public System.Action<Point, Point> onUpdateStartEndPos;
	public TileType selectedTileType = TileType.Grass;

	void Awake() {
		tiles = new int[map_width, map_height];
		tilesSprite = new SpriteRenderer[map_width, map_height];
		nativeNodes = new NativeNode[map_width, map_height];
		tiles[(int)startPos.x, (int)startPos.y] = 2;
		tiles[(int)endPos.x, (int)endPos.y] = 3;
	}

	void Start(){
		StartCoroutine(Draw());
	}

	IEnumerator Draw()
	{
		for(int i = 0; i < map_width; i++)
		{
			for(int j = 0; j < map_height; j++)
			{
				int value = tiles[i, j];
				var sprite = Instantiate(spritePrefab) as SpriteRenderer;
				sprite.sprite = sprites[value];
				sprite.transform.parent = this.transform;
				sprite.transform.localPosition = new Vector2(-6.4f + tile_width * 0.5f + i * tile_width, -3.2f + tile_width * 0.5f + j * tile_width);
				sprite.transform.localScale = Vector3.one;
				NativeNode nativeNode = sprite.GetComponent<NativeNode>();
				nativeNode.SetValue(value);
				nativeNode.SetSprites(sprites);
				nativeNodes[i, j] = nativeNode;
				tilesSprite[i, j] = sprite;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	void Update()
	{
		bool mouseLeft = Input.GetMouseButton(0);
		if(mouseLeft)
		{

			Vector3 screenPos = Input.mousePosition;
			Vector3 cameraPos = Camera.main.ScreenToWorldPoint(screenPos);
			if(Mathf.Abs(cameraPos.x) > 6.4f || Mathf.Abs(cameraPos.y) > 3.2f)
			{
				return;
			}
			int tile_x = (int)((cameraPos.x + 6.4f)/tile_width);
			int tile_y = (int)((cameraPos.y + 3.2f)/tile_width);

			// int oldTileValue = tiles[tile_x, tile_y];
			int newTileValue = 0;
			switch(selectedTileType)
			{
				case TileType.Grass:
					newTileValue = GRASS;
					break;
				case TileType.Obstacle:
					newTileValue = OBSTACLE;
					break;
				case TileType.Start:
					newTileValue = STARTPOINT;
					UpdateTile(startPos.x, startPos.y, 0);
					startPos.x = tile_x;
					startPos.y = tile_y;
					if(onUpdateStartEndPos != null)onUpdateStartEndPos(startPos, endPos);
					break;
				case TileType.End:
					newTileValue = ENDPOINT;
					UpdateTile(endPos.x , endPos.y, 0);
					endPos.x = tile_x;
					endPos.y = tile_y;
					if(onUpdateStartEndPos != null)onUpdateStartEndPos(startPos, endPos);
					break;
			}
			UpdateTile(tile_x, tile_y, newTileValue);
		}
	}

	public void UpdateTile(int tile_x, int tile_y, int value)
	{
		tiles[tile_x, tile_y] = value;
		tilesSprite[tile_x, tile_y].sprite = sprites[value];
		nativeNodes[tile_x, tile_y].GetComponent<NativeNode>().SetValue(value);
	}
}
