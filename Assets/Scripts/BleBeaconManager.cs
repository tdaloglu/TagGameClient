using UnityEngine;
using System.Collections.Generic;

public class BleBeaconManager : MonoBehaviour
{
    private string gameServiceUUID = "12345678-1234-1234-1234-123456789ABC";

    private int filterSize = 5;
    private int tagThreshold = -50;

    public Dictionary<string, Queue<int>> playerRssiHistory = new Dictionary<string, Queue<int>>();

    private bool isScanning = false;
    private bool isBroadcasting = false;

    void Start()
    {
        Debug.Log("BleBeaconManager initialized.");
    } 

    public void StartBroadcasting(string myUsername)
    {
        if (isBroadcasting) return;
        Debug.Log($"Broadcasting started as: {myUsername}");
        isBroadcasting = true;
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
        Debug.Log("Scanning started.");
        isScanning = true;
    }

    public void StopScanning()
    {
        if (!isScanning) return;
        Debug.Log("Scanning stopped.");
        isScanning = false;
    }

    public void OnPlayerFound(string foundUsername, int rssi)
    {
        if (!playerRssiHistory.ContainsKey(foundUsername))
        {
            playerRssiHistory.Add(foundUsername, new Queue<int>());
            Debug.Log($"New player enters the coverage area: {foundUsername}");
        }

        Queue<int> history = playerRssiHistory[foundUsername];
        history.Enqueue(rssi);

        if (history.Count > filterSize)
        {
            history.Dequeue();
        }

        int averageRssi = CalculateAverage(history);

        CheckDistance(foundUsername, averageRssi);
    }

    private int CalculateAverage(Queue<int> queue)
    {
        int sum = 0;
        foreach (int val in queue)
        {
            sum += val;
        }
        return sum / queue.Count;
    }

    private void CheckDistance(string username, int avgRssi)
    {
        if (avgRssi >= tagThreshold)
        {
            Debug.Log($"ATTENTION! {username} IS VERY CLOSE! (Average: {avgRssi})");
        } else if (avgRssi >= -80)
        {
            Debug.Log($"{username} is nearby. (Average: {avgRssi})");
        } else
        {
            Debug.Log($"{username} is far away. (Average: {avgRssi})");
        }
    }
}