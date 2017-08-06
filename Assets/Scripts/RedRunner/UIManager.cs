using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using RedRunner.UI;

namespace RedRunner
{

	public class UIManager : MonoBehaviour
	{

		private static UIManager m_Singleton;

		public static UIManager Singleton
		{
			get
			{
				return m_Singleton;
			}
		}

		[SerializeField]
		private UIScreen[] m_Screens;
		private UIScreen m_ActiveScreen;
		private UIWindow m_ActiveWindow;
		[SerializeField]
		private Texture2D m_CursorDefaultTexture;
		[SerializeField]
		private Texture2D m_CursorClickTexture;
		[SerializeField]
		private float m_CursorHideDelay = 1f;
		[SerializeField]
		private UIScreen m_PauseScreen;

		void Awake ()
		{
			if ( m_Singleton != null )
			{
				Destroy ( gameObject );
				return;
			}
			m_Singleton = this;
			Cursor.SetCursor ( m_CursorDefaultTexture, Vector2.zero, CursorMode.Auto );
		}

		void Start ()
		{
			OpenScreen ( m_Screens [ 0 ] );
		}

		void Update ()
		{
			if ( Input.GetButtonDown ( "Cancel" ) )
			{
				OpenScreen ( m_PauseScreen );
			}
			if ( Input.GetMouseButtonDown ( 0 ) )
			{
				Cursor.SetCursor ( m_CursorClickTexture, Vector2.zero, CursorMode.Auto );
			}
			else if ( Input.GetMouseButtonUp ( 0 ) )
			{
				Cursor.SetCursor ( m_CursorDefaultTexture, Vector2.zero, CursorMode.Auto );
			}
		}

		public void OpenWindow ( UIWindow window )
		{
			window.Open ();
			m_ActiveWindow = window;
		}

		public void CloseWindow ( UIWindow window )
		{
			if ( m_ActiveWindow == window )
			{
				m_ActiveWindow = null;
			}
			window.Close ();
		}

		public void CloseActiveWindow ()
		{
			if ( m_ActiveWindow != null )
			{
				CloseWindow ( m_ActiveWindow );
			}
		}

		public void OpenScreen ( int index )
		{
			OpenScreen ( m_Screens [ index ] );
		}

		public void OpenScreen ( UIScreen screen )
		{
			CloseAllScreens ();
			if ( m_ActiveScreen == null )
			{
				m_ActiveScreen = screen;
				m_ActiveScreen.Open ();
				return;
			}
			m_ActiveScreen.OnClosed += screen.Open;
			m_ActiveScreen.Close ();
			m_ActiveScreen = screen;
		}

		public void CloseScreen ( int index )
		{
			CloseScreen ( m_Screens [ index ] );
		}

		public void CloseScreen ( UIScreen screen )
		{
			if ( m_ActiveScreen == screen )
			{
				m_ActiveScreen = null;
			}
			screen.Close ();
		}

		public void CloseAllScreens ()
		{
			CloseAllScreens ( m_ActiveScreen );
		}

		public void CloseAllScreens ( UIScreen except )
		{
			for ( int i = 0; i < m_Screens.Length; i++ )
			{
				if ( except == null || m_Screens [ i ] != except )
				{
					CloseScreen ( m_Screens [ i ] );
				}
			}
		}

	}

}