using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGenerator : MonoBehaviour {
	public GameObject[] propPrefabs; // 목표로 생성할 프리팹 리스트
	private BoxCollider area; // 스폰지역 정보를 담고있는 콜라이더
	public int count = 100; // 스폰 갯수
	private List<GameObject> props = new List<GameObject>(); // 스폰된 객체 저장 리스트 


	// Use this for initialization
	void Start () {
		area = GetComponent<BoxCollider>(); // 스폰할 영역 가져오기
		for(int i=0; i<count; i++) {
			Spawn(); // 오브젝트 생성
		}
		area.enabled = false; 
	}

	// 오브젝트 스폰
	private void Spawn() {
		int selection = Random.Range(0, propPrefabs.Length); // 프리팹 리스트 중 무작위로 프리팹 하나 선택
		GameObject selectedPrefab = propPrefabs[selection]; 
		Vector3 spawnPos = GetRandomPosition(); // 랜덤 위치
		GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity); // 랜덤 위치에 새 오브젝트 생성
		props.Add(instance); // 생성한 오브젝트 리스트에 저장
	}

	// 랜덤 벡터 생성
	private Vector3 GetRandomPosition() {
		Vector3 basePosition = transform.position;
		Vector3 size = area.size;

		float posX = basePosition.x + Random.Range(-size.x/2f, size.x/2f);
		float posY = basePosition.y + Random.Range(-size.y/2f, size.y/2f);
		float posZ = basePosition.z + Random.Range(-size.z/2f, size.z/2f);
		return new Vector3(posX, posY, posZ);
	}

	// 리셋
	public void Reset() {
		// 모든 오브젝트 위치 랜덤으로 재설정
		for(int i=0; i<props.Count; i++) {
			props[i].transform.position = GetRandomPosition();
			props[i].SetActive(true);
		}
	}
}
