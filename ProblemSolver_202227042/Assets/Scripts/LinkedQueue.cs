using UnityEngine;

public class LinkedQueue : MonoBehaviour
{
    public class Node<T>
    {
        //저장할 데이터
        public T Data { get; set; }
        // 다음 노드를 가리키는 변수
        public Node<T> Next { get; set; }
        // 생성자
        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }

    // 제네릭 링크드 리스트 큐
    public class LinkQueue<T>
    {
        // 큐의 맨 앞 노드
        public Node<T> head;
        // 큐의 맨 뒤 노드
        public Node<T> tail;

        // 생성자
        public LinkQueue()
        {
            head = null;
            tail = null;
        }

        // 큐가 비어있는지 확인
        public bool IsEmpty()
        {
            return head == null;
        }

        // 큐의 맨 뒤에 새로운 데이터 추가
        public void Enqueue(T data)
        {
            Node<T> newNode = new Node<T>(data);
            if (IsEmpty())
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                tail = newNode;
            }
        }

        // 큐의 맨 앞 데이터 제거, 반환
        public T Dequeue()
        {
            if (IsEmpty())
            {
                Debug.Log("Queue가 비어있음");  
                return default(T);
            }

            T value = head.Data;
            head = head.Next;

            if (head == null)
                tail = null;

            return value;
        }

        // 큐의 맨 앞 데이터를 반환
        public T Peek()
        {
            if (IsEmpty())
            {
                Debug.Log("Queue가 비어있음");
                return default(T);
            }
            return head.Data;
        }
    }
}
