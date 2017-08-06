using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using RedRunner.Collectables;

namespace RedRunner.UI
{

	public class UICoinImage : Image
	{

		[SerializeField]
		protected ParticleSystem m_ParticleSystem;

		protected override void Awake ()
		{
			Coin.OnCoinCollected += Coin_OnCoinCollected;
			base.Awake ();
		}

		void Coin_OnCoinCollected (Coin coin)
		{
			GetComponent<Animator> ().SetTrigger ("Collect");
		}

		public virtual void PlayParticleSystem ()
		{
			m_ParticleSystem.Play ();
		}

	}

}