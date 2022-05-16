using System.Collections.Generic;
using UnityEngine;

public class TongueTipBehaviours : MonoBehaviour
{
    [SerializeField] private GameObject m_TongueSegmentPrefab;
    [SerializeField] private float m_RetractionForce = 100.0f;
    [SerializeField] private int m_MaxTongueSize = 50;

    private GameObject m_Frog;
    
    private float m_SegmentsDistance = 0.5f;
    private List<GameObject> m_Positions = new List<GameObject>();

    void Start()
    {
        m_Frog = GameObject.Find("/Frog");
        
        m_Positions.Add(gameObject);
        m_Positions.Add(GameObject.Find("/TongueSEG_1"));
        m_Positions.Add(GameObject.Find("/TongueEND_2"));

        GetTongueEND().GetComponent<SpringJoint2D>().connectedAnchor = m_Frog.transform.position;
    }

    void Update()
    {
        if (m_Positions[m_Positions.Count-1] == null)
            Debug.Log("");

        if (!GameplayManager.IsGamePlaying() && !GameplayManager.IsGameWon())
            return;

        if (GetTongueSize() < m_MaxTongueSize && GetDistanceFromLastSegmentToFrog() > m_SegmentsDistance * 2.0f)
        {
            CreateNewSegment();
        }
        else if (GetTongueSize() > 3 && GetDistanceFromLastSegmentToFrog() < m_SegmentsDistance)
        {
            DeleteLastSegment();
        }

        RetractTongue();
    }

    void CreateNewSegment()
    {
        GameObject tongueEnd = GetTongueEND();

        // Create new segment
        GameObject newSegment = Instantiate(m_TongueSegmentPrefab, GetNextSegmentPosition(), Quaternion.identity);
        SpringJoint2D newSegmentJoint = newSegment.GetComponent<SpringJoint2D>();
        AssignTongueENDJointProperties(newSegmentJoint);

        // Attach new to last
        SpringJoint2D oldSegmentJoint = tongueEnd.GetComponent<SpringJoint2D>();
        oldSegmentJoint.connectedBody = newSegment.GetComponent<Rigidbody2D>();
        AssignTongueSEGJointProperties(oldSegmentJoint);
        
        // Swap names
        tongueEnd.name = "TongueSEG_" + (GetTongueSize() - 1);
        newSegment.name = "TongueEND_" + GetTongueSize();
        m_Positions.Add(newSegment);
    }

    void DeleteLastSegment()
    {
        GameObject lastTongueSeg = GetLastTongueSEG();
        SpringJoint2D lastTongueJoint = lastTongueSeg.GetComponent<SpringJoint2D>();
        AssignTongueENDJointProperties(lastTongueJoint);
        lastTongueSeg.name = "TongueEND_" + (GetTongueSize() - 2);

        GameObject tongueEnd = GetTongueEND();
        m_Positions.Remove(tongueEnd);
        Destroy(tongueEnd);
    }

    void RetractTongue()
    {
        Vector2 moveTowardsFrog = m_Frog.transform.position - GetTongueEND().transform.position;
        GetTongueEND().GetComponent<Rigidbody2D>().AddForce(moveTowardsFrog * m_RetractionForce * Time.deltaTime);
    }

    void AssignTongueENDJointProperties(SpringJoint2D joint)
    {
        joint.distance = m_SegmentsDistance;
        joint.dampingRatio = 1.0f;
        joint.frequency = 1.2f;
        joint.autoConfigureDistance = false;
        joint.connectedAnchor = new Vector2(m_Frog.transform.position.x, m_Frog.transform.position.y - 0.15f);
        joint.connectedBody = null;
    }

    void AssignTongueSEGJointProperties(SpringJoint2D joint)
    {
        joint.distance = m_SegmentsDistance;
        joint.connectedAnchor = Vector2.zero;
        joint.frequency = 0.0f;
    }

    Vector3 GetNextSegmentPosition()
    {
        Vector3 lastSegmentPos = GetTongueEND().transform.position;
        Vector3 frogPos = m_Frog.transform.position;
        return (lastSegmentPos + frogPos) * 0.5f;
    }

    float GetDistanceFromLastSegmentToFrog()
    {
        Vector3 obj1 = GetTongueEND().transform.position;
        Vector3 obj2 = m_Frog.transform.position;
        return Mathf.Sqrt( Mathf.Pow(obj2.x - obj1.x, 2) + Mathf.Pow(obj2.y - obj1.y, 2) );
    }

    GameObject GetLastTongueSEG()
    {
        return m_Positions[m_Positions.Count - 2];
    }

    GameObject GetTongueEND()
    {
        return m_Positions[m_Positions.Count - 1];
    }

    public List<GameObject> GetPositions()
    {
        return m_Positions;
    }

    int GetTongueSize()
    {
        return m_Positions.Count;
    }

}
