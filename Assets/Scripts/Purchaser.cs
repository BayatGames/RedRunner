//add this namespace to your script to give you  access to the plugin classes.
using Aptoide.AppcoinsUnity;
using RedRunner;
using UnityEngine;

public class Purchaser : AppcoinsPurchaser
{

    //method gets called on successful purchases
    public override void purchaseSuccess(string skuid)
    {
        Debug.Log("Purchaser::purchaseSuccess purchase of " + skuid + " successful!");

        base.purchaseSuccess(skuid);
        //purchase is successful release the product

        if (skuid == "continue")
            GameManager.Singleton.OnPurchaseSuccessful();
    }

    //method gets called on failed purchases
    public override void purchaseFailure(string skuid)
    {
        Debug.Log("Purchaser::purchaseFailure purchase of " + skuid + " failed!");
        
        base.purchaseFailure(skuid);
        //purchase failed perhaps show some error message

        GameManager.Singleton.OnPurchaseFailed();
    }

    //example methods to initiate a purchase flow
    //the string parameter of the makePurchase method is the skuid you specified in the inspector for each product
    public void buyContinue()
    {
        Debug.Log("Going to call makePurchase with skuid continue");
        makePurchase("continue");
    }
}