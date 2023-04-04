using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float speed = 10f;
    public float fireRate = 0.3f;//������ ����� ����������
    public float health = 10;
    public int score = 100;//���� �� ����������� �������
    public float showDamageDurantion = 0.1f;//������������ ������� ���������
    public float powerUpDropChance = 1f;//����������� ������ �������
    [Header("Set Dynamically Enemy")]
    public Color[] originalColors;
    public Material[] materials;
    public bool showingDanage = false;
    public float damageDoneTime;//����� ����������� ����������� �������
    public bool notifiedDestruction = false;
    protected BoundsCheck bndCheck;
    //��� ��������, ����� ��������� ��� ����
    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }
    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        /*���������� ����� ����� Utils.GetAllMaterials() � ����������� ������
materials. ����� ����������� ����� ���� ���������� � ����������� �� ��������
�����. �������� �� �� ��� � ��������� ������ ��� ������� �������
Enemy �������� � ����� ����, ���� ����� ��������� ���������� �� � �����
����� �� ������ ������, ������������ �� ������� ������ ��� ���������
� ����� ���������� �������� ����.
�������� ��������, ��� ����� Utils.GetAllMaterials() ������������
� ������ Awake(), � ��������� ���������� � materials. �� ���� ��� �������
���������� Enemy ��� �������� ����� ��������� ������ ���� ���.
Utils. GetAllMaterials () ���������� �������� ��������� �������
GetComponentsInChildren<>(),*/
        materials = Utils.GetAllMaterials(gameObject);
        originalColors = new Color[materials.Length];
        for (int i = 0; i < materials.Length; i++)
        {
            originalColors[i] = materials[i].color;
        }
    }
    private void Update()
    {
        Move();
        if (showingDanage && Time.time > damageDoneTime)
        {
            UnShowDamage();
        }
        if (bndCheck != null && bndCheck.offDown)/*������� ����������� ������� �������������� ������ � bndCheck. ����
���������� �������� Enemy � �������� �������, �� �������� ����������
�������� BoundsCheck, ��� ������ ������� �� ����������. �������� ������
�������� ������� �� ������� ������ (��� ���������� � �������� BoundsCheck)
����������� ������ ��� ������� bndCheck ! = null.*/
        {
            //����������  ����� �� �� ������ ��������
            Destroy(gameObject);
        }
    }
    public virtual void Move()
    {/*����� Move() �������� ������� �������������� ������� ������� Enemy_0,
���������� ��� ����, ����� ��� Y, � ����������� ����� ���������� ��������
pos (������������� �������������� �������� �������).*/
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }
    private void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                //���� ��������� ������� �� ��������� ������ �� �� �������  ��� �����������
                if (!bndCheck.isOnScreen)
                {
                    Destroy(otherGO);
                    break;
                };
                //�������� ��������� �������
                ShowDamage();
                //������� ����������� ���� �� WEAP_DICT � ������ Main
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0)
                {//�������� ������� �� �����������
                    if (!notifiedDestruction)
                    {
                        Main.S.ShipDestroyed(this);
                    }
                    notifiedDestruction = true;
                    Destroy(this.gameObject);
                }
                Destroy(otherGO);
                break;
            default:
                print("Enemy hit by non-PrijectileHero: " + otherGO.name);
                break;
        }
    }
    void ShowDamage()
    {
        foreach (Material m in materials)
        {
            m.color = Color.red;
        }
        showingDanage = true;
        damageDoneTime = Time.time + showDamageDurantion;
    }
    void UnShowDamage()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i].color = originalColors[i];
        }
        showingDanage = false;
    }
}
