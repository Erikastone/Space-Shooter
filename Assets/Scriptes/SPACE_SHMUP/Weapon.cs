using UnityEngine;
/// <summary>
/// ��� ������������ ���� ��������� ����� ������
/// ����� ������� ��� "shield", ��� �� ���� ���������� ���������������� ������
/// ������������� [HP] ���� �������� ������� ������� �� �����������
/// </summary>
public enum WeaponType
{
    none,//�� ��������� ��� ������
    blaster,//�������
    spread,//������� �����, ���������� ����������� ���������
    phaser,//[HP]�������� �����
    missile,//[HP]��������������� ������
    laser,//[HP]������� ����������� ��� �������������� �����������
    shield//���������� shielLevel
}
/// <summary>
/// ����� WeaponDefinition �������� ����������� ��������
/// ����������� ���� ������ � ����������. ��� ������ Main
/// ����� ������� ������ ��������� ���� WeaponDefinition
/// </summary>
[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.none;
    public string letter;//����� �� ������ ������������ �����
    public Color color = Color.white;//���� ������ ������� ������ ������
    public GameObject projectilePrefab;//������ �������
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;//�������������� ��������
    public float continuosDamage = 0;//������� ���������� � ������� (��� laser)
    public float delayBetweenShots = 0;
    public float velocity = 20;//�������� ������ �������
}
public class Weapon : MonoBehaviour
{
    public static Transform PROJECTILE_ANCHOR;
    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.none;
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;
    private Renderer collarRend;
    private void Start()
    {
        collar = transform.Find("Collar").gameObject;
        collarRend = collar.GetComponent<Renderer>();
        //������� SetType() ����� �������� ��� ������ �� ���������
        SetType(_type);/*� ������ �������� ������� ������ Weapon ������� ����� SetType() � ��������
��� �������� �� ���� WeaponType _type. ���� ����� ������ ������
Weapon (���� _type ����� �������� WeaponType. ����) ��� ������� ��� �����
� ��������������� ���� (���� _type ����� �������� WeaponType. blaster ���
WeaponType. spread).*/
        //����������� ������� ����� �������� �� ���� ��������
        if (PROJECTILE_ANCHOR == null)/*PRO3ECTILE_ANCHOR � ��� ����������� ��������� Transform, ������� ������
������ ���� �������� � �������� ��� ���� ��������, ����������� ����������
Weapon. ���� PROJECTILE_ANCHOR ����� �������� null (�� ���� ����� ����
��������� ��� �� ������), �������� ������� ����� ������� ������ � ������
_ProjectileAnchor � ����������� ������ �� ��� ��������� Transform ����
PROJECTILE_ANCHOR.*/
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }
        //����� fireDelegate � �������� ������� �������
        GameObject rootGO = transform.root.gameObject;/*������ ������ ������������ � ������� �������� ������� (������, ��� _����).
��� ������ ������� �������� ������� ������, � �������� ���������� ���
������.*/
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
    }
    public WeaponType type
    {
        get { return (_type); }
        set { SetType(value); }
    }
    public void SetType(WeaponType wt)
    {
        _type = wt;
        if (type == WeaponType.none)/*���� type ����� �������� WeaponType. none, ���� ������� ������ ��������������.
����� ������� ������ ���������, �� ���������� �� ���� �� ��� �������,
�������������� �� ������ MonoBehaviour (��������, Update (), LateUpdate (),
FixedUpdate(), OnCollisionEnter () � �. �.), �� �� ��������� � �������������
������ � �� ������������ � �����. ������ ����������� ����������� ��������
������� � �������� �������� ���������� �� ���������, ������������
� ����������� �������� �������, �� ���� ���� ������-�� �� �������
����� ������� SetType() ��� ��������� �������� type �������� WeaponType.
blaster ��� WeaponType. spread, ����� SetType() ���������� ������� ������,
� �������� ���������.*/
        {
            this.gameObject.SetActive(false);
            return;
        }
        else
        {
            this.gameObject.SetActive(true);
        }
        def = Main.GetWeaponDefinition(_type);
        collarRend.material.color = def.color;
        lastShotTime = 0;//����� ����� ��������� _type ����� ����������
    }
    public void Fire()
    {
        if (!gameObject.activeInHierarchy)/*gameObject.activelnHierarchy ����� �������� false, ���� ��������� ������
��������� Weapon ��� ��������� ��� ��������� ������� ������ _����
(�������� �������� ��� ������� ������). � ����� ������, ���� gameObject.
activelnHierarchy ����� �������� false, ��� ������� ��� �� ����������
���������� � ������� �� ������������.*/
        {
            return;
        }
        //���� ����� ���������� ������ ������������ ����� �������, �����
        if (Time.time - lastShotTime < def.delayBetweenShots)
        {
            return;
        }
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;/*�������� ��������� �������� ������ � ����������� �����, �� ���� transform.
up.� < 0 (��� �������� ��� ��������� ��������, ������ ������� ��������
����), ��������� � �������� vel ����� ���������� ����.*/
        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }
        switch (type)
        {
            case WeaponType.blaster:
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;
            case WeaponType.spread:/*���� _type ����� �������� WeaponType. spread, ��������� ��� ������ �������.
��� ���� �� ��� ������������ ������� ����������� �� 10 �������� ������������
��� Vector3. back (�� ���� ��� Z, ������� ������������ �� ���������
������ � ���). ����� �� �������� Rigidbody.velocity ���������������
������� ������������ ���� �������� �� vel*/
                p = MakeProjectile();//������ ������� �����
                p.rigid.velocity = vel;
                p = MakeProjectile();//� �����
                p.transform.rotation = Quaternion.AngleAxis(10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile();//� ����
                p.transform.rotation = Quaternion.AngleAxis(-10, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
        }
    }
    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }
        else
        {
            go.tag = "ProjectileEnemy";
            go.layer = LayerMask.NameToLayer("ProjectileEnemy");
        }
        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);/*��������� �������� ������� Projectile ����������� PROJECTILE ANCHOR. � ����������
����� � ������ Hierarchy (��������) ������ ����������� ������
_ProjectileAnchor, ��� ������������ ������������� ������� � ��� � ����������
������������� �� �������� ���������� ������ Projectile. �������� true
��������� �������� ������� go, ��� �������� ������ ������ ��������� ����
������� ���������� ���������� �� ��������.
�. ���� lastShotTime ������������� ������� �����, ��� ������������� �����������
���������� �������� ������, ��� ����� def .delayBetweenShots
������.*/
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return (p);
    }
}
