using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UGUI_HP : MonoBehaviour
{
	private float HP = 0.0f;
	private float newHP = 0.0f;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		transform.rotation = Camera.main.transform.rotation; //保持面对摄像头
		GetComponent<Slider>().value = HP * 100;
	}

	void OnGUI()
	{
		if (GUI.Button(new Rect(900, 60, 100, 50), "Add", new GUIStyle("button") { fontSize = 15 }))
		{
			newHP = newHP + 0.01f;
			if (newHP > 1) newHP = 1f;
		}
		if (GUI.Button(new Rect(900, 120, 100, 50), "Sub", new GUIStyle("button") { fontSize = 15 }))
		{
			newHP = newHP - 0.01f;
			if (newHP < 0) newHP = 0f;
		}
		HP = Mathf.Lerp(HP, newHP, 0.01f);
	}
}
