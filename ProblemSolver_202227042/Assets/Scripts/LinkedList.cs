using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LinkedList : MonoBehaviour
{
    public class Node<T>
    {
        public T Value { get; set; }
        public Node<T> Next { get; set; }
        public Node(T value)
        {
            Value = value;
            Next = null;
        }
    }
    
    public class Queue<T>
    {
        private Node<T> head;
        private Node<T> tail;

        public Queue()
        {
            head = null;
            tail = null;
        }

        public bool IsEmpty()
        {
            return head == null;
        }

        public void Enquee(T value)
        {
            Node<T> newNode = new Node<T>(value);

        }
    }
}
