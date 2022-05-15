using System.Collections.Generic;
using UnityEngine;

public class TongueTipBehaviours : MonoBehaviour
{
    [SerializeField] private GameObject m_TongueSegmentPrefab;
    [SerializeField] private float m_RetractionForce = 100.0f;
    [SerializeField] private float m_RetractionForceMulti = 10.0f;
    [SerializeField] private int m_TongueSize = 50;

    private GameObject m_Frog;
    
    private int m_SegmentsCount = 3;
    private float m_SegmentsDistance = 0.5f;
    private List<GameObject> m_Positions = new List<GameObject>();

    void Start()
    {
        m_Frog = GameObject.Find("/Frog");
        GetTongueEnd().GetComponent<SpringJoint2D>().connectedAnchor = m_Frog.transform.position;

        m_Positions.Add(gameObject);
        m_Positions.Add(GetLastTongueSeg());
        m_Positions.Add(GetTongueEnd());
        m_Positions.Add(m_Frog);
    }

    void Update()
    {
        if (m_SegmentsCount < m_TongueSize && GetDistanceFromLastSegmentToFrog() > 1.0f)
        {
            CreateNewSegment();
            GetComponent<TongueTipControls>().IncreaseSpeedBasedOnLength();
        }
        else if (m_SegmentsCount > 3 && GetDistanceFromLastSegmentToFrog() < 0.5f)
        {
            DeleteLastSegment();
            GetComponent<TongueTipControls>().DecreaseSpeedBasedOnLength();
        }

        RetractTongue(Input.GetKey(KeyCode.E) ? m_RetractionForceMulti : 1.0f);
    }

    void CreateNewSegment()
    {
        GameObject tongueEnd = GetTongueEnd();

        // Create new segment
        GameObject newSegment = Instantiate(m_TongueSegmentPrefab, GetNextSegmentPosition(), Quaternion.identity);
        SpringJoint2D newSegmentJoint = newSegment.GetComponent<SpringJoint2D>();
        AssignTongueENDJointProperties(newSegmentJoint);

        // Attach new to last
        SpringJoint2D oldSegmentJoint = tongueEnd.GetComponent<SpringJoint2D>();
        oldSegmentJoint.connectedBody = newSegment.GetComponent<Rigidbody2D>();
        AssignTongueSEGJointProperties(oldSegmentJoint);
        
        // Swap names
        tongueEnd.name = "TongueSEG_" + (m_SegmentsCount - 1);
        newSegment.name = "TongueEND_" + m_SegmentsCount;

        m_SegmentsCount++;
        m_Positions[m_SegmentsCount-1] = newSegment;
        m_Positions.Add(m_Frog);
    }

    void DeleteLastSegment()
    {
        m_Positions.Remove(GetTongueEnd());
        Destroy(GetTongueEnd());

        GameObject lastTongueSeg = GetLastTongueSeg();
        SpringJoint2D lastTongueJoint = lastTongueSeg.GetComponent<SpringJoint2D>();
        AssignTongueENDJointProperties(lastTongueJoint);
        lastTongueSeg.name = "TongueEND_" + (m_SegmentsCount - 2);

        m_SegmentsCount--;
    }

    void RetractTongue(float multiplier)
    {
        //(int)(m_Positions.Count * 0.1f)
        /*
        for (int i = 1; i < m_Positions.Count - 1; i++)
        {
            GameObject prevSeg = m_Positions[i-1];
            GameObject currSeg = m_Positions[i];

            Vector2 moveTowards = prevSeg.transform.position - currSeg.transform.position;
            currSeg.GetComponent<Rigidbody2D>().AddForce(moveTowards * m_RetractionSpeed * multiplier * Time.deltaTime);
        }*/

        GameObject tongueEnd = GetTongueEnd();
        Vector2 moveTowardsFrog = m_Frog.transform.position - tongueEnd.transform.position;
        tongueEnd.GetComponent<Rigidbody2D>().AddForce(moveTowardsFrog * m_RetractionForce * multiplier * Time.deltaTime);
    }

    GameObject GetTongueEnd()
    {
        return GameObject.Find("/TongueEND_" + (m_SegmentsCount - 1));
    }

    GameObject GetLastTongueSeg()
    {
        return GameObject.Find("/TongueSEG_" + (m_SegmentsCount - 2));
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
        Vector3 lastSegmentPos = GetTongueEnd().transform.position;
        Vector3 frogPos = m_Frog.transform.position;
        Vector3 newPos = (lastSegmentPos + frogPos) * 0.5f;
        return newPos;
    }

    float GetDistanceFromLastSegmentToFrog()
    {
        Vector3 obj1 = GetTongueEnd().transform.position;
        Vector3 obj2 = m_Frog.transform.position;
        return Mathf.Sqrt( Mathf.Pow(obj2.x - obj1.x, 2) + Mathf.Pow(obj2.y - obj1.y, 2) );
    }

    public List<GameObject> GetPositions()
    {
        return m_Positions;
    }

}
