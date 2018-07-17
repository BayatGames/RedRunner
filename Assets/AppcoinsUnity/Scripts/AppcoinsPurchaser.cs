//created by Lukmon Agboola(Codeberg)
//Modified by Aptoide
//Inherit this class to create your own purchaser class, see the example scene for more info

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aptoide.AppcoinsUnity{

	public class AppcoinsPurchaser : MonoBehaviour {

		AppcoinsUnity appcoinsUnity;

        public void Init(AppcoinsUnity appcoinsUnityRef){
            //get refference to AppcoinsUnity class
            appcoinsUnity = appcoinsUnityRef;
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

		public void makePurchase(string skuid){
			appcoinsUnity.makePurchase (skuid);
		}

	}
}
