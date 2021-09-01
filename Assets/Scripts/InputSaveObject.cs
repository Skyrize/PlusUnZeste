using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InputSave
{
    public string name;
    public KeyCode key;
}


[CreateAssetMenu(menuName = "InputSave")]
public class InputSaveObject : ScriptableObject
{
    public List<InputSave> inputs;
    
    public void Save()
    {
        foreach (var input in inputs)
        {
            PlayerPrefs.SetInt(input.name, (int)input.key);
        }
    }

    public void Load()
    {
        for (int i = 0; i != inputs.Count; i++) {
            InputSave save;
            save.name = inputs[i].name;
            save.key = (KeyCode)PlayerPrefs.GetInt(inputs[i].name, 0);
            this.inputs[i] = save;
        }
    }
    
    public KeyCode GetKey(string inputName)
    {
        var target = inputs.Find(elem => elem.name == inputName);

        if (target.key == KeyCode.None) {
            Debug.LogError($"Input {inputName} doens't exist or is set to none.");
        }
        return target.key;
    }

    public void ChangeKey(string inputName, KeyCode newKey)
    {
        for (int i = 0; i != inputs.Count; i++) {
            if (inputs[i].name == inputName) {
                InputSave s;
                s.name = inputName;
                s.key = newKey;
                PlayerPrefs.SetInt(inputName, (int)newKey);
                inputs[i] = s;
                return;
            }
        }
        Debug.LogError($"Input {inputName} doens't exist or is set to none.");
    }

}
