using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ObjectPortrait : MonoBehaviour
{
    public Vector3 offset;
    public Transform followTransform;
    public Camera my_camera;
    [HideInInspector]
    public RenderTexture rt;
    [HideInInspector]
    public RawImage my_image;

    void Awake()
    {
        rt = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        rt.Create();

        my_image = GetComponent<RawImage>();
        my_image.texture = rt;

        my_camera.targetTexture = rt;
    }

    void Update()
    {
        if(followTransform == null)
        {
            return;
        }

        my_camera.transform.position = followTransform.position + offset;
        my_camera.transform.LookAt(followTransform);
    }
}
