
using UnityEngine;
using UnityEngine.UI;


public class ButtonDirection : MonoBehaviour
{
    private Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Vector2 direction;

    private void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        GameManager.Instance.SetPlayerDirection(direction);
    }
}
