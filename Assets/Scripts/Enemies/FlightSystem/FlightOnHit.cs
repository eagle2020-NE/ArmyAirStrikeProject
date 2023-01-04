/// <summary>
/// Flight on hit. this script will take damage to the plane. If got hit by an object that are Taged in the list.
/// </summary>
using UnityEngine;
using System.Collections;


namespace StrikeKit
{
	public class FlightOnHit : MonoBehaviour
	{

		public string[] Tag = new string[1] { "Scene" };// All scene object tag.
		public string AirportTag = "Airport";// air port tag.
		public string RocketTag = "Rocket";// Rocket tag.
		public string BulletTag = "Bullet";// Bullet tag.
		public int RocketDamage = 3;
		public int BulletDamage = 1;
		public int Damage = 100;
		public AudioClip[] SoundOnHit;
		void Start()
		{

		}

		private void OnCollisionEnter(Collision collision)
		{
			bool hit = false;

			for (int i = 0; i < Tag.Length; i++)
			{
				if (collision.gameObject.tag == Tag[i])
				{
					hit = true;

				}
				if (collision.gameObject.tag == AirportTag)
				{
					hit = false;
				}
			}

			if (hit)
			{
				if (SoundOnHit.Length > 0)
					AudioSource.PlayClipAtPoint(SoundOnHit[Random.Range(0, SoundOnHit.Length)], this.transform.position);
				if (this.GetComponent<DamageManager>())
				{
					if (collision.gameObject.tag == RocketTag)
					{
						//print("___________R");
						this.GetComponent<DamageManager>().ApplyDamage(RocketDamage, collision.gameObject);
						

					}
					else if (collision.gameObject.tag == BulletTag)
					{
						//print("_________B");
						this.GetComponent<DamageManager>().ApplyDamage(BulletDamage, collision.gameObject);
						

					}
					else
					{
						//print("_________Compeletly " + collision.gameObject.name);
						this.GetComponent<DamageManager>().ApplyDamage(Damage, collision.gameObject);
						

					}

				}

			}
			if (this.gameObject.layer == 6)
            {
				
			}
			
		}
	}
}

