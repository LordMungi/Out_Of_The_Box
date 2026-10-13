using UnityEngine;

public class GrabberState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    private PartHandler _partHandler;
    
    private enum Phase { Outward, Inward }
    private Phase _currentPhase;

    private float _travelDistance;
    private float _maxDistance = 15.0f; 
    private float _handSpeed = 20.0f;   
    private UnityEngine.Vector3 _shootDirection;
    
    public override void Enable()
    {
        EventBus.Subscribe<OnGrabRequest>(OnGrabRequest);
        EventBus.Subscribe<OnGrabberHitWall>(OnGrabberHitWall);
        
        _currentPhase = Phase.Outward;
        _travelDistance = 0.0f;
        
        _shootDirection = Camera.main.transform.forward;
    }
    
    public override void Disable()
    {
        EventBus.Unsubscribe<OnGrabRequest>(OnGrabRequest);
        EventBus.Unsubscribe<OnGrabberHitWall>(OnGrabberHitWall);
    }

    public GrabberState(ref PlayerData data) : base(ref data)
    {
        _partHandler = data.handlers.parts;
    }

    public override void Update()
    {
        if (_currentPhase == Phase.Outward)
        {
            _partHandler.MoveHandOutward(_shootDirection, _handSpeed);
            _travelDistance += _handSpeed * Time.deltaTime;

            if (_travelDistance >= _maxDistance)
            {
                _currentPhase = Phase.Inward;
            }
        }
        else if (_currentPhase == Phase.Inward)
        {
            bool hasReturned = _partHandler.MoveHandInward(_handSpeed);
            
            if (hasReturned)
            {
                _partHandler.FinalizeGrab();

                EventBus.Raise<OnPlayerStateChangeRequest>(BehaviourFSM.State.Idle);
            }
        }
    }
    
    private void OnGrabRequest(in OnGrabRequest onGrabRequest)
    {
        if (_currentPhase == Phase.Outward)
        {
            _partHandler.CatchPartInAir(onGrabRequest.grabbedPart);
            _currentPhase = Phase.Inward;
        }
    }    
    private void OnGrabberHitWall(in OnGrabberHitWall onGrabRequest)
    {
        if (_currentPhase == Phase.Outward)
            _currentPhase = Phase.Inward;
    }

}