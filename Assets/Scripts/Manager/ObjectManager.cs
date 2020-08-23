using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public static ObjectManager instance;

    #region Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    #endregion Singleton

    public void KineToDyna(GameObject go_Kinematic)
        => go_Kinematic.GetComponent<Rigidbody>().isKinematic = false;

    public void DynaToKine(GameObject go_Dynamic)
        => go_Dynamic.GetComponent<Rigidbody>().isKinematic = true;
 
    // 소환한 마테리얼을 딕셔너리에 저장시켜 자원소모를 최소화
    private Dictionary<string, Material> MatDic = new Dictionary<string, Material>();
    

    public Material GetMatrialDic(string _key)
    {
        // 마테리얼 딕셔너리에 매개변수로 받은 key가 존재한다면 key에 해당하는 value 리턴
        if (MatDic.ContainsKey(_key))
            return MatDic[_key];

        // value를 만들어줄 _key 경로에서 마테리얼을 리소스에서 불러옵니다.
        Material _value = Resources.Load<Material>(_key);   
        // 처음 호출되는 것이기 때문에 key와 value를 딕셔너리에 저장시킵니다.
        MatDic.Add(_key ,_value);
        // value를 리턴합니다.
        return _value;
    }
}

