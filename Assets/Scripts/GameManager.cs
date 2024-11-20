using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;  // 생성할 적 프리팹
    public Transform[] waypoints;   // 적이 따라갈 경로 포인트들
    public float spawnInterval = 3f; // 적 생성 간격 (초 단위)
    public List<GameObject> environmentObjects; // 배치 가능한 환경 오브젝트들
    public GameObject weaponPrefab; // 배치할 무기 프리팹
    public TMP_Text pointsText;     // 포인트 UI 텍스트
    public TMP_Text timeText;       // 남은 시간 UI 텍스트
    public GameObject gameOverPanel; // 게임 오버 패널

    private int points = 300;       // 시작 포인트
    private float remainingTime = 15f; // 남은 시간 (초 단위)
    private bool isGameOver = false;  // 게임 종료 여부

    private void Start()
    {
        // 게임 오버 패널 비활성화
        gameOverPanel.SetActive(false);

        // 일정 시간마다 적을 생성하는 코루틴 시작
        StartCoroutine(SpawnEnemies());
        UpdatePointsUI();
        UpdateTimeUI();
    }

    private void Update()
    {
        if (!isGameOver)
        {
            DetectEnvironmentObjectClick();
            UpdateTimer();
        }
    }

    // 적을 주기적으로 생성하는 코루틴
    private System.Collections.IEnumerator SpawnEnemies()
    {
        while (!isGameOver)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval); // 3초마다 적 생성
        }
    }

    // 적 생성 함수
    private void SpawnEnemy()
    {
        // 적 프리팹 생성
        GameObject enemy = Instantiate(enemyPrefab, waypoints[0].position, Quaternion.identity);
        
        // 적에 EnemyMovement 스크립트를 추가하고 웨이포인트 바인딩
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.SetWaypoints(waypoints);
            enemyMovement.OnDestroyed += OnEnemyDestroyed; // 적 파괴 이벤트 연결
        }
    }

    // 환경 오브젝트 클릭을 감지하여 무기 배치
    private void DetectEnvironmentObjectClick()
    {
        if (Input.GetMouseButtonDown(0))  // 왼쪽 마우스 버튼 클릭 감지
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider != null && environmentObjects.Contains(hit.collider.gameObject))
            {
                if (points >= 200)
                {
                    // 클릭된 위치에 무기 오브젝트 배치
                    PlaceWeapon(hit.collider.gameObject.transform.position);
                    points -= 200; // 무기를 배치하면 200 포인트 감소
                    UpdatePointsUI();
                }
                else
                {
                    Debug.Log("포인트가 부족합니다.");
                }
            }
        }
    }

    // 무기를 지정된 위치에 배치하는 함수
    private void PlaceWeapon(Vector3 position)
    {
        Instantiate(weaponPrefab, position, Quaternion.identity);
    }

    // 포인트 UI 업데이트 함수
    private void UpdatePointsUI()
    {
        pointsText.text = "Points: " + points;
    }

    // 적이 파괴될 때 포인트 증가 함수
    private void OnEnemyDestroyed()
    {
        points += 150; // 적을 죽이면 150 포인트 증가
        UpdatePointsUI();
    }

    // 타이머 업데이트 함수
    private void UpdateTimer()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime < 0)
            {
                remainingTime = 0;
            }
            UpdateTimeUI();
        }
        else if (!isGameOver)
        {
            GameOver();
        }
    }

    // 남은 시간 UI 업데이트 함수
    private void UpdateTimeUI()
    {
        timeText.text = "Time: " + Mathf.CeilToInt(remainingTime) + "s";
    }

    // 게임 오버 처리 함수
    private void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);  // 게임 오버 패널 활성화
        Time.timeScale = 0f;  // 게임 멈춤
    }
}
