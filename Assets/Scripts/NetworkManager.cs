using UnityEngine;
using NativeWebSocket;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    WebSocket websocket;

    async void Start() {
        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnOpen += () =>
        {
            Debug.Log("✅ Connected to the server!");
            SendInitialMessage();
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("❌ Error: " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("🔴 Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            string message = Encoding.UTF8.GetString(bytes);
            Debug.Log("📩 Message received from server: " + message);
        };

        await websocket.Connect();
    }

    void Update()
    {
        #if !UNITY_WEBL || UNITY_EDITOR
            websocket.DispatchMessageQueue();
        #endif
    }

    async void SendInitialMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            await websocket.SendText("Hello from Unity!");
        }
    }

    private async void OnApplicationQuit()
    {
        if (websocket != null)
        {
            await websocket.Close();
        }
    }
}
