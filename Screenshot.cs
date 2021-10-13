using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Screenshot
{

    List<string> screenshots = new List<string>();

    int n = 0;
    private IEnumerator TakeScreenshot(){
        
        //disable UI here
        //~

        yield return new WaitForEndOfFrame();

        //create texture, read pixels from screen and apply them in the texture
        Texture2D capture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        capture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        capture.Apply();

        //add string name + number to screenshots list
        n++;
        string filename = "photo_" + n;
        screenshots.AddNew(filename);

        Debug.Log("Photo " + filename + " has been taken. " + photoMemory.imagesInMemory.Count + " in total.");
        
        //enable UI here
        //~

        StopCoroutine(TakeScreenshot());
    }

    private void OnTakeScreenshot() => StartCoroutine(TakeScreenshot());
    
}