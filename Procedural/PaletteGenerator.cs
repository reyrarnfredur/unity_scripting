using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PaletteGenerator : MonoBehaviour
{
    public Material[] objectMaterials;
    public Material[] terrainMaterials;

    public Texture2D[] paletteList;
    public Texture2D terrainPalette;
    public Texture2D objectPalette;

    public Color[] terrainColors;
    public Color[] objectColors;
    public int maxColors = 6;

    [Button]
    public void Generate(){

        terrainPalette = paletteList[Random.Range(0, paletteList.Length-1)];
        objectPalette = paletteList[Random.Range(0, paletteList.Length-1)];

        terrainColors = GetSampledColors( terrainPalette );
        objectColors = GetSampledColors( objectPalette );

        ColorizeMaterials( objectMaterials, objectColors );
        ColorizeMaterials( terrainMaterials, terrainColors );

    }

    private Color[] GetSampledColors(Texture2D image){
        
        Color[] colors = new Color[maxColors];
        Debug.Log( "Width is: " + image.width );
        Debug.Log( "Height is: " + image.height );

        int x = image.width - 1;
        int y = 0;
        int i = 0;

        while (y < image.height) {
            while (x > -1) {
                colors[i] = image.GetPixel( x, y );
                i++;
                x--;
            }
            x = image.width - 1;
            y++;
        }

        return colors;

    }

    private void ColorizeMaterials(Material[] materials, Color[] colors){
        for (int i = 0; i < materials.Length; i++){
            materials[i].color = colors[i];
        }
    }


}
