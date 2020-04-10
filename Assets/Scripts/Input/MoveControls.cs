// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/MoveControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @MoveControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @MoveControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MoveControls"",
    ""maps"": [
        {
            ""name"": ""PointDetection"",
            ""id"": ""f65da41f-14b5-400f-87fd-d74b48121738"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""a3268a8e-8398-41e0-9a3e-fcc57b3e4546"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b7d0248a-697a-48c8-8aa8-719ea7577ec4"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""MouseScheme"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""MouseScheme"",
            ""bindingGroup"": ""MouseScheme"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PointDetection
        m_PointDetection = asset.FindActionMap("PointDetection", throwIfNotFound: true);
        m_PointDetection_Movement = m_PointDetection.FindAction("Movement", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // PointDetection
    private readonly InputActionMap m_PointDetection;
    private IPointDetectionActions m_PointDetectionActionsCallbackInterface;
    private readonly InputAction m_PointDetection_Movement;
    public struct PointDetectionActions
    {
        private @MoveControls m_Wrapper;
        public PointDetectionActions(@MoveControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PointDetection_Movement;
        public InputActionMap Get() { return m_Wrapper.m_PointDetection; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PointDetectionActions set) { return set.Get(); }
        public void SetCallbacks(IPointDetectionActions instance)
        {
            if (m_Wrapper.m_PointDetectionActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PointDetectionActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PointDetectionActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PointDetectionActionsCallbackInterface.OnMovement;
            }
            m_Wrapper.m_PointDetectionActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
            }
        }
    }
    public PointDetectionActions @PointDetection => new PointDetectionActions(this);
    private int m_MouseSchemeSchemeIndex = -1;
    public InputControlScheme MouseSchemeScheme
    {
        get
        {
            if (m_MouseSchemeSchemeIndex == -1) m_MouseSchemeSchemeIndex = asset.FindControlSchemeIndex("MouseScheme");
            return asset.controlSchemes[m_MouseSchemeSchemeIndex];
        }
    }
    public interface IPointDetectionActions
    {
        void OnMovement(InputAction.CallbackContext context);
    }
}
