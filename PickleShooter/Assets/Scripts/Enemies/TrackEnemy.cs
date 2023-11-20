using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackEnemy : MonoBehaviour
{
    public Transform target;
    public GameObject track;
    public int trackNum; // what track they are on (track 1 or 2)

    List<GameObject> tPoints = new List<GameObject>(); // track points
    GameObject tPoint; // track point
    int pointDex = 0;
    Vector3 pos; // position
    Quaternion rot; // rotation
    Transform entrance;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
        rot = transform.rotation;
        GameObject entrances = GameObject.Find("/Holes");
        
        if (entrances == null)
        {
            Debug.LogError("Cannot find 'Holes.'");
            return;
        }
        
        switch (trackNum)
        {
            case 1:
                entrance = entrances.transform.Find("Entrance_1");
                break;
            case 2:
                entrance = entrances.transform.Find("Entrance_2");
                break;
            default:
                Destroy(this.gameObject);
                Debug.LogError("Invalid trackNum. Object destroyed.");
                break;
        }   
        
        if (entrance == null)
        {
            Debug.LogError("Cannot find entrance for trackNum: " + trackNum);
            return;
        }
        
        for(int i = 0; i < 2; i++){
            tPoints.Add(track.transform.GetChild(3).GetChild(i).gameObject);
        }

        pos = entrance.position;
        pos.y = 12.22f;
    }

    void Update()
    {   
        tPoint = tPoints[pointDex];
        target = tPoint.transform;
        Debug.Log(target);
        if(target.position != pos){
            pos += (target.position - pos) * Time.deltaTime;
            pos.y = 12.22f;
        }else pointDex++;
        transform.position = pos;
    }
}
