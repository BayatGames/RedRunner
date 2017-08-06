using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedRunner.Collectables
{

	[RequireComponent ( typeof ( Rigidbody2D ) )]
	public class CoinRigidbody2D : Coin
	{

		[SerializeField]
		protected Rigidbody2D m_Rigidbody2D;
		[SerializeField]
		protected Collider2D m_SecondCollider2D;

		public Rigidbody2D Rigidbody2D { 
			get { 
				return m_Rigidbody2D;
			}
		}

		public override void Collect ()
		{
			m_SecondCollider2D.enabled = false;
			base.Collect ();
		}

	}

}