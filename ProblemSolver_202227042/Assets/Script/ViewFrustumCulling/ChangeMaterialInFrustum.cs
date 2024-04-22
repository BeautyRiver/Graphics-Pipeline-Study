using Unity.VisualScripting;
using UnityEngine;

// 카메라 프러스텀에 따라 오브젝트의 머티리얼을 변경하는 스크립트
public class ChangeMaterialInFrustum : MonoBehaviour
{
    public Material material1; // 프러스텀 내부에 있는 오브젝트에 적용될 머티리얼
    public Material material2; // 프러스텀 밖에 있는 오브젝트에 적용될 머티리얼

    private Camera thisCamera; // 이 스크립트가 부착된 카메라

    private void Start()
    {
        // 스크립트가 부착된 게임 오브젝트에서 카메라 컴포넌트를 가져옴
        thisCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        // 카메라 컴포넌트가 없으면 에러 메시지를 출력하고 함수를 종료
        if (thisCamera == null)
        {
            Debug.LogError("카메라 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        // 카메라의 프러스텀 평면을 계산
        FrustumPlanes frustum = new FrustumPlanes(thisCamera);

        // 모든 씬에 있는 모든 Renderer 컴포넌트를 가져옴
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // 각 렌더러의 bounding box 크기를 계산
            Vector3 size = renderer.bounds.size;
            // bounding box의 최대 직경을 계산
            float diameter = Mathf.Max(size.x, Mathf.Max(size.y, size.z));

            // Renderer의 중심점이 프러스텀 내에 있는지 확인
            if (frustum.IsInsideFrustum(renderer.bounds))
            {
                // 프러스텀 내에 있는 경우 Material1 적용
                renderer.material = material1;
            }
            else
            {
                // 프러스텀 밖에 있는 경우 Material2 적용
                renderer.material = material2;
            }
        }
    }
}

// 카메라 프러스텀 평면을 관리하는 클래스
public class FrustumPlanes
{
    private readonly Plane[] planes; // 프러스텀을 구성하는 평면 배열

    public FrustumPlanes(Camera camera)
    {
        // 카메라의 프러스텀 평면을 계산하여 저장
        planes = GeometryUtility.CalculateFrustumPlanes(camera);
    }

    // 지정된 점과 반지름을 가진 구가 프러스텀 내부에 있는지 판단하는 함수
    public bool IsInsideFrustum(Bounds bounds)
    {
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }
}
