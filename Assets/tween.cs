using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class tween : MonoBehaviour
{
    Tween tw;
    public bool on;
    // Start is called before the first frame update
    void Start()
    {
        tw = transform.DOMove(new Vector3(0, 0, 5), 2)
                    .SetRelative()
                    .SetRecyclable()
                    .SetAutoKill(false)
                    .Pause();
        //transform.DOMove(new Vector3(50, 1, 50), 3);
    }

    // Update is called once per frame
    void Update()
    {
        if(on)
        {
            tw.Play();
            on = false;
            Debug.Log("Played");
        }
    }
}
