using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

    private const string typeName = "UniqueGameName";
    private const string gameName = "RoomName";
    private HostData[] hostList;
    private bool notified = false;

    public int characterLayer;
    public GameObject playerPrefab;

    public static bool isConnected
    {
        get { return Network.isClient || Network.isServer; }
    }

	// Use this for initialization
	void Start () {
	    if (new ArrayList(System.Environment.GetCommandLineArgs()).Contains("+server"))
        {
            StartServer();
        }
	}

    private void StartServer()
    {
        Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
        EventManager.InvokeEvent("Network.ServerInit");
    }

    void OnServerInitialized()
    {
        SpawnPlayer();
        Debug.Log("Server Initializied");
    }

    private void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }

    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    void OnConnectedToServer()
    {
        SpawnPlayer();
        EventManager.InvokeEvent("Network.Connected");
        Debug.Log("Server Joined");
    }

    private void SpawnPlayer()
    {
        bool exist = GameObject.FindGameObjectsWithTag("Player").Length == 0;
        GameObject obj = Network.Instantiate(playerPrefab, transform.position, Quaternion.identity, 0) as GameObject;
        
        if (exist)
        {
            obj.GetComponentInChildren<Camera>().enabled = true;
        }
        else
        {
            obj.layer = characterLayer;
        }
    }

    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (notified)
            {
                EventManager.InvokeEvent("Network.Disconnected");
                notified = true;
            }

            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                StartServer();

            if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
                RefreshHostList();

            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }
        }
        else if (notified)
        {
            notified = false;
        }
    }
}
