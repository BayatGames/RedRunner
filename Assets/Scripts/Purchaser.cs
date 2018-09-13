using UnityEngine;
using UnityEngine.UI;

/* add this namespace to your script to give you  access to
the plugin classes. */
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

    // This method will be called by AppcoinsUnity, in the Start method, located at ASFAppcoinsUnity prefab
    // prefab to register all the sku's
	public override void RegisterSKUs()
	{
		AddSKU(new AppcoinsSKU("Continue", "continue", 5));

    // OR (Create a SKU without a name (that is optional))
    // AddSKU(new AppcoinsSKU("continue", 5));
	}


	// Example methods to initiate a purchase flow
	// the string parameter of the makePurchase method is the skuid you
    // specified in the inspector for each product
	public void buyContinue()
	{
		Debug.Log("Going to call makePurchase with skuid continue");
		MakePurchase("continue");
	}
}