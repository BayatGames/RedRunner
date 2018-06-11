//created by Lukmon Agboola(Codeberg)

using System.Collections;
using UnityEngine;

namespace Codeberg.AppcoinsUnity{

[CreateAssetMenu(fileName = "AppcoinsProduct", menuName = "Appcoins Product", order = 1)]
public class AppcoinsSku : ScriptableObject {
	[Header("The name of your in-app product")]
	public string Name;
	[Header("The skuid of your in-app product")]
	public string SKUID;
	[Header("Price in Appcoins currency(APPC)")]
	public double Price;
  }
}
