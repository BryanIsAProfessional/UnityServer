using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    public GameObject playerPrefab;
    public GameObject projectilePrefab;
    private void Awake(){
        if(instance == null){
            instance = this;
        }
        else if(instance != this){
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start(){
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = Constants.TICKS_PER_SECOND;

        #if UNITY_EDITOR
        Debug.Log("Build the server to run it!");
        #else
        Server.Start(Constants.MAX_PLAYERS, Constants.PORT);
        #endif
    }

    private void OnApplicationQuit(){
        Server.Stop();
    }
    public Player InstantiatePlayer(){
        return Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
    }
    public Player InstantiatePlayer(Vector3 _startingLocation){
        return Instantiate(playerPrefab, _startingLocation, Quaternion.identity).GetComponent<Player>();
    }

    public Player InstantiatePlayer(int _x, int _y, int _z){
        return Instantiate(playerPrefab, new Vector3(_x, _y, _z), Quaternion.identity).GetComponent<Player>();
    }

    public Player InstantiatePlayer(int _x, int _y, int _z, int _a, int _b, int _c, int _d){
        return Instantiate(playerPrefab, new Vector3(_x, _y, _z), new Quaternion(_a, _b, _c, _d)).GetComponent<Player>();
    }

    public Player InstantiatePlayer(Vector3 _startingLocation, Quaternion _startingRotation){
        return Instantiate(playerPrefab, _startingLocation, _startingRotation).GetComponent<Player>();
    }

    public Projectile InstantiateProjectile(Transform _shootOrigin){
        return Instantiate(projectilePrefab, _shootOrigin.position + _shootOrigin.forward * 0.7f, Quaternion.identity).GetComponent<Projectile>();
    }
}
