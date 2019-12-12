using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TempTests : BrainBase
{
    private Sequence parent;
    private float duration;
 
    // Empty Scene with 3 cubes next to each other named Cube1, Cube2, Cube3.
    // Empty game object with script on it. 2 check boxes in inspector to fire the methods.
    // Child sequence is a one off. Parent sequence has a restart on complete.
    // If you fire parent first, and the boxes do not adhere to the time they just fly away.
    // If you fire child first, then fire parent, and the parent sequence uses times.
 
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F5)) {
            DOTween.KillAll();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            Debug.Log("Create child");
            createChild();
        }
 
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Debug.Log("Create parent");
            createParent();
        }
    }
 
    public void createParent()
    {
        parent = DOTween.Sequence()
            .SetAutoKill(false)
            .AppendCallback(() => createChild());
        Debug.Log("- " + duration);
        parent.AppendInterval(duration)
            .OnComplete(() => {
                Debug.Log("<color=#00ff00>RESTART</color>");
                parent.Restart();
            });
    }
 
    public void createChild()
    {
        var listy = new List<GameObject>
        {
            GameObject.Find("Cube1"),
            GameObject.Find("Cube2"),
            GameObject.Find("Cube3")
        };
 
        var child = DOTween.Sequence();
        setCallBack(listy, child);
        duration = child.Duration();
        Debug.Log(">>> duration: " + duration);
    }
 
    private void setCallBack(List<GameObject> listy, Sequence child)
    {
        foreach (var thing in listy)
        {
            var rand = (float)rando();
            Debug.Log("Wait " + thing.name + " " + rand.ToString());
            child.AppendCallback(() => planToMove(thing.transform)).AppendInterval(rand); // move each, append a wait interval
        }
    }
 
    public int rando()
    {
        var rnd = new System.Random();
        return rnd.Next(1, 5);      
    }
 
    public void planToMove(Transform thing)
    {
        var rand = rando();
        thing.DOMove(new Vector3(thing.position.x, thing.position.y, thing.position.z + rand), .5f); //move the z a random amount beween 1-3.
        Debug.Log("Move " + thing.name + " " + rand.ToString());
    }
}