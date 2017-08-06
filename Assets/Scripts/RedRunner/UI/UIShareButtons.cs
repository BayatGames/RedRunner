using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RedRunner.UI
{

	public class UIShareButtons : MonoBehaviour
	{

		[SerializeField]
		protected Animator m_ShareBackground;
		[SerializeField]
		protected Animator[] m_ShareButtons;
		protected bool m_IsOpen = false;

		void Start ()
		{
			
		}

		public void Toggle ()
		{
			if ( m_IsOpen )
			{
				m_IsOpen = false;
				SetTrigger ( "Close" );
			}
			else
			{
				m_IsOpen = true;
				SetTrigger ( "Open" );
			}
		}

		public void SetTrigger ( string trigger )
		{
			m_ShareBackground.SetTrigger ( trigger );
			for ( int i = 0; i < m_ShareButtons.Length; i++ )
			{
				m_ShareButtons [ i ].SetTrigger ( trigger );
			}
		}

	}

}