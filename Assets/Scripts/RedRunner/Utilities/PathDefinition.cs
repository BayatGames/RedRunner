using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedRunner.Utilities
{

	[ExecuteInEditMode]
	public class PathDefinition : MonoBehaviour
	{

		[SerializeField]
		protected List<PathPoint> m_Points;
		[SerializeField]
		protected bool m_UseGlobalDelay = false;
		[SerializeField]
		protected float m_GlobalDelay = 0f;
		[SerializeField]
		protected bool m_ContinueToStart = false;

		protected int m_CurrentPointIndex = 0;

		public virtual List<PathPoint> Points {
			get {
				return m_Points;
			}
		}

		public virtual bool UseGlobalDelay {
			get {
				return m_UseGlobalDelay;
			}
		}

		public virtual float GlobalDelay {
			get {
				return m_GlobalDelay;
			}
		}

		public virtual bool ContinueToStart {
			get {
				return m_ContinueToStart;
			}
		}

		public virtual int CurrentPointIndex {
			get {
				return m_CurrentPointIndex;
			}
		}

		#if UNITY_EDITOR
		void OnEnable ()
		{
			if ( m_Points == null )
			{
				m_Points = new List<PathPoint> ();
			}
		}
		#endif

		#if UNITY_EDITOR
		void Update ()
		{
			if ( transform.childCount != m_Points.Count )
			{
				m_Points.Clear ();
				for ( int i = 0; i < transform.childCount; i++ )
				{
					Transform child = transform.GetChild ( i );
					PathPoint point = child.GetComponent<PathPoint> ();
					if ( point != null )
					{
						m_Points.Add ( point );
					}
				}
			}
		}
		#endif

		public IEnumerator<PathPoint> GetPathEnumerator ()
		{
			// Exit when points count is smaller one
			if ( m_Points == null || m_Points.Count < 1 )
				yield break;

			var direction = 1;
			var index = 0;
			m_CurrentPointIndex = index;
			while ( true )
			{
				yield return m_Points [ index ];
				if ( m_Points.Count == 1 )
					continue;
				
				if ( index <= 0 )
				{
					direction = 1;
				}
				else if ( index >= m_Points.Count - 1 )
				{
					direction = -1;
				}

				if ( index == m_Points.Count - 1 && m_ContinueToStart )
				{
					index = 0;
				}
				else
				{
					index = index + direction;
				}
				m_CurrentPointIndex = index;
			}
		}

		public void OnDrawGizmos ()
		{
			if ( m_Points == null || m_Points.Count < 2 )
				return;

			for ( var i = 1; i < m_Points.Count; i++ )
			{
				Gizmos.DrawLine ( m_Points [ i - 1 ].transform.position, m_Points [ i ].transform.position );
			}
		}

	}

}