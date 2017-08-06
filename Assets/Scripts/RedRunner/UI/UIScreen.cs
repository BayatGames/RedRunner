using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RedRunner.UI
{

	public class UIScreen : MonoBehaviour
	{

		public delegate void CloseHandler ();

		public delegate void OpenHandler ();

		public delegate void OpenChangedHandler ( bool open );

		public event OpenChangedHandler OnOpenChanged;
		public event CloseHandler OnClose;
		public event OpenHandler OnOpen;
		public event CloseHandler OnClosed;
		public event OpenHandler OnOpened;

		[SerializeField]
		protected Animator m_Animator;
		[SerializeField]
		protected CanvasGroup m_CanvasGroup;
		[SerializeField]
		protected OpenEvent m_OnOpen;
		[SerializeField]
		protected OpenFinishEvent m_OnOpened;
		[SerializeField]
		protected CloseEvent m_OnClose;
		[SerializeField]
		protected CloseFinishEvent m_OnClosed;
		[SerializeField]
		protected GameObject m_FirstSelected;
		protected bool m_Open = false;

		public bool open
		{
			get
			{
				return m_Open;
			}
		}

		public void Close ()
		{
			if ( OnClose != null )
			{
				OnClose ();
			}
			m_OnClose.Invoke ();
			m_Animator.enabled = true;
			m_Animator.SetBool ( "Open", false );
		}

		public void Closed ()
		{
			if ( OnClosed != null )
			{
				OnClosed ();
			}
			OnClosed = null;
			m_OnClosed.Invoke ();
			m_Open = false;
			OpenChanged ();
			m_Animator.enabled = true;
		}

		public void Open ()
		{
			if ( OnOpen != null )
			{
				OnOpen ();
			}
			m_OnOpen.Invoke ();
			m_Animator.enabled = true;
			m_Animator.SetBool ( "Open", true );
		}

		public void Opened ()
		{
			if ( OnOpened != null )
			{
				OnOpened ();
			}
			OnOpened = null;
			m_OnOpened.Invoke ();
			m_Open = true;
			OpenChanged ();
			m_Animator.enabled = false;
			if ( m_FirstSelected != null )
			{
				EventSystem.current.SetSelectedGameObject ( m_FirstSelected );
			}
		}

		protected void OpenChanged ()
		{
			m_CanvasGroup.interactable = m_Open;
			m_CanvasGroup.blocksRaycasts = m_Open;
			if ( OnOpenChanged != null )
			{
				OnOpenChanged ( m_Open );
			}
		}

		[System.Serializable]
		public class OpenEvent : UnityEvent
		{

		}

		[System.Serializable]
		public class OpenFinishEvent : UnityEvent
		{
			
		}

		[System.Serializable]
		public class CloseEvent : UnityEvent
		{

		}

		[System.Serializable]
		public class CloseFinishEvent : UnityEvent
		{

		}

	}

}