using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject cloudSphere;
    public int numSpheresMin = 6;/*numSpheresMin / numSpheresMax � ����������� � ������������ (� ����������������
�� 1 ������ ������������) ���������� ����������� �����������
CloudSphere.
    */
    public int numSpheresMax = 10;
    public Vector3 sphereOFfsetScale = new Vector3(5, 2, 1);/*sphereOff setScale � ������������ ���������� (������������� ��� �������������)
CloudSphere �� ������ Cloud ����� ������ ���
    */
    public Vector2 sphereScaleRangeX = new Vector2(4, 8);/*sphereScaleRangeX / Y / Z � �������� �������� ��� ������ ���. ��
��������� ��������� ���������� CloudSphere, ������ ������� ������
������.*/
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);
    public float scaleYMin = 2f;/*scaleYMin � � ����� ������� Start () ������� �� ��� Y ������� ����������
CloudSphere ����������� � ����������� �� ����������� �� ������
����� ��� X. ��������� ����� ������� ������� ����� ����������� � �����.
scaleYMin � ��� ���������� ��������� ������� �� ��� Y (�����
�������� ��������� ������� ������ �������).*/

    private List<GameObject> spheres;
    private void Start()
    {
        spheres = new List<GameObject>();

        int num = Random.Range(numSpheresMin, numSpheresMax);
        for (int i = 0; i < num; i++)
        {
            GameObject sp = Instantiate<GameObject>(cloudSphere);
            spheres.Add(sp);
            Transform spTrans = sp.transform;
            spTrans.SetParent(this.transform);

            //������� ��������� ��������������
            /*���������� ��������� ����� ������ ��������� ����� (�� ���� ����� ���-��
            ������ ����� � ��������, ������ �������, � � ������� � ������ ���������:
            [ 0, 0, 0 ] ). ������ ���������� (X, Y, Z) ���� ����� ����� ���������� �� ���������������
            �������� sphereOff setScale.*/
            Vector3 offset = Random.insideUnitSphere;
            offset.x *= sphereOFfsetScale.x;
            offset.y *= sphereOFfsetScale.y;
            offset.z *= sphereOFfsetScale.z;
            spTrans.localPosition = offset;/*�������� CloudSphere.transform.localposition ������������� ��������
offset, transform, position ������ ���������� � ������� �����������, �����
��� transform. localPosition � � ����������� ������������ ������ ��������
(� ������ ������ ������� Cloud).*/

            //������� ��������� �������
            /*�������� ���������� ������� ��� ������ ��� � �����������. ������
            sphereScaleRange ������ � ���� � ����������� ��������, � � ���� � � ������������.*/
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

            //�������������� ������� � �� ���������� � �� ������
            /*����� ������ �������� �� ���� ���������� ������� �� ��� Y, � �����������
            �� �������� CloudSphere �� ������ Cloud ����� ��� X. ��� ������ ����� ��
            ������ ������, ��� ������ ������� �� ��� Y.*/
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOFfsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);

            /*�������� scale ������������� �������� localscale ���������� CloudSphere.
              ������� ������ ������������ ������������ ������������� ����������
              Transform, ������� ��� ���� transform. scale. ������ localscale � lossyScale.
              �������� lossyScale, ��������� ������ ��� ������, �������� ����������
              ������� � ������� �����������, ��� ���� �� ������ ��������, ��� ���
              ����� ���� ������.*/
            spTrans.localScale = scale;
        }
    }
    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
        */
    }
    private void Restart()
    {
        //������� ������ �����, ������������ ������
        /*����� Restart() ���������� ��� �������� ����� CloudSphere � ��������
        Start(), ����� ������������� ����� ������*/
        foreach (GameObject sp in spheres)
        {
            Destroy(sp);
        }
        Start();
    }
}
