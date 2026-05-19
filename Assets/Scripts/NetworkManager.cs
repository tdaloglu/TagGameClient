using UnityEngine;
using NativeWebSocket;
using System.Text;

[System.Serializable]
public class ClientMessage
{
    public string type;
    public string username;
    public string roomId;
}

[System.Serializable]
public class ServerMessage
{
    public string type;
    public string message;
    public string roomId;
    public string username;
}

public class NetworkManager : MonoBehaviour
{
    WebSocket websocket;

    async void Start() {
        websocket = new WebSocket("ws://localhost:8080");

        websocket.OnOpen += () =>
        {
            Debug.Log("Connected to the server!");
            SendLoginMessage();
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error: " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            string jsonString = Encoding.UTF8.GetString(bytes);

            ServerMessage response = JsonUtility.FromJson<ServerMessage>(jsonString);
            Debug.Log($"Server says: [{response.type}] {response.message}");
        };

        await websocket.Connect();
    }

    void Update()
    {
        #if !UNITY_WEBL || UNITY_EDITOR
            websocket.DispatchMessageQueue();
        #endif
    }

    async void SendLoginMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            ClientMessage msg = new ClientMessage();
            msg.type = "create_room";
            msg.username = "Player_01";

            string jsonMessage = JsonUtility.ToJson(msg);

            await websocket.SendText(jsonMessage);
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
