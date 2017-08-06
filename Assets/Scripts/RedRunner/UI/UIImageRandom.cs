using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedRunner.UI
{

	public class UIImageRandom : MonoBehaviour
	{

		[System.Serializable]
		public struct RandomImageItem
		{

			public Color color;
			public Sprite sprite;
			
		}

		[SerializeField]
		protected RandomImageItem[] m_RandomItems;
		[SerializeField]
		protected Image m_ColorImage;
		[SerializeField]
		protected Image m_PatternImage;

		protected virtual void Start ()
		{
			if ( m_RandomItems.Length > 0 )
			{
				int index = Random.Range ( 0, m_RandomItems.Length );
				m_PatternImage.sprite = m_RandomItems [ index ].sprite;
				Color newColor = m_RandomItems [ index ].color;
				newColor.a = Color.white.a;
				m_ColorImage.color = newColor;
				newColor = Color.white;
				newColor.a = m_RandomItems [ index ].color.a;
				m_PatternImage.color = newColor;
			}
		}
	
	}

}