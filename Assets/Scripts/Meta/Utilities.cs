using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Utilities : MonoBehaviour {

    /*Returns true if the distance to other is larger than distVal*/
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
        int random = UnityEngine.Random.Range(from, to +1);
        return random;

    }

    public static IEnumerator FadeInAndOut(float fadeToValue, Image im, Action onComplete)
    {

        im.CrossFadeAlpha(fadeToValue, GameSettings.CROSSFADE_ALPHA_VALUE, false);
        yield return new WaitForSeconds(GameSettings.CROSSFADE_ALPHA_VALUE);
        onComplete();

    }


}
