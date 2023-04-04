using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set in Inspector")]
    //Vector2.x ������ ����������� �������� � � ������������ ��� ������ ������
    public Vector2 rotMinMaax = new Vector2(15, 90);
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 6f;
    public float fadeTime = 4f;
    [Header("Set Dynamically")]
    public WeaponType type; // ��� ������
    public GameObject cube;
    public TextMesh letter;
    public Vector3 rotPerSecond;//�������� ��������
    public float birthTime;

    private Rigidbody rigid;
    private BoundsCheck bndCheck;
    private Renderer cubeRend;
    private void Awake()
    {
        cube = transform.Find("Cube").gameObject;
        letter = GetComponent<TextMesh>();
        rigid = GetComponent<Rigidbody>();
        bndCheck = GetComponent<BoundsCheck>();
        cubeRend = cube.GetComponent<Renderer>();
        //������� ��������� ��������
        Vector3 vel = Random.onUnitSphere;
        vel.z = 0;
        vel.Normalize();
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        rigid.velocity = vel;
        //���������� ���� ��������� ������ R:[0,0,0]
        transform.rotation = Quaternion.identity;
        //������� ��������� �������� �������� ��� ����
        rotPerSecond = new Vector3(Random.Range(rotMinMaax.x, rotMinMaax.y),
            Random.Range(rotMinMaax.x, rotMinMaax.y),
            Random.Range(rotMinMaax.x, rotMinMaax.y));
        birthTime = Time.time;
    }
    private void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);
        //������ ����������� ���� � �������� �������
        //�� ��������� ����� ���������� 10 ������
        //����������� 4
        float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
        //� ������� lifeTime ������ �������� u ����� <=0 ����� ��� ������ 
        //������������� � ����� fadeTime ������ ����� 1
        //���� u>=1 ���������� �����
        if (u >= 1)
        {
            Destroy(this.gameObject);
            return;
        }
        //������������ u ��� ������������ ����� �������� ���� � �����
        if (u > 0)
        {
            Color c = cubeRend.material.color;
            c.a = 1f - u;
            cubeRend.material.color = c;
            //����� ���� ������������� �� ���������
            c = letter.color;
            c.a = 1f - (u * 0.5f);
            letter.color = c;
        }
        if (!bndCheck.isOnScreen)
        {
            Destroy(gameObject);
        }
    }
    public void SetType(WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition(wt);
        cubeRend.material.color = def.color;//���������� ���� ����
        letter.text = def.letter;//���������� ������������ �����
        type = wt;
    }
    public void AbsorderBy(GameObject target)
    {
        Destroy(this.gameObject);
    }
}
