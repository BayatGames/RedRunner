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

        private bool isServer = false;

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
                isServer = GUILayout.Toggle(isServer, "Host");
            }
        }

        public void Connect()
        {
            if (isServer)
            {
                StartHost();
            }
            else
            {
                networkAddress = "localhost";
                StartClient();
            }
        }

        public override void OnServerAddPlayer(Mirror.NetworkConnection conn)
        {
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            Mirror.NetworkServer.AddPlayerForConnection(conn, player);
        }
    }
}
