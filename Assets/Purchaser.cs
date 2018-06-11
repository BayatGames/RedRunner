//add this namespace to your script to give you  access to the plugin classes.
using Codeberg.AppcoinsUnity;
using RedRunner;

public class Purchaser : AppcoinsPurchaser
{

    //method gets called on successful purchases
    public override void purchaseSuccess(string skuid)
    {
        base.purchaseSuccess(skuid);
        //purchase is successful release the product

        if (skuid == "continue")
            GameManager.Singleton.OnPurchaseSuccessful();
    }
    //method gets called on failed purchases
    public override void purchaseFailure(string skuid)
    {
        base.purchaseFailure(skuid);
        //purchase failed perhaps show some error message

    }

    //example methods to initiate a purchase flow
    //the string parameter of the makePurchase method is the skuid you specified in the inspector for each product
    public void buyContinue()
    {
        makePurchase("continue");
    }
}