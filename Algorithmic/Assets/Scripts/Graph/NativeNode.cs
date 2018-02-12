using UnityEngine;
using System.Collections;

public class NativeNode : MonoBehaviour {
	SpriteRenderer spriteRender;
	Sprite[] sprites;
	int current;

	void Start()
	{
		spriteRender = GetComponent<SpriteRenderer>();
	}

	public void SetSprites(Sprite[] sprites)
	{
		this.sprites = sprites;
	}

	public void SetValue(int value)
	{
		this.current = value;
	}

	public void Reset()
	{
		spriteRender.sprite = sprites[current];
	}

	public void BlinkOn()
	{
		spriteRender.sprite = sprites[MapView.NODESEARCHED];
	}

	public void BlinkOff()
	{
		spriteRender.sprite = sprites[MapView.NODESEARCHING];
	}
}
