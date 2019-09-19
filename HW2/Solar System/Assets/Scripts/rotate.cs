using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
 
    }
    // Update is called once per frame
    void Update()
    {
        //Rotate为自转，RotateAround为公转
        this.transform.Rotate(Vector3.up * Time.deltaTime * 10);
        GameObject.Find("Mercury").transform.RotateAround(this.transform.position, new Vector3(0, 1, 0.5f), 60 * Time.deltaTime);
        GameObject.Find("Mercury").transform.Rotate(Vector3.up * Time.deltaTime * 10);
        GameObject.Find("Venus").transform.RotateAround(this.transform.position, new Vector3(0, 1, 0.3f), 50 * Time.deltaTime);
        GameObject.Find("Venus").transform.Rotate(Vector3.up * Time.deltaTime * 10);
        GameObject.Find("Earth").transform.RotateAround(this.transform.position, new Vector3(0, 1, 0.7f), 40 * Time.deltaTime);
        GameObject.Find("Earth").transform.Rotate(Vector3.up * Time.deltaTime * 10);
        GameObject.Find("Mars").transform.RotateAround(this.transform.position, new Vector3(0, 1, -0.1f), 30 * Time.deltaTime);
        GameObject.Find("Mars").transform.Rotate(Vector3.up * Time.deltaTime * 10);
        GameObject.Find("Jupiter").transform.RotateAround(this.transform.position, new Vector3(0, 1, -0.5f), 20 * Time.deltaTime);
        GameObject.Find("Jupiter").transform.Rotate(Vector3.up * Time.deltaTime * 10);
        GameObject.Find("Saturn").transform.RotateAround(this.transform.position, new Vector3(0, 1, -0.7f), 10 * Time.deltaTime);
        GameObject.Find("Saturn").transform.Rotate(Vector3.up * Time.deltaTime * 10);
        GameObject.Find("Uranus").transform.RotateAround(this.transform.position, new Vector3(0, 1, 0), 5 * Time.deltaTime);
        GameObject.Find("Uranus").transform.Rotate(Vector3.up * Time.deltaTime * 10);
        GameObject.Find("Neptune").transform.RotateAround(this.transform.position, new Vector3(0, 1, 0.9f), 3 * Time.deltaTime);
        GameObject.Find("Neptune").transform.Rotate(Vector3.up * Time.deltaTime * 10);
    }
}
