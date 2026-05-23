using UnityEngine;
using System.Collections.Generic;

public class BleBeaconManager : MonoBehaviour
{
    private string gameServiceUUID = "12345678-1234-1234-1234-123456789ABC";

    public Dictionary<string, int> nearbyPlayers = new Dictionary<string, int>();

    private bool isScanning = false;
    private bool isBroadcasting = false;

    void Start()
    {
        Debug.Log("BleBeaconManager initialized.");
    }

    public void StartBroadcasting(string myUsername)
    {
        if (isBroadcasting) return;

        Debug.Log($"Broadcasting started as: {myUsername} with UUID: {gameServiceUUID}");
        isBroadcasting = true;

        //TO-DO: Buraya iOS/Android native broadcast kodu gelecek.
    }

    public void StopBroadcasting()
    {
        if (!isBroadcasting) return;

        Debug.Log("Broadcasting stopped.");
        isBroadcasting = false;
    }

    public void StartScanning()
    {
        if (isScanning) return;

        Debug.Log($"Scannig started for UUID: {gameServiceUUID}");
        isScanning = true;

        //TO-DO: Buraya iOS/Android native scan kodu gelecek.
    }

    public void StopScanning()
    {
        if (!isScanning) return;

        Debug.Log("Scanning stopped.");
        isScanning = false;
    }

    public void OnPlayerFound(string foundUsername, int rssi)
    {
        if (nearbyPlayers.ContainsKey(foundUsername))
        {
            nearbyPlayers[foundUsername] = rssi;
        } 
        else
        {
            nearbyPlayers.Add(foundUsername, rssi);
            Debug.Log($"New player enters the coverage area: {foundUsername}");
        }

        CheckDistance(foundUsername, rssi);
    }

    private void CheckDistance(string username, int rssi)
    {
        Debug.Log($"Signal strength between {username} and us: {rssi}");
    }
}
