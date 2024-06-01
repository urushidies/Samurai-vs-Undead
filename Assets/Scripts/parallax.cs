using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Range(0f, 0.5f)]
    public float speed = 0.2f;
    private Material mat;

    private float offset;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset += Time.deltaTime * speed;
        mat.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}