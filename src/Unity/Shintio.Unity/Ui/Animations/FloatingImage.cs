using UnityEngine;

namespace Shintio.Unity.Ui.Animations
{
	public class FloatingImage : MonoBehaviour
	{
		public float Amplitude = 10.0f;
		public float Speed = 1.0f;

		private RectTransform _rectTransform = null!;
		private Vector3 _initialPosition;

		private void Start()
		{
			Input.gyro.enabled = true;
			
			_rectTransform = GetComponent<RectTransform>();
			_initialPosition = _rectTransform.anchoredPosition;
		}

		private void Update()
		{
			// // var gyro = GyroToUnity(Input.gyro.attitude);
			// // var rotationX = Mathf.Repeat(gyro.eulerAngles.y - 180, 360.0f) - 180f;
			//
			// Debug.Log(Input.gyro.rotationRateUnbiased.y);
			//
			// var newX = _initialPosition.x;// + rotationX / 2; 
			// var newY = _initialPosition.y + Mathf.Sin(Time.time * Speed) * Amplitude;
			//
			// _rectTransform.anchoredPosition = new Vector3(newX, newY, _initialPosition.z);
			
			// Получаем угол наклона устройства по осям X и Y (Pitch и Roll)
			float tiltX = Input.gyro.attitude.eulerAngles.x;  // Поворот по оси X
			float tiltY = Input.gyro.attitude.eulerAngles.y;  // Поворот по оси Y

			// Нормализуем углы для более плавного эффекта
			tiltX = (tiltX > 180) ? tiltX - 360 : tiltX;
			tiltY = (tiltY > 180) ? tiltY - 360 : tiltY;

			// Смещаем объект в зависимости от угла наклона
			Vector3 offset = new Vector3(tiltY * 0.5f, tiltX * 0.5f, 0);
			Debug.Log(offset);
			transform.position = _initialPosition + offset;
		}
		
		private static Quaternion GyroToUnity(Quaternion q)
		{
			return new Quaternion(q.x, q.y, -q.z, -q.w);
		}
	}
}