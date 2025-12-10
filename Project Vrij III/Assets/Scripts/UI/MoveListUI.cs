using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveListUI : MonoBehaviour
{
    [System.Serializable]
    private struct Icon
    {
        public InputSequence.InputType type;
        public Texture controllerIcon;
        public Vector3 controllerIconScale;
        public Texture keyboardIcon;
        public Vector3 keyboardIconScale;
    }

    [SerializeField] private InputSequence[] m_Sequences;
    [SerializeField] private Icon[] m_Icons;
    private Dictionary<InputSequence.InputType, Texture> m_ControllerIconLookup;
    private Dictionary<InputSequence.InputType, Texture> m_KeyboardIconLookup;
    private Dictionary<InputSequence.InputType, Icon> m_IconLookup;
    private bool m_ControllerInputs = true;

    private void Awake()
    {
        CreateIconLookup();
        ApplyIcons();
    }

    private void CreateIconLookup()
    {
        m_ControllerIconLookup = new();
        m_KeyboardIconLookup = new();
        m_IconLookup = new Dictionary<InputSequence.InputType, Icon>();

        for (int i = 0; i < m_Icons.Length; i++)
        {
            var icon = m_Icons[i];
            m_ControllerIconLookup[icon.type] = icon.controllerIcon;
            m_KeyboardIconLookup[icon.type] = icon.keyboardIcon;
            m_IconLookup[icon.type] = icon;
        }
    }

    private void ApplyIcons()
    {
        for (int i = 0; i < m_Sequences.Length; i++) ApplySequence(m_Sequences[i]);
    }

    private void ApplySequence(InputSequence sequence)
    {
        var types = m_ControllerInputs ? sequence.controllerTypes : sequence.keyboardTypes;

        for (int i = 0; i < sequence.slots.Length; i++)
        {
            var image = sequence.slots[i];
            var type = (i < types.Length) ? types[i] : InputSequence.InputType.NONE;

            if (type != InputSequence.InputType.NONE) image.color = Color.white;
            else image.color = new Color(0, 0, 0, 0);

            Texture icon;
            if (m_ControllerInputs) m_ControllerIconLookup.TryGetValue(type, out icon);
            else m_KeyboardIconLookup.TryGetValue(type, out icon);

            if (m_IconLookup.TryGetValue(type, out var iconStruct))
            {
                image.transform.localScale = m_ControllerInputs
                    ? iconStruct.controllerIconScale
                    : iconStruct.keyboardIconScale;
            }

            if (image.texture != icon) image.texture = icon;
        }
    }

    public void ToggleInputSprites()
    {
        m_ControllerInputs = !m_ControllerInputs;
        ApplyIcons();
    }
}
