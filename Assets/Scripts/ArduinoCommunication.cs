using UnityEngine; // Core Unity library for game objects and components.
using System.Collections; // For collections and coroutines.
using UnityEngine.SceneManagement;  // For managing and transitioning between scenes.

public class ArduinoCommunication : MonoBehaviour // Manages communication with the Arduino.
{
    // Reference to each object in the scene.
    private SerialController serialController;
    public GameObject earth;
    public GameObject mars;
    public GameObject rocket;
    public GameObject saturnRing; 
    public GameObject sun;
    public GameObject moon;
    public GameObject neptune;
    public GameObject uranus;
    public GameObject jupiter;
    public GameObject venus;
    public GameObject mercury;

    public Camera mainCamera; // Reference to the main camera in the Unity scene.

    // Audio source for object-related sounds or effects.
    public AudioSource earthInfo;
    public AudioSource saturnInfo;
    public AudioSource rocketAudio;
    public AudioSource sunAudio;
    public AudioSource neptuneAudio;
    public AudioSource uranusAudio;
    public AudioSource jupiterAudio;
    public AudioSource venusAudio;
    public AudioSource mercuryAudio;
    public AudioSource marsAudio;
    public AudioSource stopCall;
    public AudioSource moonInfo;

    public float rocketSpeed = 4f; // The speed at which the rocket moves during its launch sequence.
    public float rocketDuration = 30f; // The total duration of the rocket launch animation.
    public float glowDuration = 8.7f;  // The duration for which the Sun's glow effect lasts.
    public float maxGlowScale = 4.25f; // The maximum scale for the Sun's glow effect during the animation.

    public float durationRotate = 9f; // The duration for which general rotational animations are performed.
    public float durationEarth = 14.5f; // The specific duration for Earth's rotation and orbit animation.
    public float rotationSpeed = 50f; // The speed of rotation.



    // Defines the camera's position relative to the rocket during its launch animation.
    public Vector3 rocketCameraOffset = new Vector3(0, 2, -3);

    // Specifies the camera's position for viewing the entire solar system.
    public Vector3 solarSystemCameraPosition = new Vector3(0, 20, -40);

    // Sets the camera's rotation angle for the solar system view.
    public Quaternion solarSystemCameraRotation = Quaternion.Euler(30, 0, 0);

    // Tracks whether the rocket launch animation is currently active to prevent multiple launches.
    private bool isRocketLaunching = false;

    void Start()
    {
        //Finds the GameObject named "SerialController" in the scene and get its SerialController componen, for handling communication with an Arduino
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();

        // Ensures the camera starts in the correct position to view the rocket.
        mainCamera.transform.position = rocket.transform.position + rocketCameraOffset;

        // This ensures the rocket is the main focus when the scene starts.
        mainCamera.transform.LookAt(rocket.transform.position);
    }

    void Update()
    {
        // Read a message from the SerialController (communication with Arduino).
        string message = serialController.ReadSerialMessage();

        // // If no message is received, exit the method.
        if (message == null) 
        
        return;
        
        // Log the received message for debugging purposes.
        Debug.Log("Message from Arduino: " + message);


        // // Check if the received message corresponds to planet names declared in arduino and call method.
        if (message == "Earth")
        {
            RotateEarth();
        }
        else if (message == "Mars")
        {
            RotateMars();
        }
        else if (message == "Saturn")
        {
            RotateSaturnRing();
        }
        else if (message == "Sun")
        {
            StartCoroutine(SunGlowEffect());
        }
        else if (message == "LaunchRocket" && !isRocketLaunching)
        {
            StartCoroutine(LaunchRocketSequence());
        }
        else if (message == "StopEverything")
        {
            ResetScene();
        }
        else if (message == "Moon")
        {
            OrbitMoon();
        }
        else if (message == "Neptune")
        {
            AnimateNeptune();
        }
        else if (message == "Uranus")
        {
            AnimateUranus();
        }
        else if (message == "Jupiter")
        {
        AnimateJupiter();
        }
        else if (message == "Venus")
        {
        AnimateVenus();
        }
        else if (message == "Mercury")
        {
        AnimateMercury();
        }
    }

    void ResetScene()
    {
        // Log a message to indicate the scene is resetting.
        Debug.Log("Resetting scene where everything started.");

         // Check if the stopCall AudioSource exists.
        if (stopCall != null)
        {
            // Prevent the stopCall object from being destroyed when the scene reloads.
            DontDestroyOnLoad(stopCall.gameObject);

             // Play the stopCall audio clip.
            stopCall.Play();
        }
        // Reload the current active scene by its build index.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Start a coroutine to handle cleaning up the stopCall audio source after resetting.
        StartCoroutine(ResetAudioPersistence());
    }

    private IEnumerator ResetAudioPersistence()
    {
        // Waits for one frame before continuing to ensure scene reload is complete.
        yield return null;

        // Checks if the stopCall AudioSource exists.
        if (stopCall != null)
        {   
             // Temporarily disables the stopCall game object to prevent it from being active.
            stopCall.gameObject.SetActive(false);
            // Destroys the stopCall game object, removing it completely from the scene.
            Destroy(stopCall.gameObject);
        }
    }

    // Method to initiate the Earth rotation animation.
    void RotateEarth()
    {
        // Log a message indicating Earth rotation is starting.
        Debug.Log("Rotating Earth...");
         // Start the coroutine responsible for rotating the Earth.
        StartCoroutine(RotateEarthCoroutine());

         // Check if the AudioSource for Earth's information exists.
        if (earthInfo != null)
        {
            // Play the Earth's audio clip.
            earthInfo.Play();
        }
    }

    // Coroutine for rotating the Earth and animating its movement.
    private IEnumerator RotateEarthCoroutine()
    {
     // Initialize elapsed time to track the duration of the rotation.
    float elapsedTime = 0f;
    Vector3 axis = Vector3.up;  // Define the axis of rotation as the upward direction (Y-axis).

    while (elapsedTime < durationEarth) // Continue rotating until the specified duration is reached.
    {
         // Rotate the Earth on its axis using the rotationSpeed and delta time.
        earth.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Simulate Earth's orbit by rotating it around the Sun's position along the defined axis.
        earth.transform.RotateAround(sun.transform.position, axis, rotationSpeed * Time.deltaTime);

        // Increment elapsed time by the time passed since the last frame.
        elapsedTime += Time.deltaTime;
        // Wait for the next frame before continuing.
        yield return null;
    }

    // Log a message indicating the Earth rotation and movement have completed.
    Debug.Log("Earth rotation and movement completed!");
}



    void RotateMars()
    {
        Debug.Log("Rotating Mars...");
        StartCoroutine(RotateMarsCoroutine());

        if (marsAudio != null)
        {
            marsAudio.Play();
        }
    }

    private IEnumerator RotateMarsCoroutine()
    {
        // Initialize elapsed time to track the rotation duration.
        float elapsedTime = 0f;

         // Loop until the elapsed time reaches the specified rotation duration.
        while (elapsedTime < durationRotate)
        {
            // Rotate the Mars GameObject around its Y-axis at a speed defined by rotationSpeed.
            mars.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            // Increment elapsed time by the time passed since the last frame.
            elapsedTime += Time.deltaTime;
            // Wait for the next frame before continuing.
            yield return null;
        }
        // Log a message indicating that Mars's rotation is complete.
        Debug.Log("Mars rotation completed!");
    }



    void RotateSaturnRing()
    {
        Debug.Log("Rotating Saturn's Ring...");
        StartCoroutine(RotateRingCoroutine());

        if (saturnInfo != null)
        {
            saturnInfo.Play();
        }
    }

     private IEnumerator RotateRingCoroutine()
    {
        // Initialize elapsed time to track the rotation duration.
        float elapsedTime = 0f;

        // Loop until the elapsed time reaches the specified rotation duration.
        while (elapsedTime < durationRotate)
        {
             // Rotate Saturn's ring around its parent object's position (Saturn) along the Y-axis.
            saturnRing.transform.RotateAround(saturnRing.transform.parent.position, Vector3.up, 100f * Time.deltaTime);
             // Increment elapsed time by the time passed since the last frame.
            elapsedTime += Time.deltaTime;
            // Wait for the next frame before continuing.
            yield return null;
        }

        // Log a message indicating that Saturn's ring rotation is complete.
        Debug.Log("Saturn's ring rotation completed!");
    }

    void OrbitMoon()
    {
    Debug.Log("Moon orbiting Earth...");
    StartCoroutine(OrbitMoonCoroutine());

    if (moonInfo != null)
        {
            moonInfo.Play();
        }
    }

    private IEnumerator OrbitMoonCoroutine()
    {
    //Define the total duration of the Moon's orbit.
    float duration = 15f; 
    // Initialize elapsed time to track how long the orbit has been ongoing.
    float elapsedTime = 0f;
    // Define the axis of orbit as the Y-axis (upward direction).
    Vector3 orbitAxis = Vector3.up; 
    
    // Continue orbiting the Moon around the Earth until the elapsed time reaches the specified duration.
    while (elapsedTime < duration)
    {
        // Rotate the Moon around the Earth's position along the specified orbit axis.
        moon.transform.RotateAround(earth.transform.position, orbitAxis, rotationSpeed * Time.deltaTime);

         // Increment elapsed time by the time passed since the last frame.
        elapsedTime += Time.deltaTime;
         // Wait for the next frame before continuing the loop.
        yield return null;
    }

     // Log a message indicating the Moon's orbit has completed.
    Debug.Log("Moon orbit completed.");
    } 

void AnimateNeptune()
{
    Debug.Log("Animating Neptune with an icy storm effect...");
    StartCoroutine(NeptuneIcyStormAnimation());

    if (neptuneAudio != null)
    {
        neptuneAudio.Play();
    }
}

private IEnumerator NeptuneIcyStormAnimation()
{
    // Get the Renderer component of Neptune to modify its material properties.
    Renderer neptuneRenderer = neptune.GetComponent<Renderer>();
    // Get the ParticleSystem component (child object) to simulate snow particles on Neptune.
    ParticleSystem snowParticles = neptune.GetComponentInChildren<ParticleSystem>();
    // Start the snow particle effect to simulate an icy storm.
    snowParticles.Play();

    float duration = 10f; 
    float elapsedTime = 0f;
    // Store the original scale of Neptune to restore it later.
    Vector3 originalScale = neptune.transform.localScale;

    while (elapsedTime < duration)
    {
        float scaleFactor = 1 + Mathf.Sin(elapsedTime * 2f) * 0.03f;
        neptune.transform.localScale = originalScale * scaleFactor;

        elapsedTime += Time.deltaTime;
        yield return null;
    }
    // Reset Neptune's scale to its original size after the animation.
    neptune.transform.localScale = originalScale; 
    // Stop the snow particle effect once the animation is complete.
    snowParticles.Stop();
    
    // Log a message indicating the icy storm animation has completed.
    Debug.Log("Neptune animation completed!");
}


    void AnimateUranus()
{
    Debug.Log("Animating Uranus with a tilt and spin...");
    StartCoroutine(UranusSpinAnimation());

    if (uranusAudio != null)
    {
        uranusAudio.Play();
    }
}


    private IEnumerator UranusSpinAnimation()
{
    // Total duration for Uranus animation.
    float duration = 10f;
    // Tracks how long the animation has been running.
    float elapsedTime = 0f;
    // Defines a tilt axis for Uranus's rotation, not aligned with a single axis.
    Vector3 uranusAxis = new Vector3(1, 1, 0.5f);

     // Loop until the total animation duration is reached.
    while (elapsedTime < duration)
    {
         // Rotate Uranus along the defined tilt axis at a speed of 30 degrees per second.
        uranus.transform.Rotate(uranusAxis, 30f * Time.deltaTime);
         // Increment elapsed time by the time passed since the last frame.
        elapsedTime += Time.deltaTime;
        // Wait until the next frame to continue the animation.
        yield return null;
    }
     // Log a message to indicate the completion of Uranus's tilt and spin animation.
    Debug.Log("Uranus animation completed!");
}

   
void AnimateJupiter()
{
    Debug.Log("Animating Jupiter with its storm swirling...");
    StartCoroutine(JupiterStormAnimation());

    if (jupiterAudio != null)
    {
        jupiterAudio.Play();
    }
}

    private IEnumerator JupiterStormAnimation()
{
    // Total duration for the Jupiter storm animation.
    float duration = 12f;
    // Tracks how long the animation has been running.
    float elapsedTime = 0f;

    // Loop until the animation duration is reached.
    while (elapsedTime < duration)
    {
        // Rotate Jupiter around its own Y-axis to simulate spinning.
        jupiter.transform.Rotate(Vector3.up, 20f * Time.deltaTime);
        // jupiter.transform.RotateAround(jupiter.transform.position, Vector3.forward, 10f * Time.deltaTime);
        // Increment elapsed time with the time passed since the last frame.
        elapsedTime += Time.deltaTime;
        // Wait for the next frame to continue the animation.
        yield return null;
    }
    // Log a message indicating the completion of the Jupiter storm animation.
    Debug.Log("Jupiter animation completed!");
}

void AnimateVenus()
{
    Debug.Log("Animating Venus with a dynamic orbit...");
    StartCoroutine(VenusDynamicOrbit());

    if (venusAudio != null)
    {
        venusAudio.Play();
    }
}

private IEnumerator VenusDynamicOrbit()
{
     // Log a message indicating the start of the Venus orbit animation.
    Debug.Log("Starting Venus dynamic orbit animation...");

    float duration = 12f; // Total duration
    float orbitRadius = 2f; // Orbit radius
    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
        // Orbit Venus around its original position
        float angle = elapsedTime * Mathf.PI * 2 / duration;
        venus.transform.position += new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * orbitRadius * Time.deltaTime;

        elapsedTime += Time.deltaTime;
        yield return null;
    }
    Debug.Log("Venus animation completed!");
}

    

    void AnimateMercury()
{
    Debug.Log("Animating Mercury with orbit sparks...");
    StartCoroutine(MercurySparkAnimation());

    if (mercuryAudio != null)
    {
        mercuryAudio.Play();
    }
}


    private IEnumerator MercurySparkAnimation()
{
    float duration = 6f;
    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
        // Calculate the spark size dynamically using PingPong for a pulsing effect.
        float sparkSize = Mathf.PingPong(elapsedTime * 2f, 0.5f) + 1f;
        // Update Mercury's scale to create the spark effect.
        mercury.transform.localScale = new Vector3(sparkSize, sparkSize, sparkSize);
        // Increment elapsed time based on the frame time.
        elapsedTime += Time.deltaTime;
        // Wait until the next frame to continue the animation.
        yield return null;
    }
     // Reset Mercury's scale to its original size after the animation.
    mercury.transform.localScale = Vector3.one; // Reset scale
    // Log a message indicating the animation is complete.
    Debug.Log("Mercury animation completed!");
}
    

    private IEnumerator SunGlowEffect()
    {
        // Log a message to indicate the Sun glow effect has started.
        Debug.Log("Starting Sun glow effect...");

        // Play the Sun's associated audio if it exists.
        if (sunAudio != null)
        {
            sunAudio.Play();
        }
        
         // Store the original scale of the Sun for resetting later.
        Vector3 originalScale = sun.transform.localScale;
        // Calculate the target scale for the Sun by multiplying the original scale by maxGlowScale.
        Vector3 targetScale = originalScale * maxGlowScale;

        // Phase 1: Grow the Sun
        float elapsedTime = 0f;
        while (elapsedTime < glowDuration / 2) // Half of the glow duration.
        {
            // Smoothly interpolate the Sun's scale from the original to the target scale.
            sun.transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsedTime / (glowDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;// Wait for the next frame.
        }
        // Set the Sun's scale to the exact target scale.
        sun.transform.localScale = targetScale;

        // Phase 2: Shrink the Sun back to its original size
        elapsedTime = 0f;
        while (elapsedTime < glowDuration / 2)
        {
            // Smoothly interpolate the Sun's scale from the target back to the original scale.
            sun.transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsedTime / (glowDuration / 2));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // Ensure the Sun's scale is fully reset to its original size.
        sun.transform.localScale = originalScale;
        // Log a message indicating the Sun glow effect has completed.
        Debug.Log("Sun glow effect completed.");
    }

    private IEnumerator LaunchRocketSequence()
    {
        // Set the rocket launch status to true.
        isRocketLaunching = true;
         // Log a message indicating the rocket launch sequence has started.
        Debug.Log("Rocket is launching...");

        // Play the rocket's audio if it exists.
        if (rocketAudio != null)
        {
            rocketAudio.Play();
        }

        // Initialize elapsed time for tracking the rocket's flight.
        float elapsedTime = 0f;
        // Store the rocket's initial position.
        Vector3 initialPosition = rocket.transform.position;
        // Calculate the target position for the rocket by moving it upwards.
        Vector3 targetPosition = initialPosition + Vector3.up * rocketSpeed * rocketDuration;

        // Animate the rocket's position over the duration of the launch.
        while (elapsedTime < rocketDuration)
        {
            // Smoothly transition the rocket's position from initial to target.
            rocket.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / rocketDuration);
            // Increment elapsed time based on the frame duration.
            elapsedTime += Time.deltaTime;

            // Adjust the camera's position to follow the rocket.
            mainCamera.transform.position = rocket.transform.position + rocketCameraOffset;
            // Make the camera look at the rocket.
            mainCamera.transform.LookAt(rocket.transform.position);

            // Wait for the next frame before continuing.
            yield return null;
        }

        // Log a message indicating the rocket launch is complete.
        Debug.Log("Rocket launch completed. Transitioning to solar system view.");

        // Transition to the solar system view after the rocket launch.
        yield return StartCoroutine(TransitionToSolarSystemView());
        isRocketLaunching = false;
    }

    private IEnumerator TransitionToSolarSystemView()
    {
        float duration = 2f;
        float elapsedTime = 0f;

        // Store the camera's starting position.
        Vector3 startPosition = mainCamera.transform.position;
        // Store the camera's starting rotation.
        Quaternion startRotation = mainCamera.transform.rotation;

        // Smoothly transition the camera's position and rotation over the specified duration.
        while (elapsedTime < duration)
        {
            // Interpolate the camera's position towards the solar system view position.
            mainCamera.transform.position = Vector3.Lerp(startPosition, solarSystemCameraPosition, elapsedTime / duration);
            // Interpolate the camera's rotation towards the solar system view rotation.
            mainCamera.transform.rotation = Quaternion.Lerp(startRotation, solarSystemCameraRotation, elapsedTime / duration);
            // Increment elapsed time by the time passed since the last frame.
            elapsedTime += Time.deltaTime;
            // Wait for the next frame before continuing.
            yield return null;
        }

        // Ensure the camera is exactly at the target position and rotation after the transition.
        mainCamera.transform.position = solarSystemCameraPosition;
        mainCamera.transform.rotation = solarSystemCameraRotation;

        // Log a message indicating the solar system view has been displayed.
        Debug.Log("Solar system view displayed.");
    }
}
