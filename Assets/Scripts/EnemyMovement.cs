using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Transform[] waypoints;  // 경로 포인트들
    private int currentWaypointIndex = 0;  // 현재 목표 웨이포인트 인덱스
    public float speed = 2f;  // 이동 속도
    public int health = 2;  // 적의 체력 (미사일 2번 맞으면 사라짐)

    public event System.Action OnDestroyed;

    public void SetWaypoints(Transform[] waypoints)
    {
        // 외부에서 웨이포인트 설정
        this.waypoints = waypoints;
    }

    private void Update()
    {
        // 경로가 설정되지 않았다면 리턴
        if (waypoints == null || waypoints.Length == 0) return;

        // 아직 모든 웨이포인트를 지나지 않았다면 계속 이동
        if (currentWaypointIndex < waypoints.Length)
        {
            // 현재 목표 웨이포인트 설정
            Transform targetWaypoint = waypoints[currentWaypointIndex];

            // 현재 위치에서 목표 웨이포인트로 이동
            transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

            // 목표 위치에 도달했는지 확인
            if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;
            }
        }
        // 모든 웨이포인트를 지나면 게임 오브젝트 삭제
        else
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }

    
}
