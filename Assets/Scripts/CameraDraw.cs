using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraDraw : MonoBehaviour
{
    [SerializeField]
    RenderTexture texture;
    [SerializeField]
    Camera cam = null;
    [SerializeField]
    Image text = null;
    // Start is called before the first frame update
    void Start()
    {
        if (!texture) {
            texture = new RenderTexture(1920, 1080, 24);
            text.color = Color.red;
        }
        if (!cam)
            text.color = Color.yellow;
        GetComponent<RawImage>().texture = texture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
