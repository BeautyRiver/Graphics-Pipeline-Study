using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width; // ���� ���� ũ�⸦ �����մϴ�.
    public int height; // ���� ���� ũ�⸦ �����մϴ�.
    public GameObject lowWall; // Inspector���� ������ ���� �� ������Ʈ�� �����մϴ�.
    public GameObject highWall; // Inspector���� ������ ���� �� ������Ʈ�� �����մϴ�.

    private float planeSize; // Plane ������Ʈ�� ũ�⸦ ������ �����Դϴ�.

    // Start �Լ��� ��ũ��Ʈ�� Ȱ��ȭ�� �� �� �� ȣ��˴ϴ�.
    void Start()
    {
        planeSize = transform.localScale.x; // ���� ������Ʈ(Plane)�� scale x ���� �����ͼ� planeSize�� �����մϴ�.
        GenerateMap(); // ���� �����ϴ� �Լ��� ȣ���մϴ�.
    }

    // GenerateMap �Լ��� CSV ���Ͽ��� �� �����͸� �а� ���� �����մϴ�.
    void GenerateMap()
    {
        string[] lines = File.ReadAllLines("Assets/map.csv"); // �� �����Ͱ� ���Ե� CSV ������ �н��ϴ�.
        for (int y = 0; y < height; y++)
        {
            string[] tiles = lines[y].Split(','); // �� ���� ��ǥ�� �и��Ͽ� Ÿ�� �����͸� ����ϴ�.
            for (int x = 0; x < width; x++)
            {
                int tileType = int.Parse(tiles[x]); // Ÿ�� Ÿ���� ������ ��ȯ�մϴ�.
                PlaceTile(tileType, x, y); // Ÿ���� ��ġ�ϴ� �Լ��� ȣ���մϴ�.
                
            }
        }
    }

    // PlaceTile �Լ��� �־��� Ÿ�� Ÿ�Կ� ���� ������ ������Ʈ�� �����ϰ� �ʿ� ��ġ�մϴ�.
    void PlaceTile(int tile, int x, int y)
    {
        GameObject toPlace = null;
        switch (tile)
        {
            case 1:
                toPlace = lowWall; // ���� �� ������Ʈ ����
                break;
            case 2:
                toPlace = highWall; // ���� �� ������Ʈ ����
                break;
        }

        if (toPlace != null)
        {
            // ��ü �� �ʺ��� ���ݿ��� �� Ÿ���� ���� ũ�⸦ ���� �߾� ������ ���� x, y ������ ���
            float offsetX = planeSize * width / 2 - planeSize / 2;
            float offsetY = planeSize * height / 2 - planeSize / 2;

            // Instantiate�� ����Ͽ� ������Ʈ�� �����ϰ�, ��ġ�� �����Ͽ� Plane�� �߾ӿ� ����
            Instantiate(toPlace, new Vector3(x * planeSize - offsetX, 0, y * planeSize - offsetY), Quaternion.identity, transform);
        }
    }

}
