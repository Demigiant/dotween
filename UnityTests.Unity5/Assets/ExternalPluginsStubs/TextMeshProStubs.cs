using UnityEngine;

namespace TMPro
{
	public class TextMeshPro : MonoBehaviour
	{
		public Color color, outlineColor, faceColor;
		public Material fontMaterial, fontSharedMaterial;
		public float fontSize;
		public int maxVisibleCharacters;
		public string text;
	}

	public class TextMeshProUGUI : MonoBehaviour
	{
		public Color color, outlineColor, faceColor;
		public Material fontMaterial, fontSharedMaterial;
		public float fontSize;
		public int maxVisibleCharacters;
		public string text;
	}
}