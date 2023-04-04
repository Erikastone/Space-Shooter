using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main S;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;
    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;//������ ��� ����������������
    public WeaponDefinition[] weaponDefinitions;
    public GameObject prefabPowerUp;
    public WeaponType[] powerUpFrequency = new WeaponType[]
    {
        WeaponType.blaster, WeaponType.blaster,
        WeaponType.spread, WeaponType.shield
    };
    private BoundsCheck bndCheck;
    public void ShipDestroyed(Enemy e)/*������ powerUpFrequency ����� ������� WeaponType ���������� ������� ��������
������� ������� ����. �� ��������� �� �������� ��� ��������, ����
������� ����� � ���� ���� ������� ��� ��������� ����, ������� ������
� ���������� ����� ���������� � ��� ���� ����, ��� ������.
c. ����� ShipDestroyed() ����� ���������� ����������� ���������� �������
� ������ ��� ����������. ������ �� ����� ��������� ����� �� ����� ������������
�������.*/
    {
        //������������� ����� � �������� ������������
        if (Random.value <= e.powerUpDropChance)/*��������� ������� ���� ����� ����� ����� ���� powerUpDropChance, ��������
����� ����� 0 � 1. Random.value � ��� ��������, ������������ ���������
����� ���� float ����� 0 (������������) � 1 (������������). (��������
Random, value ����� ������� ����� �� ��������� ��������, � 0, � 1.) ����
���������� ��������� ����� ������ ��� ����� powerUpDropChance, ���������
����� ��������� PowerUp. ���� powerUpDropChance ����� ��������� � ������
Enemy, ������� ��������� ������� ������ ����� ����� ����� ����� �������
��� ������ ����������� �������� ����� ���� ����� (��������, ������� ����
Enemy_0 ����� �� ��������� ������ ����� �����, � ������� Enemy_4 � ������).
������ ��� ������ ����������� ������� ��������� ������ � ���������
MonoDevelop, ������ ��� ���� powerUpDropChance ��� �� ��������� � �����
Enemy.*/
        {
            //������� ��� ������
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            GameObject go = Instantiate(prefabPowerUp) as GameObject;
            PowerUp pu = go.GetComponent<PowerUp>();
            pu.SetType(puType);/*����� ������ ���� ������ ���������� ����� SetTy�� () ���������� ����������
PowerUp, � ���, � ���� �������, ������������� ����, ���� _type � ����������
��������������� ����� � TextMesh.*/
            //��������� � ����� ��� �������� ����������� �������
            pu.transform.position = e.transform.position;
        }
    }
    private void Awake()
    {
        S = this;
        bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
        //����� � ������� ���� WeaponType
        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions)
        {
            WEAP_DICT[def.type] = def;/*���� ���� ��������� �������� �� ��������� ������� weaponDefinitions
� ������� ��� ��� ��������������� ������ � ������� WEAP_DICT.*/
        }
    }
    public void SpawnEnemy()
    {
        //������� ��������� ������
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);
        //���������� �������� ������� ��� ������� � ��������� ������� �
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)/*�� ���� ��������� ������ ����� ��������� BoundsCheck, � ���� �������
������������ ������ ����� ����������. ������ ������ ����� ���� �������������,
����� ������� ������ ��������� ������� � ������, ������ ���
��� ���� isOnScreen ������� �������� false, ��� � ������ � Enemy_0, �������
������� ���������� �������� �������.*/
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        //���������� ��������� ���������� ���������� ���������� �������
        Vector3 pos = Vector3.zero;/*���� �������� ���� ���������� ��������� �������������� ������ ����������
�������. �� ���������� ��������� BoundsCheck ������� _MainCamera, �����
�������� camWidth � camHeight, �������� �������� ���������� X � ��������,
����� ��������� ������� �������� � �������� ����� � ������ ������ ������,
� ����� ���������� ���������� Y ���, ����� ��������� ������� ��������
����� ��� ������� �������� ������.*/
        float xMin = -bndCheck.camWidth + enemyPadding;
        float xMax = bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }
    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay);
    }
    public void Restart()
    {
        SceneManager.LoadScene("SPACE SHMUP");
    }
    ///<summary>
    ///����������� ������� ������������ WeapoDefinition �� ������������ ����������� ����
    ///WEP_DICT ������ Main.
    ///</summary>
    ///<returns>
    ///��������� WeaponDefinition ���, ���� ��� ������ �����������
    ///��� ���������� WeaponType ���������� ����� ��������� WeaponDefinition
    ///� ����� none.
    ///</returns>
    ///<param name="wt">
    ///��� ������ WeaponType ��� �������  ��������� ��������
    ///WeaponDefinition
    ///</param>
    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        //��������� ������� ���������� ����� � �������
        //������� ������� �������� �� �������������� ����� ������� ������
        //������� �� ���������� ������ ������ ����
        if (WEAP_DICT.ContainsKey(wt))
        {
            return (WEAP_DICT[wt]);
        }
        //��. ���������� ���������� ����� ��������� WeaponDefinition
        //� ����� ������ WeaponType.none ��� �������� ��������� �������
        //����� ��������� ����������� WeaponDefinition
        return (new WeaponDefinition());
    }
}
