/// <summary>
/// Damage manager. 
/// </summary>
using UnityEngine;
using System.Collections;
using ScriptableObjectArchitecture;

namespace StrikeKit
{
	public class DamageManager : MonoBehaviour
	{
		public GameEvent onPlayerDead;
		public AudioClip[] HitSound;
		public GameObject Effect;
		public int HP = 100;
		public int HPmax;
		public ParticleSystem OnFireParticle;
		public GameEvent onEnemiesKilled;
		public GameEvent onCoinsAdd;
		public bool checkhealthbar { get; set; }
		private void Start()
		{
			if(this.gameObject.tag == "Player" && this.gameObject.layer == 6)
            {
				HP = Resolver.Instance._planesData.planesDetails[PlayerPrefs.GetInt("curSelectedPlaneNumForGame")].planeCurrentHealth;
			}
			
			checkhealthbar = false;
			HPmax = HP;
			if (OnFireParticle)
			{
				OnFireParticle.Stop();
			}
		}
		// Damage function

		
		public void ApplyDamage(int damage, GameObject killer)
		{
			if (HP < 0)
				return;

			if (HitSound.Length > 0)
			{
				AudioSource.PlayClipAtPoint(HitSound[Random.Range(0, HitSound.Length)], transform.position);
			}
			
			// enemy
			if(this.gameObject.tag == "Enemy" && !playerDead)
            {
				//HudManager.Instance.KilledEnemiesCount++;
				//HudManager.Instance._coin++;

				
				HP -= damage;

				checkhealthbar = true;
				//print("hit to enemy : hp = " + HP + " damage = " + damage);
				//SetEnemiesHealthBar();

				if (HP <= 0)
				{
					PlayerPrefs.SetInt("KilledEnemiesCount", PlayerPrefs.GetInt("KilledEnemiesCount") + 1);
					onEnemiesKilled.Raise();
					//PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 100);

					HudManager.Instance.ScoreAndCoin();
					Dead();
				}


			}
			// player
			else if (this.gameObject.layer == 6)
            {
				HP -= damage;
				//print("hit to layer 6 : hp = " + HP + " damage = " + damage);
				HudManager.Instance.SetPlayerHealthBar();
				if (HP <= 0)
                {
					Dead();
                }
			}
			// friend
			else if (this.gameObject.tag == "Player" && this.gameObject.layer == 0)
            {
				HP -= damage;
				//print("hit to player tag layer 0 : " + HP);
				if (HP <= 0)
				{
					Dead();
				}
			}
				
			if (OnFireParticle)
			{
				if (HP < (int)(HPmax / 2.0f))
				{
					OnFireParticle.Play();
				}
			}
			
		}



		private void Dead()
		{
			if (Effect)
			{
				GameObject obj = (GameObject)GameObject.Instantiate(Effect, transform.position, transform.rotation);
				if (this.GetComponent<Rigidbody>())
				{
					if (obj.GetComponent<Rigidbody>())
					{
						obj.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
						//obj.GetComponent<Rigidbody>().AddTorque(Random.rotation.eulerAngles * Random.Range(100, 2000));
					}
				}
			}
			//player
			if (this.gameObject.layer == 6)
            {
				
				if (PlayerPrefs.GetInt("Record") < HudManager.Instance.KilledEnemiesCount)
                {
					PlayerPrefs.SetInt("Record", HudManager.Instance.KilledEnemiesCount);

				}
				//Time.timeScale = 0;
				onPlayerDead.Raise();
				Destroy(this.gameObject);

			}
			// friends
			if (this.gameObject.tag == "Player" && this.gameObject.layer == 0)
            {
				Destroy(this.gameObject);
			}
			
			// we must destroy Aims

		}


		bool playerDead;
		public void DestroyAfterPlayer()
        {
			playerDead = true;
			if (Effect)
			{
				GameObject obj = (GameObject)GameObject.Instantiate(Effect, transform.position, transform.rotation);
				if (this.GetComponent<Rigidbody>())
				{
					if (obj.GetComponent<Rigidbody>())
					{
						obj.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
						obj.GetComponent<Rigidbody>().AddTorque(Random.rotation.eulerAngles * Random.Range(100, 2000));
					}
				}
			}
			Destroy(Resolver.Instance.gameObject);
			Destroy(this.gameObject);

		}

	}
}

