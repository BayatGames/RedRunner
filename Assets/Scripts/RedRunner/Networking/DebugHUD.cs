using System.ComponentModel;
using UnityEngine;

namespace RedRunner.Networking
{
    [RequireComponent(typeof(NetworkManager))]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DebugHUD : MonoBehaviour
    {
        NetworkManager manager;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 40, 215, 9999));
            if (!Mirror.NetworkClient.isConnected && !Mirror.NetworkServer.active)
            {
                if (!Mirror.NetworkClient.active)
                {
                    // LAN Host
                    if (Application.platform != RuntimePlatform.WebGLPlayer)
                    {
                        if (GUILayout.Button("LAN Host"))
                        {
                            manager.StartHost();
                        }
                    }

                    // LAN Client + IP
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("LAN Client"))
                    {
                        manager.StartClient();
                    }
                    manager.networkAddress = GUILayout.TextField(manager.networkAddress);
                    GUILayout.EndHorizontal();

                    // LAN Server Only
                    if (Application.platform == RuntimePlatform.WebGLPlayer)
                    {
                        // cant be a server in webgl build
                        GUILayout.Box("(  WebGL cannot be server  )");
                    }
                    else
                    {
                        if (GUILayout.Button("LAN Server Only")) manager.StartServer();
                    }
                }
                else
                {
                    // Connecting
                    GUILayout.Label("Connecting to " + manager.networkAddress + "..");
                    if (GUILayout.Button("Cancel Connection Attempt"))
                    {
                        manager.StopClient();
                    }
                }
            }
            else
            {
                // server / client status message
                if (Mirror.NetworkServer.active)
                {
                    GUILayout.Label("Server: active. Transport: " + Mirror.Transport.activeTransport);
                }
                if (Mirror.NetworkClient.isConnected)
                {
                    GUILayout.Label("Client: address=" + manager.networkAddress);
                }
            }

            // client ready
            if (Mirror.NetworkClient.isConnected && !Mirror.ClientScene.ready)
            {
                if (GUILayout.Button("Client Ready"))
                {
                    Mirror.ClientScene.Ready(Mirror.NetworkClient.connection);

                    if (Mirror.ClientScene.localPlayer == null)
                    {
                        Mirror.ClientScene.AddPlayer();
                    }
                }
            }

            // stop
            if (Mirror.NetworkServer.active || Mirror.NetworkClient.isConnected)
            {
                if (GUILayout.Button("Stop"))
                {
                    manager.StopHost();
                }
            }

            GUILayout.EndArea();
        }
    }
}
