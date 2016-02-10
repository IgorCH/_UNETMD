using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AirplaneController : NetworkBehaviour
{

    #region Magic Nums
    private const float MaxSpeed = 700f;
    private const float Acceleration = 150f;
    #endregion

    private bool _isCrashed = false;

    private float _rotX = 0.0F;
    private float _rotY = 0.0F;
    private float _rotZ = 0.0F;

    private float _speed = 0.0F;
    private float _upLift = 0.0F;
    private const float PseudoGravitation = -0.3F;
    private float _rightLeftSoft = 0.0F;
    private float _rightLeftSoftAbs = 0.0F;

    private float _diveSalto = 0.0F;
    private float _diveBlocker = 0.0F;

    private GroundTrigger _groundTrigger;
    private FrontTrigger _frontTrigger;
    private FrontUpTrigger _frontUpTrigger;
    private RearTrigger _rearTrigger;

    public GameObject GroundTriggerObject;
    public GameObject FrontTriggerObject;
    public GameObject FrontUpTriggerObject;
    public GameObject RearTriggerObject;

	public Transform LeftMachineGun;
	public Transform RightMachineGun;
	public GameObject AirplaneMachineGunBullet;

	private Rigidbody rigidbody;
	private Transform transform;

    void Awake()
    {
        _groundTrigger = GroundTriggerObject.GetComponent<GroundTrigger>();
        _frontTrigger = FrontTriggerObject.GetComponent<FrontTrigger>();
        _frontUpTrigger = FrontUpTriggerObject.GetComponent<FrontUpTrigger>();
        _rearTrigger = RearTriggerObject.GetComponent<RearTrigger>();

		rigidbody = GetComponent<Rigidbody> ();
		transform = GetComponent<Transform> ();
    }


	void Start ()
    {
		_groundTrigger.IsTriggered = true;
	}
	
	void Update ()
	{
	    ManualUpdate();
	}

    public void ManualUpdate ()
    {

        if (_isCrashed) return;

        _rotX = transform.eulerAngles.x;
        _rotY = transform.eulerAngles.y;
        _rotZ = transform.eulerAngles.z;

        if (Input.GetAxis("Vertical1") <= 0 && _speed > 595) transform.Rotate((Input.GetAxis("Vertical1") * Time.deltaTime * 80), 0, 0);
        if (Input.GetAxis("Vertical1") > 0 && _speed > 595) transform.Rotate((0.8F - _diveSalto) * (Input.GetAxis("Vertical1") * Time.deltaTime * 80F), 0F, 0F);

        if (_groundTrigger.IsTriggered) transform.Rotate(0, Input.GetAxis("Horizontal1") * Time.deltaTime * 30, 0, Space.World);
        if (!_groundTrigger.IsTriggered) transform.Rotate(0, Time.deltaTime * 100 * _rightLeftSoft, 0, Space.World);

        if (!_groundTrigger.IsTriggered) transform.Rotate(0F, 0F, Time.deltaTime * 100F * (1.0F - _rightLeftSoftAbs - _diveBlocker) * Input.GetAxis("Horizontal1") * -1.0F);

        if (Input.GetAxis("Horizontal1") <= 0 && _rotZ > 0 && _rotZ < 90) _rightLeftSoft = _rotZ * 2.2F / 100F * -1F;
        if (Input.GetAxis("Horizontal1") >= 0 && _rotZ > 270) _rightLeftSoft = (7.92F - _rotZ * 2.2F / 100F);

        if (_rightLeftSoft > 1) _rightLeftSoft = 1;
        if (_rightLeftSoft < -1) _rightLeftSoft = -1;

        if (_rightLeftSoft > -0.01 && _rightLeftSoft < 0.01) _rightLeftSoft = 0;

        _rightLeftSoftAbs = Mathf.Abs(_rightLeftSoft);

        if (_rotX < 90)
        {
            _diveSalto = _rotX / 100.0F;
            _diveBlocker = _rotX / 200.0F;
        }
        else
        {
            _diveSalto = -0.2F;
            _diveBlocker = 0;
        }

        if (_rotZ < 180 && Input.GetAxis("Horizontal1") > 0) transform.Rotate(0, 0, _rightLeftSoft * Time.deltaTime * 80F);
        if (_rotZ > 180 && Input.GetAxis("Horizontal1") < 0) transform.Rotate(0, 0, _rightLeftSoft * Time.deltaTime * 80F);

        if (!Input.GetButton("Horizontal1"))
        {
            if (_rotZ < 135) transform.Rotate(0, 0, _rightLeftSoftAbs * Time.deltaTime * -100);
            if (_rotZ > 225) transform.Rotate(0, 0, _rightLeftSoftAbs * Time.deltaTime * 100);
        }

        //TODO NEED FIX
        if (!Input.GetButton("Vertical1") && !_groundTrigger.IsTriggered)
        {
            //if (_rotX > 0 && _rotX < 180) transform.Rotate(Time.deltaTime * -50, 0, 0);
            //if (_rotX > 0 && _rotX > 180) transform.Rotate(Time.deltaTime * 50, 0, 0);
        }

        transform.Translate(0, 0, _speed / 20 * Time.deltaTime);

        if (_groundTrigger.IsTriggered && Input.GetKey(KeyCode.R) && _speed < MaxSpeed) _speed += Time.deltaTime * Acceleration;
        if (_groundTrigger.IsTriggered && Input.GetKey(KeyCode.F) && _speed > 0) _speed -= Time.deltaTime * Acceleration;

        if (_speed < 0) _speed = 0;

        transform.Translate(0F, _upLift * Time.deltaTime / 10.0F, 0F);

        _upLift = -500 + _speed;

        if (_groundTrigger.IsTriggered && _upLift < 0) _upLift = 0;

        if (_speed < 595)
        {
            if (!_frontTrigger.IsTriggered && _rearTrigger.IsTriggered) transform.Rotate(Time.deltaTime * 20, 0, 0);
            if (_frontTrigger.IsTriggered && !_rearTrigger.IsTriggered) transform.Rotate(Time.deltaTime * -20, 0, 0);
            if (_frontUpTrigger.IsTriggered) transform.Rotate(Time.deltaTime * -20, 0, 0);
            if (!_groundTrigger.IsTriggered) transform.Translate(0, PseudoGravitation * Time.deltaTime / 10.0F, 0);
        }

		if (Input.GetMouseButtonDown (0))
		{
			MachineGunFire ();		
		}

		if (Input.GetMouseButtonDown (1))
		{
			BombFire ();		
		}
			
		if (Input.GetKeyDown (KeyCode.Q))
		{
			Catapult ();		
		}
			
    }

	private void MachineGunFire()
	{
		if (UnityEngine.Random.Range (0, 10) < 5) {
			CmdMachineGunFire (rigidbody.velocity, 200f, LeftMachineGun.forward, LeftMachineGun.position + LeftMachineGun.forward * 5, LeftMachineGun.rotation);
		} else {
			CmdMachineGunFire (rigidbody.velocity, 200f, RightMachineGun.forward, RightMachineGun.position + RightMachineGun.forward * 5, RightMachineGun.rotation);	
		}

	}

	[Command]
	private void CmdMachineGunFire(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation)
	{
		GameObject newAirplaneMachineGunBullet = Instantiate(AirplaneMachineGunBullet, position, rotation) as GameObject;
		newAirplaneMachineGunBullet.GetComponent<Rigidbody>().velocity = rigidbodyVelocity + launchForce * forward;
		NetworkServer.Spawn(newAirplaneMachineGunBullet);
	}

	private void BombFire()
	{
		CmdBombFire(rigidbody.velocity, 3f, -transform.up, transform.position - transform.up * 5, transform.rotation);
	}

	[Command]
	private void CmdBombFire(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation)
	{
		GameObject newAirplaneMachineGunBullet = Instantiate(AirplaneMachineGunBullet, position, rotation) as GameObject;
		newAirplaneMachineGunBullet.GetComponent<Rigidbody>().velocity = rigidbodyVelocity + launchForce * forward;
		NetworkServer.Spawn(newAirplaneMachineGunBullet);
	}

	public void Catapult ()
	{
		
	}

	[Command]
	public void CmdCatapult ()
	{
		
	}

    void OnCollisionEnter(Collision collision)
    {
        if (!_groundTrigger.IsTriggered)
        {
            _isCrashed = true;
            _groundTrigger.IsTriggered = true;
            _speed = 0;
			rigidbody.useGravity = true;
        }
    }

}
