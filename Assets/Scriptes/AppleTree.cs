using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Set in Inspector")]
    //������ ��� �������� ����
    public GameObject applePrefab;

    //�������� �������� ������
    public float speed = 1f;

    //����������, �� ������� ������ ���������� ����������� �������� ������
    public float leftAndRightEdge = 10f;

    //����������� ���������� ��������� ����������� ��������
    public float chanceToChangeDirections = 0.1f;

    //������� �������� ����������� �����
    public float secondBetweenAppleDrops = 1f;
    private void Start()
    {
        /*���������� ������ ��� � �������
        ������� Invoke() �������� �������, �������� ������, ����� ���������
        ����� ������.
        */
        Invoke("DropApple", 2f);
    }
    private void DropApple()
    {/*DropApple() � ��� ���� �������. ��� ������� ��������� Apple � �����, ���
        ��������� AppleTree.
        */
        GameObject apple = Instantiate<GameObject>(applePrefab);
        apple.transform.position = transform.position;//�������������� ����� ������ �������� ������� apple ��������������� ������
                                                      //�������������� ������ AppleTree.
        Invoke("DropApple", secondBetweenAppleDrops);
    }
    private void Update()
    {
        /*������� �����������. 
        ��� ������ ���������� ��������� ���������� Vector3 pos ��� ��������
        ������� ������� ������.
        */
        Vector3 pos = transform.position;
        /*��������� � ���������� pos ������������� �� ������������ �������� speed
         � ��������� ������� Time.deltaTime(���������� ������, ��������� �����
        ����������� ����������� �����).��������� ����� ������ ����� ������������
        c ������ ��������� �������, ��� ����� ����� ��� ����������������
        ���(��.������ ��������� ��� � ��������� �������).
        */
        pos.x += speed * Time.deltaTime;
        /*���������� �������� pos ������������� ������� �������� transform,
        position(��� �������� ����������� ������ AppleTree � ����� ��������������).
        ���� �� ��������� ������������ pos �������� transform.position,
        ������ �� ������������.
        */
        transform.position = pos;
        //��������� ����������
        if (pos.x < -leftAndRightEdge)
        {
            /*���� �������� pos. � ��������� ������� ���������, ���������� speed �������������
             ��������� ������ Mathf .Abs(speed), ������� ���������� ����������
             ������������� �������� speed � ��� ����� �����������, ��� � ��������� �����
             �������� ����������� ������ ������.
            */
            speed = Mathf.Abs(speed);//������ �������� � �����
        }
        else if (pos.x > leftAndRightEdge)
        {
            speed = -Mathf.Abs(speed);//�����
        }

    }
    private void FixedUpdate()
    {
        /*Random.value ���������� ��������� ����� ���� float ����� 0 � 1 (�������
      0 � 1 ��� ��������� ��������). ���� ��������� �������� ������
       chanceToChangeDirections...
      */
        if (Random.value < chanceToChangeDirections)
        {//�������� ����������� �������� AppleTree �����, ������� ���� ����������
            speed *= -1;
        }
    }
}
