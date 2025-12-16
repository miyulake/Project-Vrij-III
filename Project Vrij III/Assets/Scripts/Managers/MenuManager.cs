using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    [System.Serializable]
    public struct Menu
    {
        public string id;
        public GameObject root;
        public Selectable firstSelected;
    }

    public static MenuManager Instance;
    [SerializeField] private Menu[] m_Menus;
    private Dictionary<string, Menu> m_MenuLookup;
    private Menu m_CurrentMenu;

    private void Awake()
    {
        Instance = this;

        m_MenuLookup = new Dictionary<string, Menu>();
        foreach (var menu in m_Menus)
        {
            menu.root.SetActive(false);
            m_MenuLookup.Add(menu.id, menu);
        }

        ShowMenu("Main");
    }

    public void ShowMenu(string id)
    {
        if (!m_MenuLookup.TryGetValue(id, out var menu)) return;
        if (m_CurrentMenu.root != null) m_CurrentMenu.root.SetActive(false);

        m_CurrentMenu = menu;
        m_CurrentMenu.root.SetActive(true);

        SetFocus(menu.firstSelected);
    }

    private void SetFocus(Selectable selectable)
    {
        if (selectable == null) return;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(selectable.gameObject);
    }
}
