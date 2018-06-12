//created by Lukmon Agboola(Codeberg)
//Note: do not change anything here as it may break the workings of the plugin else you're very sure of what you're doing.

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Codeberg.AppcoinsUnity
{

    public class AppcoinsUnity : MonoBehaviour
    {

        [Header("Your wallet address for receiving Appcoins")]
        public string receivingAddress;
        [Header("Uncheck to disable Appcoins IAB")]
        public bool enableIAB = true;
        [Header("Uncheck to disable Appcoins ADS(Proof of attention)")]
        public bool enablePOA = true;
        [Header("Enable debug to use testnets e.g Ropsten")]
        public bool enableDebug = false;
        [Header("Add all your products here")]
        public AppcoinsSku[] products;
        [Header("Add your purchaser object here")]
        public AppcoinsPurchaser purchaserObject;

        private bool previousEnablePOA = true;

        AndroidJavaClass _class;
        AndroidJavaObject instance { get { return _class.GetStatic<AndroidJavaObject>("instance"); } }

        // Use this for initialization
        void Start()
        {

            //get refference to java class
            _class = new AndroidJavaClass("com.codeberg.appcoinsunity.UnityAppcoins");

            //setup wallet address
            _class.CallStatic("setAddress", receivingAddress);

            //set debug mode
            //NOTE: this allows you to make purchases with testnets e.g Ropsten
            _class.CallStatic("enableDebug", enableDebug);

            //Enable or disable In App Billing
            _class.CallStatic("enableIAB", enableIAB);

            //add all your skus here
            addAllSKUs();

            //start sdk
            _class.CallStatic("start");

        }

        // This function is called when this script is loaded or some variable changes its value.
        void OnValidate()
        {

            // Put new value of enablePOA in mainTemplate.gradle to enable it or disable it.
            if (previousEnablePOA != enablePOA)
            {
                previousEnablePOA = enablePOA;

                changeMainTemplateGradle(previousEnablePOA);
            }
        }


        //called to add all skus specified in the inpector window.
        private void addAllSKUs()
        {
            for (int i = 0; i < products.Length; i++)
            {
                _class.CallStatic("addNewSku", products[i].Name, products[i].SKUID, products[i].Price);
            }
        }

        //method used in making purchase
        public void makePurchase(string skuid)
        {
#if UNITY_EDITOR
            if (EditorUtility.DisplayDialog("AppCoins Unity Integration","AppCoins IAB Successfully integrated","Test success","Test failure")){
                purchaseSuccess(skuid);
            } else {
                purchaseFailure(skuid);
            }
#else
            _class.CallStatic("makePurchase",skuid);
#endif
      
	}

	//callback on successful purchases
	public void purchaseSuccess(string skuid){
			purchaserObject.purchaseSuccess (skuid);
	}

	//callback on failed purchases
	public void purchaseFailure(string skuid){
			purchaserObject.purchaseFailure (skuid);
	}

	// Change the mainTemplate.gradle's ENABLE_POA var to its new value
	private void changeMainTemplateGradle(bool POA) {
		string pathToMainTemplate = Application.dataPath + "/Plugins/Android/mainTemplate.gradle"; // Path to mainTemplate.gradle
		string line;
		string contentToChange = "resValue \"bool\", \"APPCOINS_ENABLE_POA\", \"" + POA.ToString().ToLower() + "\""; //Line to change inside test container
		string contentInTemplate = "resValue \"bool\", \"APPCOINS_ENABLE_POA\", \"" + (!POA).ToString().ToLower() + "\"";
		int lineToChange = -1;
		int counter = 0;
		int numberOfSpaces = 0;
		ArrayList fileLines = new ArrayList();

		System.IO.StreamReader fileReader = new System.IO.StreamReader(pathToMainTemplate);  
		
		//Read all lines and get the line numer to be changed
		while((line = fileReader.ReadLine()) != null) {
			fileLines.Add(line);

			//Get the new line and number of spaces erased.
			ArrayList a = RemoveFirstsWhiteSpaces(line);
			line = (string) a[0];

			//Debug.Log(line);

			if(line.Length == contentInTemplate.Length && line.Substring(0, contentInTemplate.Length).Equals(contentInTemplate)) {
				lineToChange = counter;
				numberOfSpaces = (int) a[1];
			} 

			counter++;
		}

		fileReader.Close();

		if(lineToChange > -1) {
			for(int i = 0; i < numberOfSpaces; i++) {
				contentToChange = string.Concat(" ", contentToChange);
			}

			fileLines[lineToChange] = contentToChange;
		}

		System.IO.StreamWriter fileWriter = new System.IO.StreamWriter(pathToMainTemplate);

		foreach(string newLine in fileLines) {
			fileWriter.WriteLine(newLine);
		}

		fileWriter.Close();
	}

	private static ArrayList RemoveFirstsWhiteSpaces(string line) {
		int lettersToRemove = 0;

		foreach(char letter in line) {
			if(char.IsWhiteSpace(letter)) {
				lettersToRemove++;
			}

			else {
				break;
			}
		}

		if(lettersToRemove > 0) {
			line = line.Substring(lettersToRemove);
		}

		ArrayList a = new ArrayList();
		a.Add(line);
		a.Add(lettersToRemove);

		return a;
	}
 }
}
