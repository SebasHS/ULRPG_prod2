using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWaterTextures : MonoBehaviour
{
    [SerializeField]
    private GameObject quad;

    [SerializeField]
    private Texture[] textures;

    private Renderer quadRenderer;

    private int randomTextureIndex;

    // Start is called before the first frame update
    void Start()
    {
        quadRenderer = quad.GetComponent<Renderer>();
        InvokeRepeating("ChangeQuadTexture",0.5f,0.5f);
    }

    // Update is called once per frame
    private void ChangeQuadTexture()
    {
        randomTextureIndex = Random.Range(0, textures.Length);
        quadRenderer.material.mainTexture = textures[randomTextureIndex];
    }
}
