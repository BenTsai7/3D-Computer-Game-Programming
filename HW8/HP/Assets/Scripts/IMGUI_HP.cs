using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IMGUI_HP : MonoBehaviour
{
	private float HP = 0.0f;
	private float newHP = 0.0f;
    void OnGUI()
    {

		if (GUI.Button(new Rect(30, 60, 100, 50), "Add", new GUIStyle("button") { fontSize = 15 }))
		{
			newHP = newHP + 0.1f;
			if (newHP > 1) newHP = 1f;
		}
		if (GUI.Button(new Rect(30, 120, 100, 50), "Sub", new GUIStyle("button") { fontSize = 15 }))
		{
			newHP = newHP - 0.1f;
			if (newHP < 0) newHP = 0f;
		}
		HP = Mathf.Lerp(HP, newHP, 0.01f);
		GUI.HorizontalScrollbar(new Rect(30, 30, 200, 20), 0.0f,HP, 0.0f, 1.0f);
	}
}