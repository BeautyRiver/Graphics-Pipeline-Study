using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStructure_Exam : MonoBehaviour
{
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Next { get; set; }

        public Node(T Data)
        {
            this.Data = Data;
            Next = null;
        }
    }

    public class Queue<T>
    {
        public Node<T> Head { get; set; }
        public Node<T> Tail { get; set; }
        public Queue()
        {
            Head = null;
            Tail = null;
        }

        public bool IsEmpty()
        {
            return Head == null;
        }
        public void Enqueue(T data)
        {
            Node<T> newNode = new Node<T>(data);
            if(IsEmpty() == true)
            {
                Head = newNode;
                Tail = newNode;
            }
            else
            {
                Tail.Next = newNode;
                Tail = newNode;
            }
        }
        public T Dequeue()
        {
            if (IsEmpty() == true)
            {
                Debug.Log("Queue가 비어있음");
                return default(T);
            }

            T value = Head.Data;
            Head = Head.Next;
            
            if (Head == null)
                Tail = null;

            return value;
        }

        public T Peek()
        {
            if (IsEmpty() == true)
            {
                Debug.Log("Queue가 비어있음");
                return default(T);
            }
            return Head.Data;
        }
    }
}
