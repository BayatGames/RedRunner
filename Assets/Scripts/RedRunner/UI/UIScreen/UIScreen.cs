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
        [SerializeField]
        internal UIScreenInfo ScreenInfo;
		[SerializeField]
		protected Animator m_Animator;
		[SerializeField]
		protected CanvasGroup m_CanvasGroup;

        public bool IsOpen { get; set; }

        public virtual void UpdateScreenStatus(bool open)
        {
            m_Animator.SetBool("Open", open);
            m_CanvasGroup.interactable = open;
            m_CanvasGroup.blocksRaycasts = open;
            IsOpen = open;
        }
	}

}