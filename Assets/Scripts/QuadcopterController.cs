using UnityEngine;

public class QuadcopterController : MonoBehaviour
{
    public Transform[] propellers; // Ссылки на модели пропеллеров
    public float maxThrust = 100f; // Максимальная сила подъема
    public float maxTorque = 10f; // Максимальный крутящий момент
    public float forwardSpeed = 10f; // Скорость движения вперед/назад
    public float rotationSpeed = 5f; // Скорость поворота
    public float pitchPID = 1f; // Коэффициенты PID для наклона
    public float rollPID = 1f; // Коэффициенты PID для крена
    public float hoverHeight = 5f; // Высота, на которой дрон будет плавать при нулевом газе
    public float hoverForce = 10f; // Сила для поддержания высоты

    private Rigidbody rb;
    private float thrustInput;
    private float torqueInput;
    private float forwardInput;
    private float rotationInput;

    private PIDController pitchController;
    private PIDController rollController;

    private InputControl inputControl;

    private void Awake()
    {
        inputControl = new InputControl();
        //inputControl.Drone.KillSwitch.performed += context => killSwitchOnOff();
		//inputControl.Drone.Menu.performed += context => OnMenu();
		//inputControl.Drone.EditMode.performed += context => EditMode();
        rb = GetComponent<Rigidbody>();

        pitchController = new PIDController(pitchPID);
        rollController = new PIDController(rollPID);
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    private void OnDisable()
    {
        inputControl.Disable();
    }

    private void Update()
    {
        Vector3 direction = inputControl.Drone.Move.ReadValue<Vector3>();
		float forwardBackward = inputControl.Drone.ForwardBackward.ReadValue<float>();
		float leftRight = inputControl.Drone.LeftRight.ReadValue<float>();
		float slider2 = inputControl.Drone.slider2.ReadValue<float>();

        // Получаем ввод от игрока
        thrustInput = direction.y / 5;
        torqueInput = direction.x / 5;
        forwardInput = forwardBackward / 5;
        rotationInput = leftRight / 5;
    }

    private void FixedUpdate()
    {
        // Применяем силу подъема
        Vector3 thrustForce = transform.up * thrustInput * maxThrust;
        rb.AddForce(thrustForce);

        // Применяем крутящий момент
        Vector3 torque = new Vector3(0f, torqueInput * maxTorque, 0f);
        rb.AddTorque(torque);

        // Наклон квадрокоптера
        float pitch = pitchController.Update(-transform.forward.y, forwardInput);
        float roll = rollController.Update(transform.right.y, rotationInput);

        // Применяем наклон
        Vector3 pitchTorque = transform.right * pitch * maxTorque;
        Vector3 rollTorque = transform.forward * roll * maxTorque;
        rb.AddTorque(pitchTorque + rollTorque);

        // Движение вперед/назад
        Vector3 forwardForce = transform.forward * forwardInput * forwardSpeed;
        rb.AddForce(forwardForce);

        // Поворот
        Vector3 rotation = new Vector3(0f, rotationInput * rotationSpeed, 0f);
        Quaternion deltaRotation = Quaternion.Euler(rotation * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);

        // Поддержание высоты при нулевом газе
        if (thrustInput == 0)
        {
            float hoverError = hoverHeight - transform.position.y;
            float hoverCorrection = hoverError * hoverForce;
            rb.AddForce(transform.up * hoverCorrection);
        }
    }
}

// Простая реализация PID-регулятора
public class PIDController
{
    private float pGain;
    private float iGain;
    private float dGain;
    private float integral;
    private float previousError;

    public PIDController(float p, float i = 0f, float d = 0f)
    {
        pGain = p;
        iGain = i;
        dGain = d;
    }

    public float Update(float error, float input)
    {
        float proportional = error * pGain;
        integral += error * Time.fixedDeltaTime * iGain;
        float derivative = (error - previousError) / Time.fixedDeltaTime * dGain;

        float output = proportional + integral + derivative;

        previousError = error;

        return output * input;
    }
}