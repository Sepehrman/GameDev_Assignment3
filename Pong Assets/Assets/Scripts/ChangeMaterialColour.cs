using UnityEngine;

public class ChangeMaterialColor : MonoBehaviour
{
    public Material targetMaterial; // Assign the material you want to change in the Inspector

    void Start()
    {
        SetMaterialColor("red");
    }

    void SetMaterialColor(string colorName)
    {
        Color newColor = GetColorFromString(colorName);

        if (targetMaterial != null)
        {
            targetMaterial.color = newColor;
        }
    }

    Color GetColorFromString(string colorName)
    {
        switch (colorName.ToLower())
        {
            case "red":
                return Color.red;
            case "blue":
                return Color.blue;
            case "green":
                return Color.green;
            case "yellow":
                return Color.yellow;
            // Add more color mappings as needed.
            default:
                return Color.white; // Default to white if the color name is not recognized.
        }
    }
}
