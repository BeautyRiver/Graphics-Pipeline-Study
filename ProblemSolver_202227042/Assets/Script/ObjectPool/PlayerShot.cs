using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : DataStructure
{
    public Queue<GameObject> queue;
    public Stack<GameObject> stack;
    public GameObject bullet;
    public float fireDelay = 0.5f; // 발사 사이의 딜레이 시간 (초)
    private float lastFireTime = 0f; // 마지막 발사 시간
    private int number = 0;
    public enum DataStructure
    {
        Queue,
        Stack
    };
    public DataStructure selectDataStruct;
    private void Awake()
    {
        stack = new Stack<GameObject>();
        queue = new Queue<GameObject>();
        MakeBullets();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= lastFireTime + fireDelay)
        {
            GameObject obj;

            // Queue일때
            if (selectDataStruct == DataStructure.Queue)
            {
                obj = queue.Dequeue();
                if (queue.IsEmpty())
                    MakeBullets();
            }

            // Stack일때
            else if (selectDataStruct == DataStructure.Stack)
            {
                obj = stack.Pop();
                if (stack.IsEmpty())
                    MakeBullets();
            }

            // SelectDataStructure 오류 일때
            else
            {
                obj = null;
                Debug.Log("DataStructure 설정 오류");
                return;
            }

            if (obj != null)
            {
                Debug.Log("Dequeued bullet: " + obj.name);
                obj.SetActive(true);
            }

            else            
                Debug.Log("Failed to dequeue a bullet.");
                        
            lastFireTime = Time.time;
        }
    }

    private void MakeBullets()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity, transform);
            obj.name = "Bullet_" + number++;

            // Queue일때
            if (selectDataStruct == DataStructure.Queue)
                queue.Enqueue(obj);

            // Stack 일때
            else if (selectDataStruct == DataStructure.Stack)
                stack.Push(obj);

            // 예외 처리
            else
            {
                Debug.Log("DataStructure 설정 오류 // 총알 생성 불가");
                return;
            }

            obj.SetActive(false);
        }       
    }
}
