using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedRunner.UI
{
    public class EndScreen : UIScreen
    {
        [SerializeField]
        protected Button ResetButton = null;
        [SerializeField]
        protected Button HomeButton = null;
        [SerializeField]
        protected Button ExitButton = null;

        // The Continue button
        [SerializeField]
        Button ContinueButton = null;

        private void Start()
        {
            ResetButton.SetButtonAction(() =>
            {
                GameManager.Singleton.Reset();
                var ingameScreen = UIManager.Singleton.GetUIScreen(UIScreenInfo.IN_GAME_SCREEN);
                UIManager.Singleton.OpenScreen(ingameScreen);
                GameManager.Singleton.StartGame();
            });

            // Add a function to the continue button
            ContinueButton.SetButtonAction(() =>
            {
                // Remove 50 gold when the player clicks the button
                GameManager.Singleton.m_Coin.Value -= 50;
                GameManager.Singleton.RespawnMainCharacter();
                var ingameScreen = UIManager.Singleton.GetUIScreen(UIScreenInfo.IN_GAME_SCREEN);
                UIManager.Singleton.OpenScreen(ingameScreen);
                GameManager.Singleton.StartGame();
            }
            );
        }

        // Check for if the player has enough coins. If so, enable the button. Otherwise, disable it
        private void Update()
        {
            if (GameManager.Singleton.m_Coin.Value >= 50)
            {
                ContinueButton.gameObject.SetActive(true);
            }
            else
            {
                ContinueButton.gameObject.SetActive(false);
            }
        }

        public override void UpdateScreenStatus(bool open)
        {
            base.UpdateScreenStatus(open);
        }
    }

}
