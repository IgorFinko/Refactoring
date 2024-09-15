using System;
using UnityEngine;
using UnityEngine.UI;

namespace Example4
{
    public interface ICheatElementFactory
    {
        CheatElementBehaviour CreateCheatElement(CheatActionDescription description, Transform parent);
    }

    public interface ICheatProvider
    {
        IEnumerable<CheatActionDescription> GetCheatActions();
    }

    public class CheatElementFactory : ICheatElementFactory
    {
        private readonly CheatElementBehaviour _cheatElementPrefab;

        public CheatElementFactory(CheatElementBehaviour cheatElementPrefab)
        {
            _cheatElementPrefab = cheatElementPrefab;
        }

        public CheatElementBehaviour CreateCheatElement(CheatActionDescription description, Transform parent)
        {
            var element = UnityEngine.Object.Instantiate(_cheatElementPrefab, parent);
            element.Setup(description);
            return element;
        }
    }

    public class CheatActionDescription
    {
        public readonly string name;
        public readonly Action cheatAction;

        public CheatActionDescription(string name, Action cheatAction)
        {
            this.name = name;
            this.cheatAction = cheatAction;
        }
    }

    public class CheatElementBehaviour : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Button _button;

        public void Setup(CheatActionDescription description)
        {
            _text.text = description.name;
            _button.onClick.AddListener(() => description.cheatAction());
        }
    }

    public class CheatManager
    {
        // Using the Singleton pattern can create problems when using multiple instances in different contexts.
        // Instead, I recommend using Dependency Injection to improve flexibility.
        // You can use VContainer or Zenject for this purpose.
        public static readonly CheatManager Instance = new CheatManager();
        private readonly List<ICheatProvider> _providers = new List<ICheatProvider>();
        private GameObject _panelPrefab;
        private GameObject _panel;
        private ICheatElementFactory _elementFactory;

        public void Setup(GameObject panelPrefab, ICheatElementFactory elementFactory)
        {
            _panelPrefab = panelPrefab;
            _elementFactory = elementFactory;
        }

        public void RegProvider(ICheatProvider provider)
        {
            _providers.Add(provider);
        }

        public void ShowCheatPanel()
        {
            if (_panel != null)
            {
                return;
            }

            _panel = UnityEngine.Object.Instantiate(_panelPrefab);
            CreateCheatElements();
        }

        public void HideCheatPanel()
        {
            if (_panel == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(_panel);
            _panel = null;
        }

        private void CreateCheatElements()
        {
            foreach (var provider in _providers)
            {
                foreach (var cheatAction in provider.GetCheatActions())
                {
                    _elementFactory.CreateCheatElement(cheatAction, _panel.transform);
                }
            }
        }
    }

    public class SomeManagerWithCheats : ICheatProvider
    {
        private int _health;

        public void Setup()
        {
            CheatManager.Instance.RegProvider(this);
        }

        public IEnumerable<CheatActionDescription> GetCheatActions()
        {
            yield return new CheatActionDescription("Cheat health", () => _health++);
            yield return new CheatActionDescription("Reset health", () => _health = 0);
        }
    }
}
