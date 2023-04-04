using UnityEngine;

public class Apple : MonoBehaviour
{/*�������
�, ����������� ���������� ��������� ������������ ����� ������������
������, ������� ��� ���������� Apple ����� ����� ���� � �� �� ��������
bottomY. ���� bottomY ��������� � ����� ����������, ��� ������������ ���������
�� ���� �����������. ����� ����� ��������, ��� ����������� ����,
����� ��� bottomY, �� ������������ � ����������.
    */
    [Header("In inspector")]
    public static float bottomY = -20f;
    private void Update()
    {
        if (transform.position.y < bottomY)
        {
            Destroy(this.gameObject);
            //�������� ����� �� ��������� APPLEPICKER ������ ������
            ApplePicker apScript = Camera.main.GetComponent<ApplePicker>();
            //������� ���������� ����� APPLEDESTROYES �� APSRIPT
            apScript.AppleDestroyed();
        }
    }
}
