using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 여러개의 오브젝트들의 Order in Layer를 한꺼번에 설정해주는 기능을 담은 스크립트
/// TextMeshPro 같이 인스펙터 상에서 Order in Layer를 설정하지 못하는 오브젝트들한테 필수
/// </summary>
public class Order : MonoBehaviour
{
    // 인스펙터
    [SerializeField] Renderer[] backRenderers; // 맨 뒤로 배치하는 오브젝트들
    [SerializeField] Renderer[] middleRenderers; // 비교적 중간에 오는 오브젝트들
    [SerializeField] string sortingLayerName;
    int originOrder; // 기존 순서로 복귀를 위해 백업

    private void Start()
    {
        SetOriginOrder(originOrder);
    }

    // 기존 순서로 되돌림
    public void SetOriginOrder(int originOrder)
    {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }

    // 제일 앞으로 배치
    public void SetMostFrontOrder(bool isMostFront)
    {
        SetOrder(isMostFront ? 100 : originOrder);
    }

    // 모든 오브젝트들 순서 설정
    public void SetOrder(int order)
    {
        int mulOrder = order * 10; // 오류 방지를 위해 10 단위로 순서 배치

        // 뒤에 애들부터
        foreach (var renderer in backRenderers)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder;
        }
        // 그 이후 가운데
        foreach (var renderer in middleRenderers)
        {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder + 1;
        }
    }
}
