using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlacementPoint
{
    public Transform basePoint;
    public bool fire;
    public int panIndex;
}

public class KitchenStoveGenerator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float panYOffset = 0.5f;

    [Header("References")]
    [SerializeField]
    private PlacementPoint backLeft = new PlacementPoint();
    [SerializeField]
    private PlacementPoint backRight = new PlacementPoint();
    [SerializeField]
    private PlacementPoint frontLeft = new PlacementPoint();
    [SerializeField]
    private PlacementPoint frontRight = new PlacementPoint();
    [SerializeField]
    private GameObject firePrefab = null;
    [SerializeField]
    private GameObject[] pansPrefab = null;

    private bool initialized = false;

    private void Clean(PlacementPoint point)
    {
        point.basePoint.DestroyChilds();
    }

    public void Clean()
    {
        Clean(backLeft);
        Clean(backRight);
        Clean(frontLeft);
        Clean(frontRight);
    }

    private void Generate(PlacementPoint point)
    {
        if (point.basePoint == null) {
            Debug.LogException(new MissingReferenceException("You forgot to input the placement transform"));
        }
        if (point.fire) {
            GameObject.Instantiate(firePrefab, point.basePoint);
        }
        if (point.panIndex < -1 || point.panIndex > pansPrefab.Length) {
            Debug.LogException(new System.Exception("Out of bound index. Use -1 as none"));
        }
        if (point.panIndex != -1) {
            var pan = GameObject.Instantiate(pansPrefab[point.panIndex], point.basePoint);
            pan.transform.Translate(0, panYOffset, 0);
        }
    }

    public void Generate()
    {
        if (initialized) {
            Clean();
        }
        initialized = true;
        Generate(backLeft);
        Generate(backRight);
        Generate(frontLeft);
        Generate(frontRight);
    }
}
