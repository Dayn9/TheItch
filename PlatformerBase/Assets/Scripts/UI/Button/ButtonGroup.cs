using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGroup : MonoBehaviour {

    /// <summary>
    /// allows for disabling and enabling groups of buttons
    /// scrolling through selection with button presses rather than mouse clicks
    /// </summary>

    private Button[] buttons;

	// Use this for initialization
	void Awake () {
        buttons = GetComponentsInChildren<Button>();
	}
	
}
