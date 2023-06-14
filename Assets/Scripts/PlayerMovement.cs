using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DatabaseManager databaseManager;
    [SerializeField] private List<TextMeshProUGUI> highScoresTexts;
    [SerializeField] [CanBeNull] private TextMeshProUGUI currentScoreText;
    [SerializeField] private GameObject sparkParticle;
    
    [SerializeField] private float movementSpeedLimit = 1.0f;
    [SerializeField] private float acceleration = 1000.0f;
    [SerializeField] private float maxRotationSpeed = 180.0f;

    private Rigidbody _rigidbody;
    private float _currentScore = 0.0f;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.maxLinearVelocity = movementSpeedLimit;
        
        UpdateHighScores();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            bool isRotatingLeft = Input.mousePosition.x < Screen.width / 2.0f;
            var rotationSpeed = Mathf.Abs(Input.mousePosition.x / Screen.width - 0.5f) * 2 * maxRotationSpeed;
            Quaternion localRotationDirection = isRotatingLeft ? Quaternion.Euler(0, -90, 0) : Quaternion.Euler(0, 90, 0);
            
            Quaternion processedRotation = Quaternion.RotateTowards(_rigidbody.rotation,
                localRotationDirection * _rigidbody.rotation, Time.deltaTime * rotationSpeed);
            
            _rigidbody.MoveRotation(processedRotation);
            _rigidbody.AddRelativeForce(Vector3.forward*acceleration*Time.deltaTime);

        }
        _currentScore += _rigidbody.velocity.magnitude * Time.deltaTime;
        UpdateScore();
    }

    private void UpdateScore()
    {
        currentScoreText.text = ((int)_currentScore).ToString();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(sparkParticle, collision.contacts[0].point, Quaternion.identity);
        if (collision.transform.CompareTag("Obstacle"))
        {
            databaseManager.SaveHighScore((int)_currentScore);
            _currentScore = 0.0f;

            UpdateHighScores();
        }
    }

    private void UpdateHighScores()
    {
        List<int> highScores = databaseManager.LoadHighScores();
        highScores.Sort((a, b) => b.CompareTo(a));
        for (int i = 0; i < highScores.Count; i++)
        {
            highScoresTexts[i].text = highScores[i].ToString();
        }

        for (int i = highScores.Count; i < highScoresTexts.Count; i++)
        {
            highScoresTexts[i].text = "0";
        }
    }
}