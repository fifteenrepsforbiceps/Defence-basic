using UnityEngine;

public class Missile : MonoBehaviour
{
    public float speed = 5f;  // 미사일 이동 속도
    private GameObject target;  // 목표 적 오브젝트

    // 목표 적을 설정하는 메서드
    public void SetTarget(GameObject targetEnemy)
    {
        target = targetEnemy;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);  // 타겟이 없어지면 미사일도 사라짐
            return;
        }

        // 타겟의 위치로 미사일 이동
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        // 타겟에 도달하면 적에게 피해를 주고 미사일 삭제
        if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
        {
            EnemyMovement enemyMovement = target.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.TakeDamage(1);  // 적에게 피해를 입힘
            }
            Destroy(gameObject);  // 미사일 삭제
        }
    }
}
