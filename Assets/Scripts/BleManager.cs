using UnityEngine;
using UnityEngine.Android;

public class BleManager : MonoBehaviour
{
    void Start()
    {
        RequestBluetoothPermissions();
    }

    private void RequestBluetoothPermissions()
    {
        #if UNITY_ANDROID
            Debug.Log("Checking Android BLE permissions...");

            if (!Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_SCAN")) {
                Permission.RequestUserPermission("android.permission.BLUETOOTH_SCAN");
            }
            if (!Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADVERTISE")) {
                Permission.RequestUserPermission("android.permission.BLUETOOTH_ADVERTISE");
            }
            if (!Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_CONNECT")) {
                Permission.RequestUserPermission("android.permission.BLUETOOTH_CONNECT");
            }

            if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation)) {
                Permission.RequestUserPermission(Permission.FineLocation);
            }

            Debug.Log("Permission check completed.");
        #endif
    }
}
