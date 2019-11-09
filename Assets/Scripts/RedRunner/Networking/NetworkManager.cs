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

		private string host = "localhost";

		public static bool IsServer { get; private set; } = false;

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
				IsServer = GUILayout.Toggle(IsServer, "Host");
				if (!IsServer)
				{
					host = GUILayout.TextField(host);
				}
			}
		}

		public void Connect()
		{
			if (IsServer)
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
