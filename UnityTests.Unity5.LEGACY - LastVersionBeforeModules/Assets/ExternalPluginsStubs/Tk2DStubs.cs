using UnityEngine;

public class tk2dBaseSprite : MonoBehaviour
{
	public Vector3 scale;
	public Color color;
}

public class tk2dSlicedSprite : tk2dBaseSprite
{
	public Vector2 dimensions;
}

public class tk2dTextMesh : MonoBehaviour
{
	public Vector3 scale;
	public Color color;
	public string text;
}