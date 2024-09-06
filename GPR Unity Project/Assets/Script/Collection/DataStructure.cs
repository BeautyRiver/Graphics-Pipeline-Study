using UnityEngine;

public class DataStructure : MonoBehaviour
{
    public class Node<T>
    {
        //������ ������
        public T Data { get; set; }
        // ���� ��带 ����Ű�� ����
        public Node<T> Next { get; set; }
        // ������
        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }

    // ���׸� ��ũ�� ����Ʈ ť
    public class Queue<T>
    {
        // ť�� �� �� ���
        public Node<T> head;
        // ť�� �� �� ���
        public Node<T> tail;

        // ������
        public Queue()
        {
            head = null;
            tail = null;
        }

        // ť�� ����ִ��� Ȯ��
        public bool IsEmpty()
        {
            return head == null;
        }

        // ť�� �� �ڿ� ���ο� ������ �߰�
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
        
        // ť�� �� �� ������ ����, ��ȯ
        public T Dequeue()
        {
            if (IsEmpty())
            {
                Debug.Log("Queue�� �������");
                return default(T);
            }

            T value = head.Data;
            head = head.Next;

            if (head == null)
                tail = null;

            return value;
        }

        // ť�� �� �� �����͸� ��ȯ
        public T Peek()
        {
            if (IsEmpty())
            {
                Debug.Log("Queue�� �������");
                return default(T);
            }
            return head.Data;
        }
    }

    public class Stack<T>
    {
        public Queue<T> queue1 = new Queue<T>();
        public Queue<T> queue2 = new Queue<T>();

        public void Push(T data)
        {            
            queue2.Enqueue(data);

            // queue1�� ������� �ʴٸ� queue2�� ������ ���� �ű� 
            while (!queue1.IsEmpty())
            {
                queue2.Enqueue(queue1.Dequeue());
            }
            // temp�� �׻� ����ְ�
            Queue<T> tempQueue = queue1;
            queue1 = queue2; // �ٽ� queue1�� ������ ����
            queue2 = tempQueue; // queue2 �ʱ�ȭ
        }

        // ���ÿ��� ���� �������� �߰��� ��Ҹ� �����ϰ� ��ȯ Pop
        public T Pop()
        {           
            return queue1.Dequeue();
        }
        public T Peek()
        {
            return queue1.Peek();
        }

        // ������ ��� �ִ��� Ȯ��
        public bool IsEmpty()
        {
            return queue1.IsEmpty();
        }
    }


}

