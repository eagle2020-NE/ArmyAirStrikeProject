using UnityEngine;
using System.Collections;


namespace StrikeKit
{
	public class DeadTime : MonoBehaviour
	{

		public float LifeTime = 3;
		void Start()
		{
			Destroy(this.gameObject, LifeTime);
		}

		// Update is called once per frame
		void Update()
		{

		}
	}

}
