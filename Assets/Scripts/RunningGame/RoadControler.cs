using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadControler : MonoBehaviour
{
    public List<GameObject> Road;
    public GameObject exitObject;
    public GameObject lastObject;

    private void IntansiateRoad()
    {
        int road = Random.Range(0, Road.Count - 1);
        Vector3 pos = lastObject.transform.position + new Vector3(0, 0, 103.46f);
        GameObject newObject = Instantiate(Road[road], pos, lastObject.transform.rotation);
        lastObject = newObject;
    }

    public void DestroyexitObject(GameObject exitObject)
    {
        IntansiateRoad();
        Destroy(exitObject);
    }
}
