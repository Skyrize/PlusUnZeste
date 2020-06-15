using UnityEngine;
using UnityEditor;

public class MaterialExtractor: AssetPostprocessor {
    void OnPostprocessModel(GameObject g)
    {
        ModelImporter importer = assetImporter as ModelImporter;
        // Debug.Log("did it worked ? : " + importer.ExtractTextures("Assets/Materials/"));
        Debug.Log("Remap  :  " + importer.SearchAndRemapMaterials(ModelImporterMaterialName.BasedOnTextureName, ModelImporterMaterialSearch.RecursiveUp));
        // Debug.Log("Extract done for " + g.name);
    }

}