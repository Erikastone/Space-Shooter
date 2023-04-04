using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    private Renderer rend;
    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;
    //свойство маскирует поле _type и обрабатывает
    //операции присвания ему нового значения
    public WeaponType type
    {
        get { return (_type); }
        set
        {
            SetType(value);/*метод записи set вызывает метод
SetType() и позволяет сделать намного больше, чем просто присвоить значение
полю _type.
d. Метод SetType() использует компонент Renderer, подключенный к этому
игровому объекту, поэтому здесь мы запоминаем ссылку на него.
e. SetTyре () не только присваивает значение скрытому полю .type, но и устанавливает
цвет снаряда в соответствии с цветом, заданным в weaponDefinitions
в классе Main.*/
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
            Destroy(gameObject);/*Если снаряд вышел за верхнюю границу экрана, уничтожить его.*/
        }
    }
    /// <summary>
    /// Изменяет скрытое поле _type и устанавливает цвет этого снаряда,
    /// как определено в WeapoDefinition.
    /// </summary>
    /// <param name="eType">
    /// тип WeaponType используемого оружия
    /// </param>
    public void SetType(WeaponType eType)
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    }
}
