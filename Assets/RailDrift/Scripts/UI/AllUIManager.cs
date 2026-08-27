using UnityEngine;
using UnityEngine.UIElements;

public class AllUIManager : MonoBehaviour
{
	[SerializeField] private UIDocument _allUI;
	private MenuUIManager _menuUIManager;
	private PlayerUIManager _playerUIManager;
	private VisualElement _menuUI;
	private VisualElement _playerUI;

	public void Initializing()
	{
		_menuUI = _allUI.rootVisualElement.Q<VisualElement>("MenuUI");
		_playerUI = _allUI.rootVisualElement.Q<VisualElement>("PlayerUI");

		_menuUIManager = new MenuUIManager(_menuUI);
		_playerUIManager = new PlayerUIManager(_playerUI);

		MainMenu();

		GameEvents.OnGameStart += StartGame;
		GameEvents.OnMainMenu += MainMenu;
	}

	private void StartGame()
	{
		_menuUI.style.display = DisplayStyle.None;
		_playerUI.style.display = DisplayStyle.Flex;
	}

	private void MainMenu()
	{
		_menuUI.style.display = DisplayStyle.Flex;
		_playerUI.style.display = DisplayStyle.None;
	}

	private void OnDestroy()
	{
		GameEvents.OnGameStart -= StartGame;
		GameEvents.OnMainMenu -= MainMenu;
	}
}
