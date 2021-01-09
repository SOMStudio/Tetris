using System;
using System.Collections;
using System.Collections.Generic;
using Tetris;
using UnityEngine;

public class DropObjectManagerUi : MonoBehaviour
{
    public ObjectFieldUi[] predefineObject;

    private GameObject[] predefineObjectGo;

    private void Start()
    {
        predefineObjectGo = new GameObject[predefineObject.Length];
        
        for (int i = 0; i < predefineObject.Length; i++)
        {
            predefineObjectGo[i] = predefineObject[i].gameObject;
        }
    }

    public void SetDropObject(int num, ObjectField objectField)
    {
        if (num < predefineObject.Length)
        {
            predefineObjectGo[num].SetActive(true);

            predefineObject[num].ShowObject(objectField);
        }
    }

    public void SetDropObjectList(IEnumerable<ObjectField> objectFieldList)
    {
        int i = 0;
        foreach (var objectField in objectFieldList)
        {
           SetDropObject(i, objectField);
           i++;
        }
    }
}
