using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedRunner.UI
{
    public class InGameScreen : UIScreen
    {
        [SerializeField]
        protected Button PauseButton = null;

        private void Start()
        {
            PauseButton.SetButtonAction(() =>
            {
                var pauseScreen = UIManager.Singleton.UISCREENS.Find(el => el.ScreenInfo == UIScreenInfo.PAUSE_SCREEN);
                UIManager.Singleton.OpenScreen(pauseScreen);
                GameManager.Singleton.StopGame();
            });
        }

        public override void UpdateScreenStatus(bool open)
        {
            base.UpdateScreenStatus(open);
        }
    }

}