using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
	public float MoveSpeed = 10f;
	// How quickly the camera should move from point A to B.
	public float SnapDistance = 0.25f;
	// How far from the new position we should be before snapping to it.
	public Transform MainAxis;
	// Axis that moves the camera
	public Transform ShakeAxis;
	// Axis that shakes the camera

	// For moving camera
	public bool IsMoving { get; private set; }

	public bool IsShaking {
		get {
			return _isShaking;
		}
	}

	private Vector3 _newPosition;
	private float _currentMoveSpeed;

	// For shaking camera
	private bool _isShaking = false;
	private int _shakeCount;
	private float _shakeIntensity, _shakeSpeed, _baseX, _baseY;
	private Vector3 _nextShakePosition;


	void Start ()
	{
		enabled = false;

		// Set up base positions, these are used for shaking to determine where to return to after a shake.
		_baseX = ShakeAxis.localPosition.x;
		_baseY = ShakeAxis.localPosition.y;
	}

	
	void Update ()
	{
		// Are we moving?
		if (IsMoving) {
			// Move us toward the new position
			MainAxis.position = Vector3.MoveTowards (MainAxis.position, _newPosition, Time.deltaTime * _currentMoveSpeed);

			// Determine if we are there or not (within snap distance)
			if (Vector2.Distance (MainAxis.position, _newPosition) < SnapDistance) {
				MainAxis.position = _newPosition;
				IsMoving = false;
				if (!_isShaking)
					enabled = false;
			}
		}
		// ...or are we shaking? (Could be both)
		if (_isShaking) {
			// Move toward the previously determined next shake position
			ShakeAxis.localPosition = Vector3.MoveTowards (ShakeAxis.localPosition, _nextShakePosition, Time.deltaTime * _shakeSpeed);

			// Determine if we are there or not
			if (Vector2.Distance (ShakeAxis.localPosition, _nextShakePosition) < _shakeIntensity / 5f) {
				//Decrement shake counter
				_shakeCount--;

				// If we are done shaking, turn this off if we're not longer moving
				if (_shakeCount <= 0) {
					_isShaking = false;
					ShakeAxis.localPosition = new Vector3 (_baseX, _baseY, ShakeAxis.localPosition.z);
					if (!IsMoving)
						enabled = false;
				}
                // If there is only 1 shake left, return back to base
                else if (_shakeCount <= 1) {
					_nextShakePosition = new Vector3 (_baseX, _baseY, ShakeAxis.localPosition.z);
				}
                // If we are not done or nearing done, determine the next position to travel to
                else {
					DetermineNextShakePosition ();
				}
			}
		}
	}


	/// <summary>
	/// Move the camera in a certain direction by a certain distance.
	/// </summary>
	/// <param name="x">Distance along x axis to move.</param>
	/// <param name="y">Distance along y axis to move.</param>
	/// <param name="speed">How quickly to move in specified direction.</param>
	public void Move (float x, float y, float speed = 0)
	{
		// If a speed is passed in, use that. Otherwise use the default.
		if (speed > 0)
			_currentMoveSpeed = speed;
		else
			_currentMoveSpeed = MoveSpeed;

		// Set us up to move
		_newPosition = new Vector3 (transform.position.x + x, transform.position.y + y, transform.position.z);
		IsMoving = true;
		enabled = true;
	}


	/// <summary>
	/// Immediately sets the position of the camera
	/// </summary>
	public void SetPosition (Vector2 position)
	{
		Vector3 newPosition = new Vector3 (position.x, position.y, MainAxis.position.z);
		MainAxis.position = newPosition;
	}


	/// <summary>
	/// Shakes the camera. Essentially places some random points around the camera and lerps it to them.
	/// </summary>
	/// <param name="intensity">Max distance from the center point the camera will travel.</param>
	/// <param name="shakes">Total number of random points the camera will travel to.</param>
	/// <param name="speed">How quickly the camera moves from point to point.</param>
	public void Shake (float intensity, int shakes, float speed)
	{
		enabled = true;
		_isShaking = true;
		_shakeCount = shakes;
		_shakeIntensity = intensity;
		_shakeSpeed = speed;

		DetermineNextShakePosition ();
	}


	private void DetermineNextShakePosition ()
	{
		_nextShakePosition = new Vector3 (Random.Range (-_shakeIntensity, _shakeIntensity),
			Random.Range (-_shakeIntensity, _shakeIntensity),
			ShakeAxis.localPosition.z);
	}
}