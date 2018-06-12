//created by Lukmon Agboola(Codeberg)
//Inherit this class to create your own purchaser class, see the example scene for more info

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RedRunner;

namespace Codeberg.AppcoinsUnity{

	public class AppcoinsPurchaser : MonoBehaviour {

		// CHANGES
		protected AppcoinsUnity appcoinsUnity;

		void OnEnable(){
			//get refference to AppcoinsUnity class
			appcoinsUnity = GameObject.Find ("AppcoinsUnity").GetComponent<AppcoinsUnity>();
		}

        public virtual void purchaseSuccess(string skuid)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("AppCoins Unity Integration", "Purchase Success!", "OK");
#endif
        }

        public virtual void purchaseFailure(string skuid)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog("AppCoins Unity Integration", "Purchase Failed!", "OK");
#endif
        }

		//CHANGES
		// WHAT TO DO WHEN IAB IS DISABLED
		public virtual void jumpPurchase() {
			return;
		}

		//CHANGES
		public void makePurchase(string skuid){
			if(appcoinsUnity.enableIAB) {
				appcoinsUnity.makePurchase (skuid);
			}

			else {
				jumpPurchase();
			}
		}
	}
}
