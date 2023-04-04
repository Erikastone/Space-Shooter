using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;
    [Header("Set Dynamically")]
    public float camZ;//�������� ���������� Z ������
    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    private void Awake()
    {
        camZ = this.transform.position.z;
    }
    private void FixedUpdate()
    {
        /* //����� ���� ��� ������������� �������
         if (POI == null) return;
         //�������� ������� ������������� �������
         Vector3 desnation = POI.transform.position;
        */

        Vector3 destination;
        //���� ��� ������������� �������, ������� 0 0 0
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            //�������� ������� ������������� �������
            destination = POI.transform.position;
            //���� ������������ ������ - ������, ���������,  ��� �� �����������
            if (POI.tag == "Projectile")
            {
                //���� �� ����� �� ����� 
                if (POI.GetComponent<Rigidbody>().IsSleeping())/*������, ����� ������ ��������� ����� � ����������� (�� ���� ����� �����
Rigidbody. IsSleeping() ������ �������� ������), �������� FollowCam ��������
null ���� P0I, � ������ �������� � �������� �������.*/
                {
                    //������� �������� ��������� ���� ������ ������
                    POI = null;
                    //� �� �����
                    return;
                }
            }
        }
        //���������� X � Y ������������ ����������
        /*
         * ������� Mathf .���() �������� ������������
           �� ���� ���������� ��������. � ������ �������� ������ ����� �������������
           ���������� X, ������� Mathf .���() �����������, ��� ������ ������� �� ���������
           ����� X = 0 � ������������� �������. ����������, ������ ����� Mathf .���() �� ��������
           ������ ���������� ���� ��������� Y = 0, ����� ���������� Y ������� ������
           �������������.*/
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //���������� ����� ����� ������� ��������������� ������ � DESTINATION
        /*
         * ����� Vector3. Lerp () ��������� ������������ ����� ����� �������, ���������
        ���������� �������. ���� ���� easing ��������� 0, Lerp() ������ ������
        ����� (transform, position); ���� ���� easing ��������� 1, Lerp() ������ ������
        ����� (destination). ���� ���� easing ��������� ����� ������ ��������
        � ��������� ����� 0 � 1, Lerp() ������ �����, ����������� ����� ���������
        (��� �������� 0,5 � ���� easing ����� ���������� �����, ������� ����� ����������).
        ����������� easing = 0.05f ��������, ��� � ������ ������ FixedUpdate
        (�� ���� ��� ������ ����������, ����������� ���������� �������, �������
        ���������� � �������� 50 ��� � �������) ������ ������ ������������ ��������
        �� 5% �� ���������� ����� ������� �� ��������������� � ���������������
        POI. ��� ��� P0I ��������� ������������, ������ ������ ������� �� ���.
        */
        destination = Vector3.Lerp(transform.position, destination, easing);
        //������������� ���������� �������� DESTINATION.Z == CAMZ,
        //����� ���������� ������ ��������
        destination.z = camZ;
        //��������� ������ � �������
        transform.position = destination;
        //�������� ������ ORTHOGRAPHICSIZE ������ , ����� ����� ���������� � ���� ������
        Camera.main.orthographicSize = destination.y + 10;
    }
}
