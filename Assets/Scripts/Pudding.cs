using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pudding : MonoBehaviour
{
    public float toggleDuration = 1f;
    IEnumerator ToggleCollision()
    {
        GetComponent<MeshCollider>().enabled = false;
        yield return new WaitForSeconds(toggleDuration);
        GetComponent<MeshCollider>().enabled = true;
    }

    public void MakePlayerJump(GameObject target)
    {
        if (target.tag != "Player")
            return;
        target.GetComponent<EventBinder>().CallEvent("Fly");
        target.GetComponent<PlayerController>().IsBumped = true;
        StartCoroutine(ToggleCollision());
    }
}
