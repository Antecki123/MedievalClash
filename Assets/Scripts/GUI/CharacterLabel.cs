using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLabel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_name;
    [SerializeField] private Slider m_healthBar;

    private Transform m_cameraTransform;

    private float m_health;
    private float m_maxHealth;

    private void OnEnable()
    {
        m_cameraTransform = Camera.main.transform;
        m_name.text = name;

        m_healthBar.maxValue = m_maxHealth;
    }

    private void LateUpdate()
    {
        m_healthBar.value = m_health;
        transform.LookAt(m_cameraTransform.position);
        transform.Rotate(new Vector3(0f, 180f, 0f));
    }
}