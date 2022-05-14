using System.Collections.Generic;
using UnityEngine;

public class TongueDrawManager : MonoBehaviour
{
    private LineRenderer m_TongueRenderer;
    private TongueTipBehaviours m_TongueTipBehaviours;

    void Start()
    {
        m_TongueRenderer = GetComponent<LineRenderer>();
        m_TongueTipBehaviours = GameObject.Find("/TongueTIP_0").GetComponent<TongueTipBehaviours>();
    }

    void Update()
    {
        List<GameObject> transforms = m_TongueTipBehaviours.GetPositions();
        m_TongueRenderer.positionCount = transforms.Count;

        for (int i = 0; i < m_TongueRenderer.positionCount; i++)
        {
            m_TongueRenderer.SetPosition(i, transforms[i].transform.position);
        }
        
    }
}
