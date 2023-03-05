using StrikeKit;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Aircraft : MonoBehaviour
{
    public Rigidbody rb;

    private InputProccessor _inputProcessor;
    public InputStructure InputStructure { get; private set; }

    protected AircraftStructure _aircraftStructure;
    public AircraftFlightState FlightState { get; private set; }

    private FlightController _flightController;

    private EnemySpawner[] enemySpawner;

    public DirectorManual directorManual;



    //[Header("ROCKET")]
    //public AAPod aaPod;
    public Transform Target;

    [Header("WindyEffect")]
    public ParticleSystem windLeft;
    public ParticleSystem windRight;

    ParticleSystem.EmissionModule windEmissionLeft;
    ParticleSystem.EmissionModule windEmissionRight;

    [Header("Speed Blues")]
    public ParticleSystem blueSpeedLeft;
    public ParticleSystem blueSpeedRight;

    ParticleSystem.EmissionModule speedblueLeftEmission;
    ParticleSystem.EmissionModule speedblueRightEmission;

    InputAxis _throttleAxis;

    //public Animator anim;
    public bool inShop;
    private void Awake()
    {
        //print("Disabled");
        //if (!inShop)
        //{
        //    print("enabled");
        //    windEmissionLeft = windLeft.emission;
        //    windEmissionRight = windRight.emission;

        //    speedblueLeftEmission = blueSpeedLeft.emission;
        //    speedblueRightEmission = blueSpeedRight.emission;

        //    rb = GetComponent<Rigidbody>();
        //    InputStructure = new InputStructure();
        //    _inputProcessor = new InputProccessor(InputStructure);
        //    enemySpawner = FindObjectsOfType<EnemySpawner>();
        //    _aircraftStructure = new AircraftStructure()
        //    {
        //        Engines = GetComponentsInChildren<AircraftEngine>(true),
        //        Surfaces = GetComponentsInChildren<AircraftControlSurface>(true),
        //        Wheels = GetComponentsInChildren<AircraftWheel>(true),
        //    };

        //    FlightState = new AircraftFlightState();
        //    _flightController = new FlightController(Resolver.Instance.settings.flight, Resolver.Instance.settings.engine);

        //}

        windEmissionLeft = windLeft.emission;
        windEmissionRight = windRight.emission;

        speedblueLeftEmission = blueSpeedLeft.emission;
        speedblueRightEmission = blueSpeedRight.emission;

        rb = GetComponent<Rigidbody>();
        InputStructure = new InputStructure();
        
        enemySpawner = FindObjectsOfType<EnemySpawner>();
        _aircraftStructure = new AircraftStructure()
        {
            Engines = GetComponentsInChildren<AircraftEngine>(true),
            Surfaces = GetComponentsInChildren<AircraftControlSurface>(true),
            Wheels = GetComponentsInChildren<AircraftWheel>(true),
        };

        FlightState = new AircraftFlightState();
        if(GameObject.Find("Resolver") != null)
        {
            _flightController = new FlightController(Resolver.Instance.settings.flight, Resolver.Instance.settings.engine);
            _inputProcessor = new InputProccessor(InputStructure);
        }
        


        
    }

    private void Start()
    {
        _flightController.Initialize(transform, rb, _aircraftStructure, InputStructure, FlightState, windEmissionLeft, windEmissionRight);

        _throttleAxis = InputStructure.Axes.Single(a => a.Type == InputAxisType.Throttle);
    }

    private void Update()
    {
        _inputProcessor.Tick();
        for(int i = 0; i < enemySpawner.Length; i++)
        {
            enemySpawner[i].SpawnerTrans = this.transform;
        }
        //enemySpawner.SpawnerTrans = this.transform;
       //print("aircraft velocity " + rb.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        _flightController.FixedTick();
        ChangeSpeedBlue(_throttleAxis.Value);
    }

    public void Dispose()
    {
        //InputStructure.Dispose();
        _inputProcessor.Dispose();
    }

    //public void LaunchRocket()
    //{
    //    aaPod.Launch(Target);
    //}



    [System.Serializable]
    public class DirectorManual
    {
        public Transform vCamFollowTarget;
        public Transform vCamLookAtTarget;
        public Transform Up;
        public Transform Down;
        public Transform Left;
        public Transform Right;
    }


    public void ChangeSpeedBlue(float throthle)
    {
        speedblueLeftEmission.rate = throthle * 50;
        speedblueRightEmission.rate = throthle * 50; 
    }
}
