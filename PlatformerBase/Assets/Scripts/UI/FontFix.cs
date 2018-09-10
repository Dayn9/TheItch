using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// used to chnage the filter mode of pixel fonts to remove blurriness
/// </summary>
public class FontFix : MonoBehaviour {

	void Start () {
        //set the attached font's filtermode to point
        GetComponent<TextMesh>().font.material.mainTexture.filterMode = FilterMode.Point;
	}

}
