using System.Collections.Generic;
using UnityEngine;

public class TongueDrawManager : MonoBehaviour
{
    [SerializeField] Vector3 m_TongueEndOffset = new Vector3(0.0f, -0.15f, 0.0f);

    private LineRenderer m_TongueRenderer;
    private TongueBehaviour m_TongueBehaviour;
    private GameObject m_Frog;

    void Start()
    {
        m_TongueRenderer = GetComponent<LineRenderer>();
        m_TongueBehaviour = GameObject.Find("/Frog/TongueTIP_0").GetComponent<TongueBehaviour>();
        m_Frog = GameObject.Find("/Frog");
    }

    void Update()
    {
        List<GameObject> transforms = m_TongueBehaviour.GetPositions();
        m_TongueRenderer.positionCount = transforms.Count;

        for (int i = 0; i < m_TongueRenderer.positionCount - 1; i++)
        {
            m_TongueRenderer.SetPosition(i, transforms[i].transform.position);
        }

        m_TongueRenderer.SetPosition(m_TongueRenderer.positionCount-1, m_Frog.transform.position + m_TongueEndOffset);
    }
}
