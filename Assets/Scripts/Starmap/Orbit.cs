using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    [HideInInspector]
    public bool rotating = false;

    [HideInInspector]
	public float xSpread;
    [HideInInspector]
	public float zSpread;
    [HideInInspector]
	public float yOffset;
    [HideInInspector]
	public Transform centerPoint;

	public float rotSpeed;
    [HideInInspector]
    public float moveSpeed;
    [HideInInspector]
	public bool rotateClockwise;

	float timer = 0;

	// Update is called once per frame
	void Update()
    {
        if(!rotating)
        {
            return;
        }

		timer += Time.deltaTime * rotSpeed;
		Rotate();		
	}

	void Rotate()
    {
		if(rotateClockwise)
        {
			float x = -Mathf.Cos(timer) * xSpread;
			float z = Mathf.Sin(timer) * zSpread;
			Vector3 pos = new Vector3(x, yOffset, z);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, pos + centerPoint.localPosition, moveSpeed * Time.deltaTime);
		} 
        else
        {
			float x = Mathf.Cos(timer) * xSpread;
			float z = Mathf.Sin(timer) * zSpread;
			Vector3 pos = new Vector3(x, yOffset, z);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, pos + centerPoint.localPosition, moveSpeed * Time.deltaTime);
		}
	}
}
