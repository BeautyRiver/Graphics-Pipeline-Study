
using System;
using UnityEngine;

public class PlayerShot_Exam : DataStructure
{
    public Queue<GameObject> queue;
    public GameObject bullet;
    public Transform shotPoint;
    private int count;
    public static  Action<GameObject> EnqueueBullet;
    private void Awake()
    {
        queue = new Queue<GameObject>();
        MakeBullets(); // 초기에 총알 생성
    }
    private void OnEnable()
    {
        EnqueueBullet += Enqueue; // 이벤트 구독
    }

    private void OnDisable()
    {
        EnqueueBullet -= Enqueue; // 이벤트 구독 해제
    }

    public void Shot()
    {
        if (queue.IsEmpty())
            MakeBullets();
        
        GameObject obj = queue.Dequeue();
        obj.transform.position = shotPoint.position;
        obj.transform.rotation = shotPoint.rotation;
        obj.SetActive(true);
    }

    private void MakeBullets()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(bullet, shotPoint.position, Quaternion.identity);
            obj.name = "Bullet_" + count++;
            queue.Enqueue(obj);

            obj.SetActive(false);
        }        
    }

    private void Enqueue(GameObject bullet)
    {
        bullet.SetActive(false);
        queue.Enqueue(bullet);
    }
}
