using UnityEngine;
/// <summary>
/// ������������� ����� �������� ������� �� ������� ������
/// �����! �������� ������ � ��������������� ������� � 0 0 0
/// </summary>

public class BoundsCheck : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keepOnScreen = true;/*keepOnScreen �������� ���������� ����� ������ �������� BoundsCheck � �����
�� �� ��������� �������� ������� ����� �� ������� ������ (true) � �����
���������, �� ���������� ���, ��� ������ ����� �� ������� ������ (false).*/
    [Header("Set Dynamically")]
    public bool isOnScreen = true;/*isOnScreen �������� �������� false, ���� ������� ������ ����� �� �������
������. ������, ����� �� ������� ������� ������ � �������� �� ��� ������,
��� �� �������� ������� radius. ������ ������� radius ��� Enemy_0 �������
�������� -2.5, ����� ������ ��������� ������� � ������, ������ ���
������� isOnScreen ������� �������� false.*/
    public float camWidth;
    public float camHeight;
    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown;/*����� ����������� ������ ����������, �� ����� ��� ������ �������
������, ������� ������� ������� ������, ������� �����. �� ���������
���� �� ������������� �������� false. ������ [Hideininspector], ��������������
����� ����������, ������������ ��������� ���� ������� �����
� ���������� ���� ��� ���, ��� ��� �������� �������������� � �����
�������� (� ����������) ������� ��������. ������� [Hideininspector]
����������� �� ���� ������� ���������� off__(�� ���� offRight, offLeft
� �. �.), ������ ��� ��� ��� ����������� � ����� ������ ��� ���. ���� ��
���������� off__����������� � ������� ��������� �������, ��� ��������
�� �������� ������� [Hideininspector] ����� ������ �� ���, �����
�������� ���� �� �������.*/
    private void Awake()
    {/*Camera.main ��������� ������ � ������ ������ � ����� MainCamera � �����.
�����, ���� ������ ���������������,. orthographicSize ������ ����� �� ����
Size � ���������� (� ������ ������ 40). �� ���� ���������� camHeight �����
��������� ���������� �� ������ ������� ��������� (������� [ 0, 0, 0 ]) ��
�������� ��� ������� ���� ������.*/
        camHeight = Camera.main.orthographicSize;
        /*Camera. main. aspect � ��� ��������� ������ � ������ ���� ������ ������,
��� ���������� ��������� ������ ������ Game (����) � � ��������� ������
Portrait (3:4). ������� camHeight �� .aspect, ����� �������� ���������� ��
������ �� ����� ��� ������ ������� ������.*/
        camWidth = camHeight * Camera.main.aspect;
    }
    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offUp = offDown = false;

        if (pos.x > camWidth - radius)
        {
            pos.x = camWidth - radius;
            isOnScreen = false;/*���� ������� � ����� �� ���� ������� ���������� if �����������, ��
������� ������ �������� �� ��������� �������, ��� �� ������ ����������.
���������� isOnScreen ������������� �������� false, � �������� pos ��������������
���, ����� ��� ������������� ����� ����� ���� ������� �������
������ ������� ��� �����.*/
            offRight = true;
        }
        if (pos.x < -camWidth + radius)
        {
            pos.x = -camWidth + radius;
            isOnScreen = false;
            offLeft = true;
        }
        if (pos.y > camHeight - radius)
        {
            pos.y = camHeight - radius;
            isOnScreen = false;
            offUp = true;
        }
        if (pos.y < -camHeight + radius)
        {
            pos.y = -camHeight + radius;
            isOnScreen = false;
            offDown = true;
        }
        isOnScreen = !(offRight || offLeft || offUp || offDown);
        if (keepOnScreen && !isOnScreen)/*���� keepOnScreen ����� �������� true, �� �������� ������ ��������� �������
������ ���������� � �������� ������. ���� keepOnScreen ����� ��������
true � isOnScreen ����� �������� false, �� ������� ������ ������� �������
������ � ��� ����� ������� �����. � ���� ������ � transform.position ������������
����������� �������� pos, ��������������� ������� �� ������,
� ���������� isOnScreen ������������� true, ������ ��� ������� ������
������ ��� �������� �� �����.
���� keepOnScreen ����� �������� false, ����� �������� pos �� ������������
� transform.position � �������� ������� ����������� �������� �����,
� ���������� isOnScreen ���������� ������� false. ����� �������� ��������,
����� ������� ������ ��������� �� ������ ��� �����, � � ���� ������
isOnScreen �������� �������� true,*/
        {
            transform.position = pos;
            isOnScreen = true;
            offRight = offLeft = offUp = offDown = false;
        }
    }
    //������ �������
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
