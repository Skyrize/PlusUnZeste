using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Object = UnityEngine.Object;
using System.IO;

public class ExtractMaterialsWizard: ScriptableWizard {
    [MenuItem("MyTools/ExtractMaterials")]
    private static void MenuEntryCall() {
        DisplayWizard<ExtractMaterialsWizard>("Extract Materials Wizard", "Extract");
    }

    private void OnWizardCreate() {
        var models = AssetDatabase.FindAssets("t:model");
        List<string> tmp = new List<string>(); 

        foreach (var item in models)
        {
            string path = AssetDatabase.GUIDToAssetPath(item);
            if (path.Contains("Assets")) {
                ExtractMaterials(path, "Assets/Materials/");
            }
        }
    }
    
 public static void ExtractMaterials(string assetPath, string destinationPath)
 {
     HashSet<string> hashSet = new HashSet<string>();
     IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
                                         where x.GetType() == typeof(Material)
                                         select x;
     foreach (Object item in enumerable)
     {
         string path = System.IO.Path.Combine(destinationPath, item.name) + ".mat";
         path = AssetDatabase.GenerateUniqueAssetPath(path);
         string value = AssetDatabase.ExtractAsset(item, path);
         if (string.IsNullOrEmpty(value))
         {
             hashSet.Add(assetPath);
         }
     }
 
     foreach (string item2 in hashSet)
     {
         AssetDatabase.WriteImportSettingsIfDirty(item2);
         AssetDatabase.ImportAsset(item2, ImportAssetOptions.ForceUpdate);
     }
     Debug.Log("End for " + assetPath);
 }
}
