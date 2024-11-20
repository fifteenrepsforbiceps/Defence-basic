using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    public GameObject missilePrefab;     // 발사할 미사일 프리팹
    public float fireInterval = 1f;      // 미사일 발사 간격 (초 단위)
    private GameObject targetEnemy;      // 가장 가까운 적을 타겟으로 설정

    private void Start()
    {
        // 미사일을 1초 간격으로 발사하는 코루틴 시작
        StartCoroutine(FireMissile());
    }

    private void Update()
    {
        // 가장 가까운 적 찾기
        targetEnemy = FindClosestEnemy();
    }

    // 가장 가까운 적을 찾아 반환하는 메서드
    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");  // "Enemy" 태그를 가진 모든 오브젝트를 찾음
        GameObject closest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                closest = enemy;
                minDistance = distance;
            }
        }
        return closest;
    }

    // 미사일을 주기적으로 발사하는 코루틴
    private IEnumerator FireMissile()
    {
        while (true)
        {
            if (targetEnemy != null)
            {
                GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
                Missile missileScript = missile.GetComponent<Missile>();

                if (missileScript != null)
                {
                    missileScript.SetTarget(targetEnemy);
                }
            }
            yield return new WaitForSeconds(fireInterval);
        }
    }
}
