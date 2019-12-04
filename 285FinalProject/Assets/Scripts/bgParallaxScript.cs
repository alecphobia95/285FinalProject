using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bgParallaxScript : MonoBehaviour
{
    public GameObject planeGameObject;
    private Renderer planeRenderer;

    public float scrollSpeed;

    void Start()
    {
        planeRenderer = planeGameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        Vector2 textureOffset = new Vector2(Time.time * scrollSpeed, 0);
        planeRenderer.material.mainTextureOffset = textureOffset;
    }
}

