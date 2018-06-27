using UnityEngine;
using System.Collections;
using UnityEditor;

using Aptoide.AppcoinsUnity;

// Creates a custom Label on the inspector for all the scripts named ScriptName
// Make sure you have a ScriptName script in your
// project, else this will not work.
[CustomEditor(typeof(AppcoinsSku))]
public class AppCoinsProductEditor : Editor
{
    bool shouldAddToProductList = false;

    public override void OnInspectorGUI()
    {
        AppcoinsSku product = (AppcoinsSku)target;
        GUILayout.Label("The name of your in-app product");
        product.Name = EditorGUILayout.TextField("Product Name", product.Name);
        GUILayout.Label("The skuid of your in-app product");
        product.SKUID = EditorGUILayout.TextField("Product SKUID",product.SKUID);
        GUILayout.Label("Price in Appcoins currency(APPC)");
        product.Price = EditorGUILayout.DoubleField("Price", product.Price);
        GUILayout.Label("Should the product be added to the product list?");
        shouldAddToProductList = EditorGUILayout.Toggle("Add to list?", shouldAddToProductList);

        if (GUILayout.Button("Apply")) {
             //Rename the asset to have the same name as the product name

            //Get the path to the selected Scriptable object
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            string newName = product.Name + ".asset";

            AssetDatabase.RenameAsset(path, newName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            if (shouldAddToProductList) {
                AddProductToList(product);
            }
         }
    }

    void AddProductToList(AppcoinsSku product) {
        //Get the AppcoinsUnity script component
        AppcoinsUnity[] objs = FindObjectsOfType<AppcoinsUnity>();
        AppcoinsUnity appcoins = objs[0];

        //Check if product is in the list, if it isn't add it!
        bool found = false;
        foreach(AppcoinsSku existingProduct in appcoins.products) {
            if (existingProduct == product) {
                found = true;
                break;
            }
        }

        if (!found) {
            int length = appcoins.products.Length;

            AppcoinsSku[] newProducts = new AppcoinsSku[length + 1];
            int i = 0;
            foreach (AppcoinsSku existingProduct in appcoins.products)
            {
                newProducts[i++] = existingProduct;
            }
            newProducts[i] = product;
            appcoins.products = newProducts;    
        }

    }
}