using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzles : MonoBehaviour
{
    public static Puzzles inst;
    public bool started = false, finished = false, middlePuzzle = false;

    void Start()
    {
        inst = this;
    }

    void Update()
    {
        
    }
}
