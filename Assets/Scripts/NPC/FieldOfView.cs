using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private float viewAngle = 0.0f;       // 시야각
    [SerializeField] private float viewDistance = 0.0f;    // 시야거리
    [SerializeField] private LayerMask targetMask = 0;  // 레이어마스크 

    RaycastHit _hitInfo;

    void Update()
    {
        View();    
    }

    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for(int i = 0; i < _target.Length; i++)
        {
            Transform _targerTf = _target[i].transform;
            if (_targerTf.CompareTag("Player")) 
            {
                Vector3 direction = (_targerTf.position - transform.position).normalized;   // V1 - V2 는 V1를 바라보는 벡터
                float _angle = Vector3.Angle(direction, transform.forward);

                if(_angle <viewAngle * 0.5f)
                {
                    if (Physics.Raycast(transform.position + transform.up, direction, out _hitInfo, viewDistance))
                    {
                        if (_hitInfo.transform.CompareTag("Player"))
                        {
                            Debug.Log("플레이어가 좀비 시야 내에 있습니다");
                            Debug.DrawRay(transform.position + transform.up, direction, Color.blue);
                        }
                    }
                }
            }
        }
    }
}
