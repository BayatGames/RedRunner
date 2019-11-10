using UnityEngine;
using RedRunner.Characters;
using RedRunner.Utilities;

namespace RedRunner.Networking
{
	public class NetworkManager : Mirror.NetworkManager
	{
		[SerializeField]
		private Transform spawnPoint;
		[SerializeField]
		private CameraController cameraController;

		private static bool shouldHost = false;
		private static string host = "localhost";

		public static bool IsConnected
		{
			get
			{
				return Mirror.NetworkClient.isConnected;
			}
		}

		public static bool IsServer
		{
			get
			{
				return shouldHost && IsConnected;
			}
		}

		public static RedCharacter LocalCharacter { get; private set; }

		public static NetworkManager Instance { get; private set; }

		public override void Awake()
		{
			Instance = GetComponent<NetworkManager>();
			base.Awake();

			RedCharacter.LocalPlayerSpawned += () =>
			{
				cameraController.Follow(RedCharacter.Local.transform);
			};
		}

		// TODO(shane) get rid of this nasty hack when we have a proper dedicated server.
		public void OnGUI()
		{
			if (!Mirror.NetworkClient.isConnected && !Mirror.NetworkServer.active && !Mirror.NetworkClient.active)
			{
				shouldHost = GUILayout.Toggle(shouldHost, "Host");
				if (!shouldHost)
				{
					host = GUILayout.TextField(host);
				}
			}
		}

		public void Connect()
		{
			if (shouldHost)
			{
				StartHost();
			}
			else
			{
				networkAddress = host;
				StartClient();
			}
		}

		public override void OnServerAddPlayer(Mirror.NetworkConnection conn)
		{
			GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
			Mirror.NetworkServer.AddPlayerForConnection(conn, player);
		}

		public static void RegisterSpawnablePrefab(GameObject prefab)
		{
			Mirror.ClientScene.RegisterPrefab(prefab);
		}

		public static void Spawn(GameObject gameObject)
		{
			Mirror.NetworkServer.Spawn(gameObject);
		}
	}
}
