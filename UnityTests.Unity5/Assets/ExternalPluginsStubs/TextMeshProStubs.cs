using UnityEngine;

namespace TMPro
{
    public class TMP_Text : MonoBehaviour
    {
        public Color color, outlineColor, faceColor;
        public Material fontMaterial, fontSharedMaterial;
        public float fontSize;
        public int maxVisibleCharacters;
        public string text;
    }

	public class TextMeshPro : TMP_Text
	{
		
	}

	public class TextMeshProUGUI : TMP_Text
	{
		
	}
}