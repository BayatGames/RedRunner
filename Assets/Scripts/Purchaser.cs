using UnityEngine;
using UnityEngine.UI;

using Aptoide.AppcoinsUnity;
using RedRunner;

//Inherit from the AppcoinsPurchaser Class
public class Purchaser : AppcoinsPurchaser {
	public override void PurchaseSuccess (string skuid)
	{
		base.PurchaseSuccess (skuid);
		//purchase is successful release the product

		if(skuid.Equals("continue"))
		{
			Debug.Log(skuid + " was purchased successfully.");
			GameManager.Singleton.OnPurchaseSuccessful();
		}
	}

	public override void PurchaseFailure (string skuid)
	{
		base.PurchaseFailure (skuid);
		//purchase failed perhaps show some error message

		if(skuid.Equals("continue"))
		{
			Debug.Log(skuid + " purchase failed");
			GameManager.Singleton.OnPurchaseFailed();
		}
	}

	public override void RegisterSKUs()
	{
		AddSKU(new AppcoinsSKU("Continue", "continue", 0.1));
	}


	//example methods to initiate a purchase flow
	//the string parameter of the makePurchase method is the skuid you specified in the inspector for each product
	public void buyContinue()
	{
		Debug.Log("Going to call makePurchase with skuid continue");
		MakePurchase("continue");
	}
}
