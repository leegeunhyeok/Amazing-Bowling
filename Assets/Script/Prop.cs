using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour {

	public int score = 5; // 파괴시 누적될 점수 
	public ParticleSystem explosionParticle; // 폭발 이펙트
	public float hp = 10f; // 체력

	// 해당 데미지만큼 hp 감소
	public void TakeDamage(float damage) {
		hp -= damage;
		if(hp <= 0) { // hp가 0 이하일 경우 
			// 폭발 이펙트 동적 생성
			ParticleSystem instance = Instantiate(explosionParticle, transform.position, transform.rotation);
			instance.Play(); // 이펙트 실행

			// 게임 매니저에게 점수 추가 전달
			GameManager.instance.AddScore(score);

			// 이펙트 종료 후 이펙트 인스턴스 제거
			Destroy(instance.gameObject, instance.duration);
			gameObject.SetActive(false); // 본 오브젝트는 삭제가 아닌 숨김 처리
		}
	}
}
