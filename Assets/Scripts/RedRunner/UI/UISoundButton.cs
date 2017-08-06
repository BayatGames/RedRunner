using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedRunner.UI
{

	public class UISoundButton : UIButton
	{

		[SerializeField]
		protected Sprite m_DisabledSoundSprite;
		protected Sprite m_DefaultSprite;

		protected override void Awake ()
		{
			m_DefaultSprite = image.sprite;
			GameManager.OnAudioEnabled += GameManager_OnAudioEnabled;
			base.Awake ();
		}

		public override void OnPointerDown ( UnityEngine.EventSystems.PointerEventData eventData )
		{
			GameManager.Singleton.ToggleAudioEnabled ();
			base.OnPointerDown ( eventData );
		}

		void GameManager_OnAudioEnabled ( bool active )
		{
			image.sprite = active ? m_DefaultSprite : m_DisabledSoundSprite;
		}

	}

}