using UnityEngine;
using UnityEngine.UI;

public class Basket : MonoBehaviour
{
    [Header("Set Dynamically")]
    public Text scoreGT;
    private void Start()
    {
        //�������� ������ �� ������� ������ SCORECOUNTER
        GameObject scoreGO = GameObject.Find("ScoreCounter");
        //��������� ��������� TEXT ����� �������� �������
        scoreGT = scoreGO.GetComponent<Text>();
        //���������� ��������� ����� ����� ������ 0
        scoreGT.text = "0";
    }
    private void Update()
    {
        //�������� ������� ���������� ��������� ���� �� ������ �� INPUT
        /*���������� mousePos2D ������������� �������� Input.mousePosition. ���
        �������� ����������, �� ���� ���������� � �������� �� ������ ��������
        ���� ������ �� ��������� ����. ���������� Z � Input.mousePositon ������
        ����� 0, ������ ��� �����, �� ����, ��� ��������� ���������.
        */
        Vector3 mousePos2D = Input.mousePosition;

        //���������� Z ������ ����������, ��� ������ � ���������� �����������
        //��������� ��������� ����
        /*��� ������ ����������� ���������� Z � mousePos2D �������� ���������� Z
        ������� ������ � �������� ������. � ���� ���������� Z ������� ������
        ����� -10, �������������� mousePos2D. z ������� �������� 10. ��� ����� ��
        �������� ������������ ������ ������� ScreenToWorldPoint(), ��� ������
        �� ������ ������ ���������� ����� mousePos3D � ���������� ������������,
        ���������� ������� �� �� ��������� Z=0.
        */
        mousePos2D.z = -Camera.main.transform.position.z;

        //������������� ����� �� ��������� ��������� ������ � 
        //���������� ���������� ����
        /*ScreenToWorldPoint () ����������� �������� ���������� mousePoint2D � ����������
        � ���������� ������� ������������. ���� �������� mousePos2D. z
        �������� ������ 0, ����� mousePos3D ������� ���������� Z, ������ -10
        (���������� Z ������� ������). �������� mousePos2D.z �������� 10, ��
        ��������� ����� mousePos3D � ���������� ������������ �� �������� 10
        ������ �� ������� ������, ��������� ���� ���� mousePos3D. z �������� ��������
        0. � ���� Apple Picker ��� �� ����� �������� ��������, �� � �������
        ����� ���������� Z ��������� ���� ����� ������ ����� ������ ����.
        */
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //����������� ������� ����� ��� � � ���������� � ��������� ����
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;
    }
    private void OnCollisionEnter(Collision coll)
    {
        //�������� ������, �������� � ��� �������
        /*��� ������ ����������� ��������� ���������� collidedWith ������ ��
        ������� ������, ������������� � ��������.
        */
        GameObject colldeWith = coll.gameObject;
        if (colldeWith.tag == "Apple")
        {
            Destroy(colldeWith);
        }
        //������������� ����� � SCOREGT � ����� �����
        int score = int.Parse(scoreGT.text);
        //�������� ���� �� ��������� ������
        score += 100;
        //������������� ����� ����� ������� � ������ � ������� �� �� �����
        scoreGT.text = score.ToString();
        //��������� ������ ����������
        if (score > HighScore.score)
        {
            HighScore.score = score;
        }
    }
}
