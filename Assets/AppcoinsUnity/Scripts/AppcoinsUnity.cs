//created by Lukmon Agboola(Codeberg)
//Modified by Aptoide
//Note: do not change anything here as it may break the workings of the plugin else you're very sure of what you're doing.

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Aptoide.AppcoinsUnity
{

    public class AppcoinsUnity : MonoBehaviour
    {
        [Header("Your wallet address for receiving Appcoins")]
        public string receivingAddress;
        [Header("Uncheck to disable Appcoins IAB")]
        public bool enableIAB = true;
        [Header("Uncheck to disable Appcoins ADS(Proof of attention)")]
        public bool enablePOA = false;
        [Header("Enable debug to use testnets e.g Ropsten")]
        public bool enableDebug = false;
        [Header("Add all your products here")]
        public AppcoinsSku[] products;
        [Header("Add your purchaser object here")]
        public AppcoinsPurchaser purchaserObject;

        private string NAME = "NAME";
        private string previousName = null;

        private string POA = "POA";
        private string DEBUG = "DEBUG";
        private string APPCOINS_PREFAB = "APPCOINS_PREFAB";
        private bool previousEnablePOA = false;
        private bool previousDebug = false;

        AndroidJavaClass _class;
        AndroidJavaObject instance { get { return _class.GetStatic<AndroidJavaObject>("instance"); } }

        private void Awake()
        {
            purchaserObject.Init(this);
        }

        // Use this for initialization
        void Start()
        {
            //get refference to java class
            _class = new AndroidJavaClass("com.aptoide.appcoinsunity.UnityAppcoins");

            //setup wallet address
            _class.CallStatic("setAddress", receivingAddress);

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
                updateVarOnMainTemplateGradle(POA, previousEnablePOA.ToString());
            }

            if (previousDebug != enableDebug)
            {
                previousDebug = enableDebug;
                updateVarOnMainTemplateGradle(DEBUG, previousDebug.ToString());
            }

            if(previousName == null || !previousName.Equals(this.name))
            {
                changePrefabName();
                previousName = string.Copy(this.name);
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
            if (!enableIAB)
            {
                Debug.LogWarning("Tried to make a purchase but enableIAB is false! Please set it to true on AppcoinsUnity object before using this functionality");
                return;
            }

#if UNITY_EDITOR
            if (EditorUtility.DisplayDialog("AppCoins Unity Integration", "AppCoins IAB Successfully integrated", "Test success", "Test failure"))
            {
                purchaseSuccess(skuid);
            }
            else
            {
                purchaseFailure(skuid);
            }
#else
            _class.CallStatic("makePurchase",skuid);
#endif

        }

        //callback on successful purchases
        public void purchaseSuccess(string skuid)
        {
            if (purchaserObject != null)
            {
                Debug.Log("Going to call purchaseSuccess on purchaserObject skuid " + skuid);
                purchaserObject.purchaseSuccess(skuid);
            }
            else
            {
                Debug.Log("purchaserObject is null");
            }
        }

        //callback on failed purchases
        public void purchaseFailure(string skuid)
        {
            if (purchaserObject != null)
            {
                Debug.Log("Going to call purchaseFailure on purchaserObject skuid " + skuid);
                purchaserObject.purchaseFailure(skuid);
            }
            else
            {
                Debug.Log("purchaserObject is null");
            }
        }

        private void changePrefabName()
        {
            string line;
            ArrayList fileLines = new ArrayList();

            System.IO.StreamReader fileReader = new System.IO.StreamReader(Application.dataPath + "/Plugins/Android/mainTemplate.gradle");

            while((line = fileReader.ReadLine()) != null)
            {
                if(line.Contains(APPCOINS_PREFAB))
                {
                    int i = 0;
                    string newLine = "";

                    while(line[i].Equals("\t") || line[i].Equals(" "))
                    {
                        i++;
                        newLine = string.Concat("\t", "");
                    }

                    newLine = string.Concat(newLine, line);

                    //Erase content after last comma
                    int lastComma = newLine.LastIndexOf(",");
                    newLine = newLine.Substring(0, lastComma + 1);
                    newLine = string.Concat(newLine, " \"" + this.name + "\"");

                    fileLines.Add(newLine);
                }

                else
                {
                    fileLines.Add(line);
                }
            }

            fileReader.Close();

            System.IO.StreamWriter fileWriter = new System.IO.StreamWriter(Application.dataPath + "/Plugins/Android/mainTemplate.gradle");

            foreach(string newLine in fileLines)
            {
                fileWriter.WriteLine(newLine);
            }

            fileWriter.Close();
        }

        // Change the mainTemplate.gradle's ENABLE_POA var to its new value
        private void updateVarOnMainTemplateGradle(string varName, string varToCheck)
        {
            string pathToMainTemplate = Application.dataPath + "/Plugins/Android/mainTemplate.gradle"; // Path to mainTemplate.gradle
            string line;
            string contentToChange = null;
            string contentInTemplate = null;
            ArrayList linesToChange = new ArrayList();
            int counter = 0;
            int numberOfSpaces = 0;
            ArrayList fileLines = new ArrayList();

            //Line to change inside test container
            if(varName.Equals(POA))
            {
                contentToChange = "resValue \"bool\", \"APPCOINS_ENABLE_POA\", \"" + varToCheck.ToLower() + "\"";
                contentInTemplate = "resValue \"bool\", \"APPCOINS_ENABLE_POA\", \"" + ((varToCheck.ToLower()).Equals("true") ? "false" : "true") + "\"";
            }

            else if(varName.Equals(DEBUG))
            {
                contentToChange = "resValue \"bool\", \"APPCOINS_ENABLE_DEBUG\", \"" + varToCheck.ToLower() + "\"";
                contentInTemplate = "resValue \"bool\", \"APPCOINS_ENABLE_DEBUG\", \"" + ((varToCheck.ToLower()).Equals("true") ? "false" : "true") + "\"";
            }

            System.IO.StreamReader fileReader = new System.IO.StreamReader(pathToMainTemplate);

            //Read all lines and get the line numer to be changed
            while ((line = fileReader.ReadLine()) != null)
            {
                fileLines.Add(line);

                //Get the new line and number of spaces erased.
                ArrayList a = RemoveFirstsWhiteSpaces(line);
                line = (string)a[0];

                //Debug.Log(line);

                if (line.Length == contentInTemplate.Length && line.Substring(0, contentInTemplate.Length).Equals(contentInTemplate))
                {
                    linesToChange.Add(counter);
                    numberOfSpaces = (int)a[1];
                }

                counter++;
            }

            fileReader.Close();

            foreach(int lineToChange in linesToChange)
            {
                if (lineToChange > -1)
                {
                    string change = contentToChange;

                    for (int i = 0; i < numberOfSpaces; i++)
                    {
                        change = string.Concat(" ", change);
                    }

                    fileLines[lineToChange] = change;
                }
            }

            System.IO.StreamWriter fileWriter = new System.IO.StreamWriter(pathToMainTemplate);

            foreach (string newLine in fileLines)
            {
                fileWriter.WriteLine(newLine);
            }

            fileWriter.Close();
        }

        private static ArrayList RemoveFirstsWhiteSpaces(string line)
        {
            int lettersToRemove = 0;

            foreach (char letter in line)
            {
                if (char.IsWhiteSpace(letter))
                {
                    lettersToRemove++;
                }

                else
                {
                    break;
                }
            }

            if (lettersToRemove > 0)
            {
                line = line.Substring(lettersToRemove);
            }

            ArrayList a = new ArrayList();
            a.Add(line);
            a.Add(lettersToRemove);

            return a;
        }
    }
}