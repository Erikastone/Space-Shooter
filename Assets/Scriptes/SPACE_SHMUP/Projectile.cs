using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;
    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;
    //�������� ��������� ���� _type � ������������
    //�������� ��������� ��� ������ ��������
    public WeaponType type
    {
        get { return (_type); }
        set
        {
            SetType(value);/*����� ������ set �������� �����
SetType() � ��������� ������� ������� ������, ��� ������ ��������� ��������
���� _type.
d. ����� SetType() ���������� ��������� Renderer, ������������ � �����
�������� �������, ������� ����� �� ���������� ������ �� ����.
e. SetTy�� () �� ������ ����������� �������� �������� ���� .type, �� � �������������
���� ������� � ������������ � ������, �������� � weaponDefinitions
� ������ Main.*/
        }
    }
    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (bndCheck.offUp)
        {
            Destroy(gameObject);/*���� ������ ����� �� ������� ������� ������, ���������� ���.*/
        }
    }
    /// <summary>
    /// �������� ������� ���� _type � ������������� ���� ����� �������,
    /// ��� ���������� � WeapoDefinition.
    /// </summary>
    /// <param name="eType">
    /// ��� WeaponType ������������� ������
    /// </param>
    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
