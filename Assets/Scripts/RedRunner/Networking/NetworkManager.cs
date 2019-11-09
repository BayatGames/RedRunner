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

        public override void OnServerAddPlayer(Mirror.NetworkConnection conn)
        {
            GameObject player = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
            Mirror.NetworkServer.AddPlayerForConnection(conn, player);
        }
    }
}
