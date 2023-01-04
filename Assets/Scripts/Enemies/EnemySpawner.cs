/// <summary>
/// Enemy spawner. auto Re-Spawning an Enemy by Random index of Objectman[]
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;

namespace StrikeKit
{
    public enum SawnType
    {
        none,
        friend,
        enemy,
        
    }
	public class EnemySpawner : MonoBehaviour
	{
        public SawnType _spawnType =  SawnType.none;

        
        public bool Enabled = true;
		public GameObject[] Objectman; // object to spawn
		public float timeSpawn = 3;
		public int havingNumInScene = 2;
		public int MaxNumInScene = 2;
		public int timeIntervalAddOneUnit = 40;
		public int radius = 10;
		public string Tag = "Enemy";
		public string Type = "Enemy";
		private float timetemp = 0;
		private int indexSpawn;

        //[HideInInspector]
        public Transform SpawnerTrans;
        public int EnemyFirstCount;

        public GameObject _hudSystem;
        bl_HudManager hudSys;

        public Texture2D Icon;
        public Texture arrowIcon;
        public Color iconColor;

        float timer;
        private void Awake()
        {
            //// hud system initialize
            //if (FindObjectOfType<bl_HudManager>() != null)
            //{
            //    hudSys = FindObjectOfType<bl_HudManager>();
            //}
            //else
            //{
            //    GameObject hud = Instantiate(_hudSystem, null);
            //    hud.transform.position = transform.position;
            //    hudSys = hud.GetComponent<bl_HudManager>();
            //    //hud.SetActive(false);
            //}

            //GameObject hud = Instantiate(_hudSystem, null);
            //hud.transform.position = transform.position;
            //hudSys = hud.GetComponent<bl_HudManager>();

            //hudSys.LocalPlayer = GameObject.Find("CM Camera player").transform;
            //hudSys.clampBorder = 15;
            //hudSys.useGizmos = true;

            //hudSys.IconSize = 50;
            //hudSys.OffScreenIconSize = 20;
            //hudSys.AutoScale = true;
        }
        void Start()
		{
			indexSpawn = Random.Range(0, Objectman.Length);
			timetemp = Time.time;
            SpawnerTrans = this.transform;
            


            

            Generator(EnemyFirstCount);
        }

		void Update()
        {
            timer += Time.deltaTime;
            if (timer > timeIntervalAddOneUnit  && havingNumInScene < MaxNumInScene)
            {
                havingNumInScene++;
                timer = 0;
            }
            
            // transform.position = player.transform.position
            if (Resolver.Instance.playerTrans != null)
            {
                transform.position = SpawnerTrans.position;
            }
            

            if (!Enabled)
                return;

            var gos = GameObject.FindGameObjectsWithTag(Tag);

            if (gos.Length < havingNumInScene && Time.time > timetemp + timeSpawn)
            {
                // spawing an enemys by random index of Objectman[]
                Generator(1);
            }
            //if(_spawnType == SawnType.friend)
            //{
            //    print(" friends count : " + hudSys.Huds.Count);
            //}
            

        }

        private void Generator(int firstEnemyCount)
        {
            timetemp = Time.time;

            //radius = Random.Range(radius, 2 * radius);

            for (int i = 0; i < firstEnemyCount; i++)
            {
                float r = Random.Range(-radius, radius);
                if (r < 0 && Mathf.Abs(r) < radius / 2)
                {
                    r -= radius / 2;
                }
                else if(r > 0 && r < radius / 2)
                {
                    r += radius / 2;
                }
                //else
                //{
                //    r += radius / 10;
                //}
                //print(" spawn radius 1000 :   " + r);
                Vector3 spawnDistance = new Vector3(r, 0, 0);
                GameObject obj = (GameObject)GameObject.Instantiate(Objectman[indexSpawn], transform.position + spawnDistance, Quaternion.identity);
                if (obj.CompareTag("Enemy"))
                {
                    obj.GetComponent<AIController>().AttackRate = Random.Range(0, 10);
                }
                

                obj.tag = Tag;
                indexSpawn = Random.Range(0, Objectman.Length);


                /////////////////////////////////////////// add enemy transform to hud system  ///////////////////////////////////////////////
                
                if (_spawnType == SawnType.friend || _spawnType == SawnType.enemy)
                {
                    GameObject hud = Instantiate(_hudSystem, obj.transform);
                    
                    hud.transform.position = transform.position;

                    hudSys = hud.GetComponent<bl_HudManager>();

                    hudSys.Huds[i].m_Target = obj.transform;

                    hudSys.LocalPlayer = GameObject.Find("CM Camera player").transform;
                    hudSys.clampBorder = 15;
                    hudSys.useGizmos = true;

                    hudSys.IconSize = 50;
                    hudSys.OffScreenIconSize = 20;
                    hudSys.AutoScale = true;

                    

                    hudSys.Huds[i].m_Text = obj.name;
                    hudSys.Huds[i].m_Icon = Icon;
                    hudSys.Huds[i].Arrow.ArrowIcon = arrowIcon;
                    //hudSys.Huds[i].m_Target = obj.transform;
                    hudSys.Huds[i].m_Color = iconColor;

                    hudSys.Huds[i].m_TypeHud = TypeHud.Decreasing;

                    hudSys.Huds[i].m_MaxSize = 60;
                    hudSys.Huds[i].ShowDistance = true;
                    hudSys.Huds[i].Arrow.ShowArrow = true;
                    hudSys.Huds[i].Arrow.ArrowSize = 35;
                }
               

                //hudSys.Huds.Add(hudSys.Huds[i]);
            }
            
        }

        public void CheckPlayerDead()
        {
            this.gameObject.SetActive(false);
        }

    }



}
