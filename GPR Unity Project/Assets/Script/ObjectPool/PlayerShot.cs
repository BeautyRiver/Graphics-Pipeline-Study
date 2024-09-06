using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : DataStructure
{
    public Queue<GameObject> queue;
    public Stack<GameObject> stack;
    public GameObject bullet;
    public float fireDelay = 0.5f; // �߻� ������ ������ �ð� (��)
    private float lastFireTime = 0f; // ������ �߻� �ð�
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

            // Queue�϶�
            if (selectDataStruct == DataStructure.Queue)
            {
                obj = queue.Dequeue();
                if (queue.IsEmpty())
                    MakeBullets();
            }

            // Stack�϶�
            else if (selectDataStruct == DataStructure.Stack)
            {
                obj = stack.Pop();
                if (stack.IsEmpty())
                    MakeBullets();
            }

            // SelectDataStructure ���� �϶�
            else
            {
                obj = null;
                Debug.Log("DataStructure ���� ����");
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

            // Queue�϶�
            if (selectDataStruct == DataStructure.Queue)
                queue.Enqueue(obj);

            // Stack �϶�
            else if (selectDataStruct == DataStructure.Stack)
                stack.Push(obj);

            // ���� ó��
            else
            {
                Debug.Log("DataStructure ���� ���� // �Ѿ� ���� �Ұ�");
                return;
            }

            obj.SetActive(false);
        }       
    }
}
