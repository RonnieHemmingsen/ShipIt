using UnityEngine;
using System.Collections;

public class Utilities : MonoBehaviour {

    /*Returns true if the distance to other is less than distVal*/
    public static bool DistanceLessThanValueToOther(Vector3 thisObjPos, Vector3 otherObjPos, float distVal)
    {
        float distance = 0.0f;

        distance = Vector3.Distance(thisObjPos, otherObjPos);
        if(distance > distVal)
            return true;
        else
            return false;
    }

    public static int RandomIntGenerator(int from, int to)
    {
        int random = Random.Range(from, to +1);
        return random;

    }
}
