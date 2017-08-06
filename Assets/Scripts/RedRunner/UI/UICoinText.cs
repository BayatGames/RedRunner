using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RedRunner.Collectables;

namespace RedRunner.UI
{

	public class UICoinText : UIText
	{

		[SerializeField]
		protected string m_CoinTextFormat = "x {0}";

		protected override void Awake ()
		{
			GameManager.OnCoinChanged += GameManager_OnCoinChanged;
			Coin.OnCoinCollected += Coin_OnCoinCollected;
			base.Awake ();
		}

		void Coin_OnCoinCollected (Coin coin)
		{
			GetComponent<Animator> ().SetTrigger ("Collect");
		}

		void GameManager_OnCoinChanged (int coin)
		{
			text = string.Format (m_CoinTextFormat, coin);
		}

	}

}