using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationsPerSecond = 0.1f;
    [Header("Set Dynamically")]
    public int levelShown = 0;

    private Material mat;
    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    private void Update()
    {
        //��������� ������� �������� ��������� ���� �� ������� �������� hero
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);/*�������� Hero. S. shield Level ����������� ���� �� ���������� ������, � ���������
������������� currLevel. ���������� shieldLevel �����������, ���
������� � ������ �������� � �������� �� ��� X ���������� ����� � �������
����� ����������, � �� ����� ����� ���������.*/
        //���� ��� ���������� �� levelShown
        if (levelShown != currLevel)
        {
            levelShown = currLevel;
            //�������������� �������� � ��������, ����� ���������� ���� � ������ ���������
            mat.mainTextureOffset = new Vector2(0.2f * levelShown, 0);
        }
        //������������ ���� � ������ ����� � ���������� ���������
        float rZ = -(rotationsPerSecond * Time.time * 360) % 360f;
        transform.rotation = Quaternion.Euler(0, 0, rZ);
    }
}
