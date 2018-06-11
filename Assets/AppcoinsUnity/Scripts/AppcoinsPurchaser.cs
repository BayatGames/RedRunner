//created by Lukmon Agboola(Codeberg)
//Inherit this class to create your own purchaser class, see the example scene for more info

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Codeberg.AppcoinsUnity{

	public class AppcoinsPurchaser : MonoBehaviour {

		AppcoinsUnity appcoinsUnity;

		void OnEnable(){
			//get refference to AppcoinsUnity class
			appcoinsUnity = GameObject.Find ("AppcoinsUnity").GetComponent<AppcoinsUnity>();
		}

		public virtual void purchaseSuccess(string skuid){
			
		}

		public virtual void purchaseFailure(string skuid){

		}

		public void makePurchase(string skuid){
			appcoinsUnity.makePurchase (skuid);
		}
 
	}
}
