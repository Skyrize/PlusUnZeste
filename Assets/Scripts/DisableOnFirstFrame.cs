using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnFirstFrame : MonoBehaviour
{
    private bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!done) {
            done = true;
            gameObject.SetActive(false);
        }
    }
}
