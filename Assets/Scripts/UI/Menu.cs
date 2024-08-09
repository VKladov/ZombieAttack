using TMPro;
using UnityEngine;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private TMP_Text _title;
        [SerializeField] private RectTransform _buttonsContainer;
        [SerializeField] private MenuButton _buttonPrefab;

        public void Open(string title, ButtonData[] buttons)
        {
            gameObject.SetActive(true);
            _title.text = title;
            ClearButtons();

            foreach (var button in buttons) AddButton(button);

            Time.timeScale = 0f;
        }

        public void Close()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1f;
        }

        private void AddButton(ButtonData data)
        {
            var button = Instantiate(_buttonPrefab, _buttonsContainer);
            button.Init(data);
        }

        private void ClearButtons()
        {
            foreach (Transform child in _buttonsContainer.transform) Destroy(child.gameObject);
        }
    }
}