using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
	public LayerMask whatIsProp;
	public ParticleSystem explosionParticle;
	public AudioSource explosionAudio;
	public float maxDamage = 100f; // 최대 대미지
	public float explosionForce = 1000f;
	public float lifeTime = 10f; // 생존시간
	public float explosionRadius = 20f; // 폭발 반경

	void Start() {
		Destroy(gameObject, lifeTime); // 생존시간 이후 삭제
	}

	// 삭제 전 게임매니저에게 전달
	private void OnDestroy() {
		GameManager.instance.OnBallDestroy();
	}

	// 충돌
	void OnTriggerEnter(Collider other)
	{
		// 영역에 있는 Prop 모두 조회
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, whatIsProp);
		
		// 조회된 Prop에게 대미지 입힘
		for(int i=0; i<colliders.Length; i++) {
			Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
			targetRigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
			Prop targetProp = colliders[i].GetComponent<Prop>();
			float damage = CalculateDamage(colliders[i].transform.position);
			targetProp.TakeDamage(damage);
		}

		// 폭발 이펙트는 부모에게 해방
		explosionParticle.transform.parent = null;
		explosionParticle.Play(); // 이펙트 실행
		explosionAudio.Play(); // 사운드 실행

		// 이펙트가 끝난 후 제거
		Destroy(explosionParticle.gameObject, explosionParticle.duration);
		Destroy(gameObject); // 공 제거
	}

	// 대미지 계산(폭발 반경에 가까울수록 maxDamage에 가깝다, 멀리있으면 0에 가까움)
	private float CalculateDamage(Vector3 targetPosition) {
		Vector3 explosionToTarget = targetPosition - transform.position;
		float distance = explosionToTarget.magnitude;
		float edgeToCenterDistance = explosionRadius - distance;
		float percentage = edgeToCenterDistance/explosionRadius;
		float damage = maxDamage * percentage;
		damage = Mathf.Max(0, damage); // 더 큰 수 반환(음수 방지)
		return damage;
	}
}
