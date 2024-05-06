using System.IO;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int width; // 맵의 가로 크기를 정의합니다.
    public int height; // 맵의 세로 크기를 정의합니다.
    public GameObject lowWall; // Inspector에서 설정할 낮은 벽 오브젝트를 참조합니다.
    public GameObject highWall; // Inspector에서 설정할 높은 벽 오브젝트를 참조합니다.

    private float planeSize; // Plane 오브젝트의 크기를 저장할 변수입니다.

    // Start 함수는 스크립트가 활성화될 때 한 번 호출됩니다.
    void Start()
    {
        planeSize = transform.localScale.x; // 현재 오브젝트(Plane)의 scale x 값을 가져와서 planeSize에 저장합니다.
        GenerateMap(); // 맵을 생성하는 함수를 호출합니다.
    }

    // GenerateMap 함수는 CSV 파일에서 맵 데이터를 읽고 맵을 생성합니다.
    void GenerateMap()
    {
        string[] lines = File.ReadAllLines("Assets/map.csv"); // 맵 데이터가 포함된 CSV 파일을 읽습니다.
        for (int y = 0; y < height; y++)
        {
            string[] tiles = lines[y].Split(','); // 각 줄을 쉼표로 분리하여 타일 데이터를 얻습니다.
            for (int x = 0; x < width; x++)
            {
                int tileType = int.Parse(tiles[x]); // 타일 타입을 정수로 변환합니다.
                PlaceTile(tileType, x, y); // 타일을 배치하는 함수를 호출합니다.
                
            }
        }
    }

    // PlaceTile 함수는 주어진 타일 타입에 따라 적절한 오브젝트를 생성하고 맵에 배치합니다.
    void PlaceTile(int tile, int x, int y)
    {
        GameObject toPlace = null;
        switch (tile)
        {
            case 1:
                toPlace = lowWall; // 낮은 벽 오브젝트 선택
                break;
            case 2:
                toPlace = highWall; // 높은 벽 오브젝트 선택
                break;
        }

        if (toPlace != null)
        {
            // 전체 맵 너비의 절반에서 한 타일의 절반 크기를 빼서 중앙 정렬을 위한 x, y 오프셋 계산
            float offsetX = planeSize * width / 2 - planeSize / 2;
            float offsetY = planeSize * height / 2 - planeSize / 2;

            // Instantiate를 사용하여 오브젝트를 생성하고, 위치를 조정하여 Plane의 중앙에 맞춤
            Instantiate(toPlace, new Vector3(x * planeSize - offsetX, 0, y * planeSize - offsetY), Quaternion.identity, transform);
        }
    }

}
