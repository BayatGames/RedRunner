using UnityEngine;
using System.Collections;
using UnityEditor;

using Aptoide.AppcoinsUnity;

public class ProductMaker
{
    public static string DEFAULT_APPCOINS_PRODUCT_NAME = "AppCoinsProduct";

    [MenuItem("Assets/Create/AppCoins Product")]
    public static void CreateMyAsset()
    {
        AppcoinsSku asset = ScriptableObject.CreateInstance<AppcoinsSku>();

        if (!AssetDatabase.IsValidFolder("Assets/Products")) {
            string guid = AssetDatabase.CreateFolder("Assets", "Products");    
        }

        AssetDatabase.CreateAsset(asset, "Assets/Products/" + DEFAULT_APPCOINS_PRODUCT_NAME + ".asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}