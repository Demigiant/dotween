using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class TempPro : MonoBehaviour
{
    public GameObject target;

    IEnumerator Start()
    {
        GameObject newPath = Instantiate(target);
        newPath.transform.SetParent(this.transform.parent, false);
        yield return new WaitForSeconds(1);

        newPath = Instantiate(target);
        newPath.transform.SetParent(this.transform.parent, false);
        yield return new WaitForSeconds(2);
    }
}