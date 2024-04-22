using Unity.VisualScripting;
using UnityEngine;

// ī�޶� �������ҿ� ���� ������Ʈ�� ��Ƽ������ �����ϴ� ��ũ��Ʈ
public class ChangeMaterialInFrustum : MonoBehaviour
{
    public Material material1; // �������� ���ο� �ִ� ������Ʈ�� ����� ��Ƽ����
    public Material material2; // �������� �ۿ� �ִ� ������Ʈ�� ����� ��Ƽ����

    private Camera thisCamera; // �� ��ũ��Ʈ�� ������ ī�޶�

    private void Start()
    {
        // ��ũ��Ʈ�� ������ ���� ������Ʈ���� ī�޶� ������Ʈ�� ������
        thisCamera = GetComponent<Camera>();
    }

    private void Update()
    {
        // ī�޶� ������Ʈ�� ������ ���� �޽����� ����ϰ� �Լ��� ����
        if (thisCamera == null)
        {
            Debug.LogError("ī�޶� ������Ʈ�� ã�� �� �����ϴ�.");
            return;
        }

        // ī�޶��� �������� ����� ���
        FrustumPlanes frustum = new FrustumPlanes(thisCamera);

        // ��� ���� �ִ� ��� Renderer ������Ʈ�� ������
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            // �� �������� bounding box ũ�⸦ ���
            Vector3 size = renderer.bounds.size;
            // bounding box�� �ִ� ������ ���
            float diameter = Mathf.Max(size.x, Mathf.Max(size.y, size.z));

            // Renderer�� �߽����� �������� ���� �ִ��� Ȯ��
            if (frustum.IsInsideFrustum(renderer.bounds))
            {
                // �������� ���� �ִ� ��� Material1 ����
                renderer.material = material1;
            }
            else
            {
                // �������� �ۿ� �ִ� ��� Material2 ����
                renderer.material = material2;
            }
        }
    }
}

// ī�޶� �������� ����� �����ϴ� Ŭ����
public class FrustumPlanes
{
    private readonly Plane[] planes; // ���������� �����ϴ� ��� �迭

    public FrustumPlanes(Camera camera)
    {
        // ī�޶��� �������� ����� ����Ͽ� ����
        planes = GeometryUtility.CalculateFrustumPlanes(camera);
    }

    // ������ ���� �������� ���� ���� �������� ���ο� �ִ��� �Ǵ��ϴ� �Լ�
    public bool IsInsideFrustum(Bounds bounds)
    {
        return GeometryUtility.TestPlanesAABB(planes, bounds);
    }
}
