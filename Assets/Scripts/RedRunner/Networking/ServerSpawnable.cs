using UnityEngine;

namespace RedRunner.Networking
{
	/// <summary>
	/// Helper component for game object spawned by the server which should synchronize to clients.
	/// </summary>
	[RequireComponent(typeof(Mirror.NetworkIdentity))]
	public class ServerSpawnable : MonoBehaviour
	{
		public virtual void Awake()
		{
			if (NetworkManager.IsServer)
			{
				NetworkManager.Spawn(gameObject);
			}
		}
	}
}
