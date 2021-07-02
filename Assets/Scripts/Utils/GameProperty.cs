using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameProperty : MonoBehaviour
{
    public enum Property
    {
        EJECTABLE,
        FLOAT
    }

    [SerializeField]
    private List<Property> properties = new List<Property>();
    public bool HasProperty(Property property)
    {
        return properties.Contains(property);
    }
}
