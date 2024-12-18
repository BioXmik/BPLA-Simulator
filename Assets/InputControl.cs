//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/InputControl.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputControl : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputControl"",
    ""maps"": [
        {
            ""name"": ""Drone"",
            ""id"": ""7fe4581f-36fb-44c3-b6fe-8127554ee71d"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""bc359390-562d-4ae6-8a69-4c35eb25d337"",
                    ""expectedControlType"": ""Vector3"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ForwardBackward"",
                    ""type"": ""Value"",
                    ""id"": ""4824453d-3a1e-43dd-8343-98e29a2d7ba3"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""LeftRight"",
                    ""type"": ""Value"",
                    ""id"": ""8f97af93-61ca-461f-963a-4eb33756162e"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""DropCargo"",
                    ""type"": ""Button"",
                    ""id"": ""80eb92c2-96da-4a7a-8ef4-bc89d431d45d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EvacuationCargo"",
                    ""type"": ""Button"",
                    ""id"": ""4e29c718-c73a-45e2-8f99-a0c399d030b4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EditCam"",
                    ""type"": ""Button"",
                    ""id"": ""37089495-453a-42ce-b536-2899153362c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""KillSwitch"",
                    ""type"": ""Button"",
                    ""id"": ""76086777-72d9-4cb3-bac2-3bda725af55b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""8185c9d8-6d36-4eba-a857-c21c4612cec6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EditMode"",
                    ""type"": ""Button"",
                    ""id"": ""35e0aa35-bea3-479c-968f-3ff2ac8fecfb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""slider1"",
                    ""type"": ""Value"",
                    ""id"": ""cb5f65f9-b8f9-4b96-8872-2797b40aaa96"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""slider2"",
                    ""type"": ""Value"",
                    ""id"": ""5bae131d-7aca-4a7b-8e07-f68cbb21a899"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e069a191-d221-4c97-90fc-adebc8212c3c"",
                    ""path"": ""<HID::OpenTX FrSky Taranis Joystick>/button3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DropCargo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""92cd6205-a4e1-40a8-bcaf-dc3f97ac26b9"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DropCargo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4b0862e9-64fb-4dbb-8860-ecf67d5c38c6"",
                    ""path"": ""<HID::OpenTX FrSky Taranis Joystick>/button5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EvacuationCargo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9ad01c21-7aad-4fbc-8081-dee5607c79d7"",
                    ""path"": ""<Keyboard>/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EvacuationCargo"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d4491be2-d8c1-4983-9d51-b953914d745a"",
                    ""path"": ""<HID::OpenTX FrSky Taranis Joystick>/button4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EditCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b2d593a8-17ed-4c4a-9b2f-6ed28d7f9fa0"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EditCam"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0aeec27a-5985-4bab-ae12-4b33ea1cd838"",
                    ""path"": ""<HID::OpenTX FrSky Taranis Joystick>/button2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KillSwitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""319400a5-8724-4721-b054-e0eb91b40e74"",
                    ""path"": ""<Keyboard>/u"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""KillSwitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9bcfa401-b66d-46d7-9869-19255d48151d"",
                    ""path"": ""<HID::OpenTX FrSky Taranis Joystick>/button7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""315f6132-5558-4dd1-a0a4-63839a8b9e41"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c9a58e6-b8ea-4836-b3d1-31302e9f74c8"",
                    ""path"": ""<HID::OpenTX FrSky Taranis Joystick>/button6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EditMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""26536f67-6a2d-4ef0-b3ac-c1d08e2eba1e"",
                    ""path"": ""<Keyboard>/o"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EditMode"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""92b6279a-166a-427b-a629-eba240d11d3a"",
                    ""path"": ""<HID::OpenTX FrSky Taranis Joystick>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ForwardBackward"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""3D Vector"",
                    ""id"": ""0c011195-1375-4025-b8b2-3fbf689e520c"",
                    ""path"": ""3DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""down"",
                    ""id"": ""46fbbb15-e34d-462c-9d6d-ecd622e316b5"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""5cac5b3a-7d4f-49af-b00d-0699c0c22de2"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""74d0f340-fa38-46d4-9953-a9227bad9ed6"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""b6c727ec-7955-49ee-a336-4616b7907c1f"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""6b07e9a7-a279-4025-825a-4898bc7e0e60"",
                    ""path"": ""<HID::OpenTX FrSky Taranis Joystick>/rx"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""79b47a2f-52fc-4b6a-83d6-84b8953aa945"",
                    ""path"": ""<HID::OpenTX FrSky Taranis Joystick>/ry"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""slider1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""56e366b8-604c-49cd-9c43-dec2de0cb53d"",
                    ""path"": ""<HID::OpenTX FrSky Taranis Joystick>/button8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""slider2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Drone
        m_Drone = asset.FindActionMap("Drone", throwIfNotFound: true);
        m_Drone_Move = m_Drone.FindAction("Move", throwIfNotFound: true);
        m_Drone_ForwardBackward = m_Drone.FindAction("ForwardBackward", throwIfNotFound: true);
        m_Drone_LeftRight = m_Drone.FindAction("LeftRight", throwIfNotFound: true);
        m_Drone_DropCargo = m_Drone.FindAction("DropCargo", throwIfNotFound: true);
        m_Drone_EvacuationCargo = m_Drone.FindAction("EvacuationCargo", throwIfNotFound: true);
        m_Drone_EditCam = m_Drone.FindAction("EditCam", throwIfNotFound: true);
        m_Drone_KillSwitch = m_Drone.FindAction("KillSwitch", throwIfNotFound: true);
        m_Drone_Menu = m_Drone.FindAction("Menu", throwIfNotFound: true);
        m_Drone_EditMode = m_Drone.FindAction("EditMode", throwIfNotFound: true);
        m_Drone_slider1 = m_Drone.FindAction("slider1", throwIfNotFound: true);
        m_Drone_slider2 = m_Drone.FindAction("slider2", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Drone
    private readonly InputActionMap m_Drone;
    private IDroneActions m_DroneActionsCallbackInterface;
    private readonly InputAction m_Drone_Move;
    private readonly InputAction m_Drone_ForwardBackward;
    private readonly InputAction m_Drone_LeftRight;
    private readonly InputAction m_Drone_DropCargo;
    private readonly InputAction m_Drone_EvacuationCargo;
    private readonly InputAction m_Drone_EditCam;
    private readonly InputAction m_Drone_KillSwitch;
    private readonly InputAction m_Drone_Menu;
    private readonly InputAction m_Drone_EditMode;
    private readonly InputAction m_Drone_slider1;
    private readonly InputAction m_Drone_slider2;
    public struct DroneActions
    {
        private @InputControl m_Wrapper;
        public DroneActions(@InputControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Drone_Move;
        public InputAction @ForwardBackward => m_Wrapper.m_Drone_ForwardBackward;
        public InputAction @LeftRight => m_Wrapper.m_Drone_LeftRight;
        public InputAction @DropCargo => m_Wrapper.m_Drone_DropCargo;
        public InputAction @EvacuationCargo => m_Wrapper.m_Drone_EvacuationCargo;
        public InputAction @EditCam => m_Wrapper.m_Drone_EditCam;
        public InputAction @KillSwitch => m_Wrapper.m_Drone_KillSwitch;
        public InputAction @Menu => m_Wrapper.m_Drone_Menu;
        public InputAction @EditMode => m_Wrapper.m_Drone_EditMode;
        public InputAction @slider1 => m_Wrapper.m_Drone_slider1;
        public InputAction @slider2 => m_Wrapper.m_Drone_slider2;
        public InputActionMap Get() { return m_Wrapper.m_Drone; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DroneActions set) { return set.Get(); }
        public void SetCallbacks(IDroneActions instance)
        {
            if (m_Wrapper.m_DroneActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnMove;
                @ForwardBackward.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnForwardBackward;
                @ForwardBackward.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnForwardBackward;
                @ForwardBackward.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnForwardBackward;
                @LeftRight.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnLeftRight;
                @LeftRight.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnLeftRight;
                @LeftRight.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnLeftRight;
                @DropCargo.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnDropCargo;
                @DropCargo.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnDropCargo;
                @DropCargo.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnDropCargo;
                @EvacuationCargo.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnEvacuationCargo;
                @EvacuationCargo.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnEvacuationCargo;
                @EvacuationCargo.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnEvacuationCargo;
                @EditCam.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnEditCam;
                @EditCam.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnEditCam;
                @EditCam.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnEditCam;
                @KillSwitch.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnKillSwitch;
                @KillSwitch.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnKillSwitch;
                @KillSwitch.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnKillSwitch;
                @Menu.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnMenu;
                @Menu.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnMenu;
                @Menu.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnMenu;
                @EditMode.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnEditMode;
                @EditMode.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnEditMode;
                @EditMode.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnEditMode;
                @slider1.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnSlider1;
                @slider1.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnSlider1;
                @slider1.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnSlider1;
                @slider2.started -= m_Wrapper.m_DroneActionsCallbackInterface.OnSlider2;
                @slider2.performed -= m_Wrapper.m_DroneActionsCallbackInterface.OnSlider2;
                @slider2.canceled -= m_Wrapper.m_DroneActionsCallbackInterface.OnSlider2;
            }
            m_Wrapper.m_DroneActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @ForwardBackward.started += instance.OnForwardBackward;
                @ForwardBackward.performed += instance.OnForwardBackward;
                @ForwardBackward.canceled += instance.OnForwardBackward;
                @LeftRight.started += instance.OnLeftRight;
                @LeftRight.performed += instance.OnLeftRight;
                @LeftRight.canceled += instance.OnLeftRight;
                @DropCargo.started += instance.OnDropCargo;
                @DropCargo.performed += instance.OnDropCargo;
                @DropCargo.canceled += instance.OnDropCargo;
                @EvacuationCargo.started += instance.OnEvacuationCargo;
                @EvacuationCargo.performed += instance.OnEvacuationCargo;
                @EvacuationCargo.canceled += instance.OnEvacuationCargo;
                @EditCam.started += instance.OnEditCam;
                @EditCam.performed += instance.OnEditCam;
                @EditCam.canceled += instance.OnEditCam;
                @KillSwitch.started += instance.OnKillSwitch;
                @KillSwitch.performed += instance.OnKillSwitch;
                @KillSwitch.canceled += instance.OnKillSwitch;
                @Menu.started += instance.OnMenu;
                @Menu.performed += instance.OnMenu;
                @Menu.canceled += instance.OnMenu;
                @EditMode.started += instance.OnEditMode;
                @EditMode.performed += instance.OnEditMode;
                @EditMode.canceled += instance.OnEditMode;
                @slider1.started += instance.OnSlider1;
                @slider1.performed += instance.OnSlider1;
                @slider1.canceled += instance.OnSlider1;
                @slider2.started += instance.OnSlider2;
                @slider2.performed += instance.OnSlider2;
                @slider2.canceled += instance.OnSlider2;
            }
        }
    }
    public DroneActions @Drone => new DroneActions(this);
    public interface IDroneActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnForwardBackward(InputAction.CallbackContext context);
        void OnLeftRight(InputAction.CallbackContext context);
        void OnDropCargo(InputAction.CallbackContext context);
        void OnEvacuationCargo(InputAction.CallbackContext context);
        void OnEditCam(InputAction.CallbackContext context);
        void OnKillSwitch(InputAction.CallbackContext context);
        void OnMenu(InputAction.CallbackContext context);
        void OnEditMode(InputAction.CallbackContext context);
        void OnSlider1(InputAction.CallbackContext context);
        void OnSlider2(InputAction.CallbackContext context);
    }
}
