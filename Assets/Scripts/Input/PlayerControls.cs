// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""2d5a58c0-bc50-45ec-a3ea-885c9c3bd10c"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""f3a4287c-1af2-4795-a3ea-f65c549854a1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""6e2cdeeb-f867-4d2d-a60a-2259b4785752"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""b4906629-b8ea-4a00-835c-34e0a749f2e0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""dee687e2-e17d-40ca-9780-fb9ce4727185"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""9e2ae31d-a299-40ae-b40e-c494ed917216"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""d8b7c05d-8b03-4156-bc2f-7b2f433a9465"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6c3b7605-3450-49ed-84b3-bb6e223f3bfa"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a9d9d49a-1ec8-40c3-a84b-a554b8ce6cf4"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""719984fe-b10e-4b8e-bc45-fd6a195cf623"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0f209809-5656-450d-835d-bbd9929d5947"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""75a435fb-f5ba-42f9-a2cb-ce222ddf1820"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c7d6557e-ed4d-4211-ace4-e11c8d0447f4"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""LockedDoorUI"",
            ""id"": ""6527b49d-c232-4401-bed3-e3fb63b9da31"",
            ""actions"": [
                {
                    ""name"": ""Zero"",
                    ""type"": ""Button"",
                    ""id"": ""e3e17287-1b32-410f-b77f-7eed27353eea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""One"",
                    ""type"": ""Button"",
                    ""id"": ""c714eab1-233c-40cd-8d9c-31d39073316b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Two"",
                    ""type"": ""Button"",
                    ""id"": ""0a870352-3ddf-468d-bc5b-801387e7ca31"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Three"",
                    ""type"": ""Button"",
                    ""id"": ""4b2b8609-dc5c-4905-bde8-95ab53e7f988"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Four"",
                    ""type"": ""Button"",
                    ""id"": ""ad252654-f717-4322-8be8-5db54503478b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Five"",
                    ""type"": ""Button"",
                    ""id"": ""2f0ba53a-0054-4dfd-8dce-5ee39919e77c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Six"",
                    ""type"": ""Button"",
                    ""id"": ""cadc2fe4-178e-47ca-bdda-d0dea8f347ba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Seven"",
                    ""type"": ""Button"",
                    ""id"": ""60a3e43b-21c5-4e4a-aade-f89d1a393d09"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Eight"",
                    ""type"": ""Button"",
                    ""id"": ""260a637d-69ff-489f-bb2d-174738848172"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Nine"",
                    ""type"": ""Button"",
                    ""id"": ""53a2031c-8b30-4da9-851c-9d6729b70d7f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Enter"",
                    ""type"": ""Button"",
                    ""id"": ""7c887b49-24af-448d-b761-a22ce60aaacf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Exit"",
                    ""type"": ""Button"",
                    ""id"": ""0fc26fa2-35e9-49ed-b4b6-3ab536c001bf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Delete"",
                    ""type"": ""Button"",
                    ""id"": ""19cf4273-9c45-4865-9bf5-9e2220d8fd11"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d6e57326-8538-4b89-ad02-102a4844fdde"",
                    ""path"": ""<Keyboard>/numpad0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zero"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1dc47e2c-02fe-45ff-ad1e-f51fbee1d481"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zero"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""34474c68-87be-488c-9688-303f93f5f48b"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""One"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""74f4f8ee-fb42-48fc-a7af-84201ddd5bd6"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""One"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c720a850-7f7f-46c4-888a-3f6a39c5331c"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Two"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e6daf69-1fc8-4bb1-92ce-4e3348c1c5c6"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Two"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45a99d17-1bc2-40fa-8a77-c220c1ff7f02"",
                    ""path"": ""<Keyboard>/numpad3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Three"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e754b3fe-d625-4b80-bef7-95f0f285cfab"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Three"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6e18e32-acf4-4019-b6e4-9511c827f462"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Four"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""889fbeb6-1373-4125-a844-5ac082c2437a"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Four"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f98342c0-947d-4b92-bc6b-1c357966d373"",
                    ""path"": ""<Keyboard>/numpad5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Five"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33a73099-1bce-4a37-9c48-8bdacaa758f7"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Five"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7e760657-e616-4439-ab98-f147d2ca515f"",
                    ""path"": ""<Keyboard>/numpad6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Six"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""84ff8afe-3215-4334-86c0-de92f4d00b8d"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Six"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef165474-98fd-4256-a571-1433a6aca385"",
                    ""path"": ""<Keyboard>/numpad7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Seven"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""47dcd04b-24db-4211-ac02-0b747add39a2"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Seven"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e7369c1c-4a0a-4c95-9c52-7d6ce2530804"",
                    ""path"": ""<Keyboard>/numpad8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Eight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""846ff822-5ab3-4a6c-bf76-abc897a0a8e2"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Eight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e2fd942-dcaa-4a58-8777-64acda0d2f83"",
                    ""path"": ""<Keyboard>/numpad9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Nine"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5bb0b9fc-7d4f-44e3-8b85-428b956fd087"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Nine"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c652bed9-c3b9-498b-bd54-c9880b3cf99a"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""86686f20-9c52-4490-a352-12144f56bfed"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0ce3b9d3-8422-40e9-9adb-fce6fb06bf49"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c76ea3b7-446e-4506-94d9-d5a0d39fc1f8"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""695bca70-740d-4c38-b616-6b3ddb378dc4"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9231a342-daac-40fc-8c88-9117ce121ed5"",
                    ""path"": ""<Keyboard>/delete"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""IslemYapUI"",
            ""id"": ""40df712c-b71b-4366-bbbe-7d2e421af636"",
            ""actions"": [
                {
                    ""name"": ""Zero"",
                    ""type"": ""Button"",
                    ""id"": ""fe93a0fb-3682-4b9f-94a1-62ed90ce2dba"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""One"",
                    ""type"": ""Button"",
                    ""id"": ""9da7a8d7-2ce9-4c2e-9a8a-16508871687e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Two"",
                    ""type"": ""Button"",
                    ""id"": ""f076e199-5df0-4cc9-abcc-b01e3c649fd4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Three"",
                    ""type"": ""Button"",
                    ""id"": ""cc17df0e-f8e0-4dee-9c8a-fac1a247017b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Four"",
                    ""type"": ""Button"",
                    ""id"": ""ffb8c08f-a966-4c71-b9a5-f67f23b7a17d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Five"",
                    ""type"": ""Button"",
                    ""id"": ""cc2bce5c-5b60-44a2-8baa-e2652875421e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Six"",
                    ""type"": ""Button"",
                    ""id"": ""129e7d86-b3f0-4288-92f9-e6590bfd41a7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Seven"",
                    ""type"": ""Button"",
                    ""id"": ""8ba6fdfb-7520-4f5c-9e66-a1e23d201706"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Eight"",
                    ""type"": ""Button"",
                    ""id"": ""2591fbc2-2523-4606-a636-d477927a90fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Nine"",
                    ""type"": ""Button"",
                    ""id"": ""3d434825-c4dc-4db8-b71a-402d98a6c668"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Enter"",
                    ""type"": ""Button"",
                    ""id"": ""8ec39e2c-6b64-4141-83c6-8412e82bd50f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Exit"",
                    ""type"": ""Button"",
                    ""id"": ""9f05842e-aa03-4554-acee-9b0242d8c4e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Delete"",
                    ""type"": ""Button"",
                    ""id"": ""a638536b-d5d1-4105-8bc2-6ddafce900ab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Minus"",
                    ""type"": ""Button"",
                    ""id"": ""04f82142-719f-4f60-b095-79f277ac895c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Plus"",
                    ""type"": ""Button"",
                    ""id"": ""6e8f6113-74ef-40f4-a1d9-581d5ea1c5a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ae405417-e06a-4921-9e74-517ecd5bded4"",
                    ""path"": ""<Keyboard>/numpad0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zero"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99d62043-995c-4c3b-bbd1-bf3af3b2c57c"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zero"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c85b085e-170c-4365-944e-5b53cff203ac"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""One"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba45b0e5-7f5e-41d7-bdab-b27c23d5e7f9"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""One"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bfa2e1dd-b65f-4d7c-b473-f033f579e2b2"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Two"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b1713ea2-3ba7-4c6a-918a-0d22e8d489ff"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Two"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c285f342-f9af-47c4-9bb3-2185587ac4ac"",
                    ""path"": ""<Keyboard>/numpad3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Three"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""320ea47f-d0d8-4690-ae94-2fe4f07d567b"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Three"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5d244828-9134-449c-8fa0-ee86d577f8b1"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Four"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3259a1ef-8a2e-4587-92c0-60dbb4a0cddc"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Four"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9920070d-a784-4d78-884f-d8eb87d59d6c"",
                    ""path"": ""<Keyboard>/numpad5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Five"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8809a07f-b89f-498b-bcbd-4271a17d2d7d"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Five"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""370a8c32-0ce3-4973-8614-6857ca4dc653"",
                    ""path"": ""<Keyboard>/numpad6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Six"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f74d6550-0e89-41d1-8efa-b6ca128c472f"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Six"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""62ff38b3-0200-47fa-bb45-bc396c1c4dad"",
                    ""path"": ""<Keyboard>/numpad7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Seven"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""dcf2591a-6ff3-41df-bc3d-e14328d71595"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Seven"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0beb56bb-668a-4f0e-b352-d55e298926e6"",
                    ""path"": ""<Keyboard>/numpad8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Eight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a32f06f0-f52f-44ba-8f48-d9728f7ac220"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Eight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""11a3ce06-551d-4962-a22d-9939da4c6224"",
                    ""path"": ""<Keyboard>/numpad9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Nine"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ee711cc-ded9-46da-bb81-e08d3b907b91"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Nine"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95a3b444-e9f2-47f1-9845-115755b61cfe"",
                    ""path"": ""<Keyboard>/numpadPlus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Plus"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fcffbe69-c70c-411c-aaaa-cabfb07c4e34"",
                    ""path"": ""<Keyboard>/numpadMinus"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Minus"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c924283-8b2f-4bf6-b610-3d3934c9f5c3"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e54cf83-f7ab-428b-824c-474e5cc1c51c"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5db57bbc-4ee0-4dee-82ba-1a126d37d681"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""50be9ef4-6d52-4a14-ab5e-d35c1abf7622"",
                    ""path"": ""<Keyboard>/delete"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""385b2556-b19c-4318-a537-d17ded224cb5"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f410575d-bf1a-4602-903b-2e197e75182b"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""SayiAlUI"",
            ""id"": ""cd9b1a17-a397-4ea9-9c18-8b81295b15d7"",
            ""actions"": [
                {
                    ""name"": ""Delete"",
                    ""type"": ""Button"",
                    ""id"": ""3556edb0-4b9e-48e6-b5b2-122b3fca2cf0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Exit"",
                    ""type"": ""Button"",
                    ""id"": ""8014c589-9880-474f-bf27-09d4c4b25da4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Enter"",
                    ""type"": ""Button"",
                    ""id"": ""8081f0a6-be24-4254-a1a6-3def9f35f532"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Nine"",
                    ""type"": ""Button"",
                    ""id"": ""f806b5c3-3a9a-4c29-b5ce-a31fad535f90"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Eight"",
                    ""type"": ""Button"",
                    ""id"": ""ba033d84-07f6-4a2a-b96c-91fc21e814dd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Seven"",
                    ""type"": ""Button"",
                    ""id"": ""8bd82fa3-28da-4688-b04a-882cbabe5f37"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Six"",
                    ""type"": ""Button"",
                    ""id"": ""332ee37e-c95c-4278-beb7-9f059d79a7a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Five"",
                    ""type"": ""Button"",
                    ""id"": ""14f2a171-f445-41e4-b914-821079f9d897"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Four"",
                    ""type"": ""Button"",
                    ""id"": ""9b919e5b-7796-4710-aae5-e0e63b80a9c1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Three"",
                    ""type"": ""Button"",
                    ""id"": ""f52d240a-448f-4970-865e-e5cf9794816f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Two"",
                    ""type"": ""Button"",
                    ""id"": ""79ba5efd-a856-4208-ab4a-b5d25bbb67b9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""One"",
                    ""type"": ""Button"",
                    ""id"": ""65ede7eb-b27b-4137-bfa9-0028d19576f6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Zero"",
                    ""type"": ""Button"",
                    ""id"": ""575d0809-dd42-4c81-a6c4-e7be850d8daa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""GenerateQuestion"",
                    ""type"": ""Button"",
                    ""id"": ""4e8d3c28-9fff-40a0-a063-8d26118fe4ee"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2e413b17-fc4b-4061-9769-cbe3ca421221"",
                    ""path"": ""<Keyboard>/numpad0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zero"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c50e1bf-3035-442f-b924-fa867c48d944"",
                    ""path"": ""<Keyboard>/0"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zero"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""30fc4095-8694-47b0-b029-6b759ef20103"",
                    ""path"": ""<Keyboard>/numpad1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""One"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eff5c6d4-2f42-4a76-9cfe-a6095a82b831"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""One"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""33fac859-4979-431c-87f9-fc464811822c"",
                    ""path"": ""<Keyboard>/numpad2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Two"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""35601056-cce5-4df4-bc02-6ba33f703594"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Two"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0144cd02-6498-43d8-a0a0-8223fc5395ec"",
                    ""path"": ""<Keyboard>/numpad3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Three"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9246811e-afb4-4eef-bf3c-f9ec5dcefbf6"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Three"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""41dfb497-a7ef-41ba-8252-7c337ea41555"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Four"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e344ac49-165c-4f3d-a127-281b43fc3816"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Four"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e503b8d3-89c2-4405-9f49-641eb582a06b"",
                    ""path"": ""<Keyboard>/numpad5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Five"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0fa1606f-ffdd-493a-8eb5-b2c7239c08d5"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Five"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e2e10403-2bcd-4396-bcd1-d70a60dc2114"",
                    ""path"": ""<Keyboard>/numpad6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Six"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd614732-1d64-42c3-bfe7-5592b35d1d6c"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Six"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ab16ddd2-0499-4eb3-84f8-e0f94a0f0d0a"",
                    ""path"": ""<Keyboard>/numpad7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Seven"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""544c187a-e771-4b62-b179-dfb37586d469"",
                    ""path"": ""<Keyboard>/7"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Seven"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""436a980c-f655-4269-ab11-7e36a9260316"",
                    ""path"": ""<Keyboard>/numpad8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Eight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af537b82-f0ac-4446-bd58-57eb4172eb7a"",
                    ""path"": ""<Keyboard>/8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Eight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7a5ec75-a05b-4841-8d53-1ada5df55d79"",
                    ""path"": ""<Keyboard>/numpad9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Nine"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""004abdd1-b0a3-415f-ba5d-4368963861ce"",
                    ""path"": ""<Keyboard>/9"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Nine"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e8d478b-6100-41d1-874c-6876715a3130"",
                    ""path"": ""<Keyboard>/numpadEnter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""722faf51-5d3b-4353-a0f4-731b18e69294"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eecd42c4-4e82-4380-b8e7-370dd49807ec"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1246d991-e7c3-4c7c-8f11-e1b2608336ea"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bc4ab86e-b94f-4fe8-b3d8-13630f63c4fc"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""09b0f886-e003-45ef-bc79-1a4241179f8d"",
                    ""path"": ""<Keyboard>/delete"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delete"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c0e9a2d6-1ca5-4b96-9dd0-994b642ad727"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""GenerateQuestion"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""GameManager"",
            ""id"": ""9620676d-d302-4f55-8be9-8e26311cf6df"",
            ""actions"": [
                {
                    ""name"": ""Escape"",
                    ""type"": ""Button"",
                    ""id"": ""9ad47a22-ecc6-4d74-a3de-a1d53864687d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ReloadScene"",
                    ""type"": ""Button"",
                    ""id"": ""dc9ab595-7d80-45ba-8659-f99947d90085"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""260be348-d009-4cfb-9fc5-a657202b80e4"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Escape"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""943d12e3-cdfa-45fc-834e-2b5df7806059"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ReloadScene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""BlackBoardUIManagement"",
            ""id"": ""cd10dadb-8b4e-4001-a9a6-fa8dccad48d9"",
            ""actions"": [
                {
                    ""name"": ""Exit"",
                    ""type"": ""Button"",
                    ""id"": ""abbb7b15-d993-47d2-a5d7-46e632cdd813"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3105fb28-a96f-4580-82fd-8661a2589927"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13b28c18-04f7-4817-ab3e-2fc77c883318"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Exit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Move = m_Gameplay.FindAction("Move", throwIfNotFound: true);
        m_Gameplay_Jump = m_Gameplay.FindAction("Jump", throwIfNotFound: true);
        m_Gameplay_Run = m_Gameplay.FindAction("Run", throwIfNotFound: true);
        m_Gameplay_Interact = m_Gameplay.FindAction("Interact", throwIfNotFound: true);
        // LockedDoorUI
        m_LockedDoorUI = asset.FindActionMap("LockedDoorUI", throwIfNotFound: true);
        m_LockedDoorUI_Zero = m_LockedDoorUI.FindAction("Zero", throwIfNotFound: true);
        m_LockedDoorUI_One = m_LockedDoorUI.FindAction("One", throwIfNotFound: true);
        m_LockedDoorUI_Two = m_LockedDoorUI.FindAction("Two", throwIfNotFound: true);
        m_LockedDoorUI_Three = m_LockedDoorUI.FindAction("Three", throwIfNotFound: true);
        m_LockedDoorUI_Four = m_LockedDoorUI.FindAction("Four", throwIfNotFound: true);
        m_LockedDoorUI_Five = m_LockedDoorUI.FindAction("Five", throwIfNotFound: true);
        m_LockedDoorUI_Six = m_LockedDoorUI.FindAction("Six", throwIfNotFound: true);
        m_LockedDoorUI_Seven = m_LockedDoorUI.FindAction("Seven", throwIfNotFound: true);
        m_LockedDoorUI_Eight = m_LockedDoorUI.FindAction("Eight", throwIfNotFound: true);
        m_LockedDoorUI_Nine = m_LockedDoorUI.FindAction("Nine", throwIfNotFound: true);
        m_LockedDoorUI_Enter = m_LockedDoorUI.FindAction("Enter", throwIfNotFound: true);
        m_LockedDoorUI_Exit = m_LockedDoorUI.FindAction("Exit", throwIfNotFound: true);
        m_LockedDoorUI_Delete = m_LockedDoorUI.FindAction("Delete", throwIfNotFound: true);
        // IslemYapUI
        m_IslemYapUI = asset.FindActionMap("IslemYapUI", throwIfNotFound: true);
        m_IslemYapUI_Zero = m_IslemYapUI.FindAction("Zero", throwIfNotFound: true);
        m_IslemYapUI_One = m_IslemYapUI.FindAction("One", throwIfNotFound: true);
        m_IslemYapUI_Two = m_IslemYapUI.FindAction("Two", throwIfNotFound: true);
        m_IslemYapUI_Three = m_IslemYapUI.FindAction("Three", throwIfNotFound: true);
        m_IslemYapUI_Four = m_IslemYapUI.FindAction("Four", throwIfNotFound: true);
        m_IslemYapUI_Five = m_IslemYapUI.FindAction("Five", throwIfNotFound: true);
        m_IslemYapUI_Six = m_IslemYapUI.FindAction("Six", throwIfNotFound: true);
        m_IslemYapUI_Seven = m_IslemYapUI.FindAction("Seven", throwIfNotFound: true);
        m_IslemYapUI_Eight = m_IslemYapUI.FindAction("Eight", throwIfNotFound: true);
        m_IslemYapUI_Nine = m_IslemYapUI.FindAction("Nine", throwIfNotFound: true);
        m_IslemYapUI_Enter = m_IslemYapUI.FindAction("Enter", throwIfNotFound: true);
        m_IslemYapUI_Exit = m_IslemYapUI.FindAction("Exit", throwIfNotFound: true);
        m_IslemYapUI_Delete = m_IslemYapUI.FindAction("Delete", throwIfNotFound: true);
        m_IslemYapUI_Minus = m_IslemYapUI.FindAction("Minus", throwIfNotFound: true);
        m_IslemYapUI_Plus = m_IslemYapUI.FindAction("Plus", throwIfNotFound: true);
        // SayiAlUI
        m_SayiAlUI = asset.FindActionMap("SayiAlUI", throwIfNotFound: true);
        m_SayiAlUI_Delete = m_SayiAlUI.FindAction("Delete", throwIfNotFound: true);
        m_SayiAlUI_Exit = m_SayiAlUI.FindAction("Exit", throwIfNotFound: true);
        m_SayiAlUI_Enter = m_SayiAlUI.FindAction("Enter", throwIfNotFound: true);
        m_SayiAlUI_Nine = m_SayiAlUI.FindAction("Nine", throwIfNotFound: true);
        m_SayiAlUI_Eight = m_SayiAlUI.FindAction("Eight", throwIfNotFound: true);
        m_SayiAlUI_Seven = m_SayiAlUI.FindAction("Seven", throwIfNotFound: true);
        m_SayiAlUI_Six = m_SayiAlUI.FindAction("Six", throwIfNotFound: true);
        m_SayiAlUI_Five = m_SayiAlUI.FindAction("Five", throwIfNotFound: true);
        m_SayiAlUI_Four = m_SayiAlUI.FindAction("Four", throwIfNotFound: true);
        m_SayiAlUI_Three = m_SayiAlUI.FindAction("Three", throwIfNotFound: true);
        m_SayiAlUI_Two = m_SayiAlUI.FindAction("Two", throwIfNotFound: true);
        m_SayiAlUI_One = m_SayiAlUI.FindAction("One", throwIfNotFound: true);
        m_SayiAlUI_Zero = m_SayiAlUI.FindAction("Zero", throwIfNotFound: true);
        m_SayiAlUI_GenerateQuestion = m_SayiAlUI.FindAction("GenerateQuestion", throwIfNotFound: true);
        // GameManager
        m_GameManager = asset.FindActionMap("GameManager", throwIfNotFound: true);
        m_GameManager_Escape = m_GameManager.FindAction("Escape", throwIfNotFound: true);
        m_GameManager_ReloadScene = m_GameManager.FindAction("ReloadScene", throwIfNotFound: true);
        // BlackBoardUIManagement
        m_BlackBoardUIManagement = asset.FindActionMap("BlackBoardUIManagement", throwIfNotFound: true);
        m_BlackBoardUIManagement_Exit = m_BlackBoardUIManagement.FindAction("Exit", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Move;
    private readonly InputAction m_Gameplay_Jump;
    private readonly InputAction m_Gameplay_Run;
    private readonly InputAction m_Gameplay_Interact;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Gameplay_Move;
        public InputAction @Jump => m_Wrapper.m_Gameplay_Jump;
        public InputAction @Run => m_Wrapper.m_Gameplay_Run;
        public InputAction @Interact => m_Wrapper.m_Gameplay_Interact;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                @Run.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRun;
                @Interact.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
                @Interact.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnInteract;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
                @Interact.started += instance.OnInteract;
                @Interact.performed += instance.OnInteract;
                @Interact.canceled += instance.OnInteract;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);

    // LockedDoorUI
    private readonly InputActionMap m_LockedDoorUI;
    private ILockedDoorUIActions m_LockedDoorUIActionsCallbackInterface;
    private readonly InputAction m_LockedDoorUI_Zero;
    private readonly InputAction m_LockedDoorUI_One;
    private readonly InputAction m_LockedDoorUI_Two;
    private readonly InputAction m_LockedDoorUI_Three;
    private readonly InputAction m_LockedDoorUI_Four;
    private readonly InputAction m_LockedDoorUI_Five;
    private readonly InputAction m_LockedDoorUI_Six;
    private readonly InputAction m_LockedDoorUI_Seven;
    private readonly InputAction m_LockedDoorUI_Eight;
    private readonly InputAction m_LockedDoorUI_Nine;
    private readonly InputAction m_LockedDoorUI_Enter;
    private readonly InputAction m_LockedDoorUI_Exit;
    private readonly InputAction m_LockedDoorUI_Delete;
    public struct LockedDoorUIActions
    {
        private @PlayerControls m_Wrapper;
        public LockedDoorUIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Zero => m_Wrapper.m_LockedDoorUI_Zero;
        public InputAction @One => m_Wrapper.m_LockedDoorUI_One;
        public InputAction @Two => m_Wrapper.m_LockedDoorUI_Two;
        public InputAction @Three => m_Wrapper.m_LockedDoorUI_Three;
        public InputAction @Four => m_Wrapper.m_LockedDoorUI_Four;
        public InputAction @Five => m_Wrapper.m_LockedDoorUI_Five;
        public InputAction @Six => m_Wrapper.m_LockedDoorUI_Six;
        public InputAction @Seven => m_Wrapper.m_LockedDoorUI_Seven;
        public InputAction @Eight => m_Wrapper.m_LockedDoorUI_Eight;
        public InputAction @Nine => m_Wrapper.m_LockedDoorUI_Nine;
        public InputAction @Enter => m_Wrapper.m_LockedDoorUI_Enter;
        public InputAction @Exit => m_Wrapper.m_LockedDoorUI_Exit;
        public InputAction @Delete => m_Wrapper.m_LockedDoorUI_Delete;
        public InputActionMap Get() { return m_Wrapper.m_LockedDoorUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LockedDoorUIActions set) { return set.Get(); }
        public void SetCallbacks(ILockedDoorUIActions instance)
        {
            if (m_Wrapper.m_LockedDoorUIActionsCallbackInterface != null)
            {
                @Zero.started -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnZero;
                @Zero.performed -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnZero;
                @Zero.canceled -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnZero;
                @One.started -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnOne;
                @One.performed -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnOne;
                @One.canceled -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnOne;
                @Two.started -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnTwo;
                @Two.performed -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnTwo;
                @Two.canceled -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnTwo;
                @Three.started -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnThree;
                @Three.performed -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnThree;
                @Three.canceled -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnThree;
                @Four.started -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnFour;
                @Four.performed -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnFour;
                @Four.canceled -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnFour;
                @Five.started -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnFive;
                @Five.performed -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnFive;
                @Five.canceled -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnFive;
                @Six.started -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnSix;
                @Six.performed -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnSix;
                @Six.canceled -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnSix;
                @Seven.started -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnSeven;
                @Seven.performed -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnSeven;
                @Seven.canceled -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnSeven;
                @Eight.started -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnEight;
                @Eight.performed -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnEight;
                @Eight.canceled -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnEight;
                @Nine.started -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnNine;
                @Nine.performed -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnNine;
                @Nine.canceled -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnNine;
                @Enter.started -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnEnter;
                @Enter.performed -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnEnter;
                @Enter.canceled -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnEnter;
                @Exit.started -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnExit;
                @Exit.performed -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnExit;
                @Exit.canceled -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnExit;
                @Delete.started -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnDelete;
                @Delete.performed -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnDelete;
                @Delete.canceled -= m_Wrapper.m_LockedDoorUIActionsCallbackInterface.OnDelete;
            }
            m_Wrapper.m_LockedDoorUIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Zero.started += instance.OnZero;
                @Zero.performed += instance.OnZero;
                @Zero.canceled += instance.OnZero;
                @One.started += instance.OnOne;
                @One.performed += instance.OnOne;
                @One.canceled += instance.OnOne;
                @Two.started += instance.OnTwo;
                @Two.performed += instance.OnTwo;
                @Two.canceled += instance.OnTwo;
                @Three.started += instance.OnThree;
                @Three.performed += instance.OnThree;
                @Three.canceled += instance.OnThree;
                @Four.started += instance.OnFour;
                @Four.performed += instance.OnFour;
                @Four.canceled += instance.OnFour;
                @Five.started += instance.OnFive;
                @Five.performed += instance.OnFive;
                @Five.canceled += instance.OnFive;
                @Six.started += instance.OnSix;
                @Six.performed += instance.OnSix;
                @Six.canceled += instance.OnSix;
                @Seven.started += instance.OnSeven;
                @Seven.performed += instance.OnSeven;
                @Seven.canceled += instance.OnSeven;
                @Eight.started += instance.OnEight;
                @Eight.performed += instance.OnEight;
                @Eight.canceled += instance.OnEight;
                @Nine.started += instance.OnNine;
                @Nine.performed += instance.OnNine;
                @Nine.canceled += instance.OnNine;
                @Enter.started += instance.OnEnter;
                @Enter.performed += instance.OnEnter;
                @Enter.canceled += instance.OnEnter;
                @Exit.started += instance.OnExit;
                @Exit.performed += instance.OnExit;
                @Exit.canceled += instance.OnExit;
                @Delete.started += instance.OnDelete;
                @Delete.performed += instance.OnDelete;
                @Delete.canceled += instance.OnDelete;
            }
        }
    }
    public LockedDoorUIActions @LockedDoorUI => new LockedDoorUIActions(this);

    // IslemYapUI
    private readonly InputActionMap m_IslemYapUI;
    private IIslemYapUIActions m_IslemYapUIActionsCallbackInterface;
    private readonly InputAction m_IslemYapUI_Zero;
    private readonly InputAction m_IslemYapUI_One;
    private readonly InputAction m_IslemYapUI_Two;
    private readonly InputAction m_IslemYapUI_Three;
    private readonly InputAction m_IslemYapUI_Four;
    private readonly InputAction m_IslemYapUI_Five;
    private readonly InputAction m_IslemYapUI_Six;
    private readonly InputAction m_IslemYapUI_Seven;
    private readonly InputAction m_IslemYapUI_Eight;
    private readonly InputAction m_IslemYapUI_Nine;
    private readonly InputAction m_IslemYapUI_Enter;
    private readonly InputAction m_IslemYapUI_Exit;
    private readonly InputAction m_IslemYapUI_Delete;
    private readonly InputAction m_IslemYapUI_Minus;
    private readonly InputAction m_IslemYapUI_Plus;
    public struct IslemYapUIActions
    {
        private @PlayerControls m_Wrapper;
        public IslemYapUIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Zero => m_Wrapper.m_IslemYapUI_Zero;
        public InputAction @One => m_Wrapper.m_IslemYapUI_One;
        public InputAction @Two => m_Wrapper.m_IslemYapUI_Two;
        public InputAction @Three => m_Wrapper.m_IslemYapUI_Three;
        public InputAction @Four => m_Wrapper.m_IslemYapUI_Four;
        public InputAction @Five => m_Wrapper.m_IslemYapUI_Five;
        public InputAction @Six => m_Wrapper.m_IslemYapUI_Six;
        public InputAction @Seven => m_Wrapper.m_IslemYapUI_Seven;
        public InputAction @Eight => m_Wrapper.m_IslemYapUI_Eight;
        public InputAction @Nine => m_Wrapper.m_IslemYapUI_Nine;
        public InputAction @Enter => m_Wrapper.m_IslemYapUI_Enter;
        public InputAction @Exit => m_Wrapper.m_IslemYapUI_Exit;
        public InputAction @Delete => m_Wrapper.m_IslemYapUI_Delete;
        public InputAction @Minus => m_Wrapper.m_IslemYapUI_Minus;
        public InputAction @Plus => m_Wrapper.m_IslemYapUI_Plus;
        public InputActionMap Get() { return m_Wrapper.m_IslemYapUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(IslemYapUIActions set) { return set.Get(); }
        public void SetCallbacks(IIslemYapUIActions instance)
        {
            if (m_Wrapper.m_IslemYapUIActionsCallbackInterface != null)
            {
                @Zero.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnZero;
                @Zero.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnZero;
                @Zero.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnZero;
                @One.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnOne;
                @One.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnOne;
                @One.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnOne;
                @Two.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnTwo;
                @Two.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnTwo;
                @Two.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnTwo;
                @Three.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnThree;
                @Three.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnThree;
                @Three.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnThree;
                @Four.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnFour;
                @Four.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnFour;
                @Four.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnFour;
                @Five.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnFive;
                @Five.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnFive;
                @Five.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnFive;
                @Six.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnSix;
                @Six.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnSix;
                @Six.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnSix;
                @Seven.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnSeven;
                @Seven.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnSeven;
                @Seven.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnSeven;
                @Eight.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnEight;
                @Eight.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnEight;
                @Eight.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnEight;
                @Nine.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnNine;
                @Nine.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnNine;
                @Nine.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnNine;
                @Enter.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnEnter;
                @Enter.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnEnter;
                @Enter.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnEnter;
                @Exit.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnExit;
                @Exit.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnExit;
                @Exit.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnExit;
                @Delete.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnDelete;
                @Delete.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnDelete;
                @Delete.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnDelete;
                @Minus.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnMinus;
                @Minus.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnMinus;
                @Minus.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnMinus;
                @Plus.started -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnPlus;
                @Plus.performed -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnPlus;
                @Plus.canceled -= m_Wrapper.m_IslemYapUIActionsCallbackInterface.OnPlus;
            }
            m_Wrapper.m_IslemYapUIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Zero.started += instance.OnZero;
                @Zero.performed += instance.OnZero;
                @Zero.canceled += instance.OnZero;
                @One.started += instance.OnOne;
                @One.performed += instance.OnOne;
                @One.canceled += instance.OnOne;
                @Two.started += instance.OnTwo;
                @Two.performed += instance.OnTwo;
                @Two.canceled += instance.OnTwo;
                @Three.started += instance.OnThree;
                @Three.performed += instance.OnThree;
                @Three.canceled += instance.OnThree;
                @Four.started += instance.OnFour;
                @Four.performed += instance.OnFour;
                @Four.canceled += instance.OnFour;
                @Five.started += instance.OnFive;
                @Five.performed += instance.OnFive;
                @Five.canceled += instance.OnFive;
                @Six.started += instance.OnSix;
                @Six.performed += instance.OnSix;
                @Six.canceled += instance.OnSix;
                @Seven.started += instance.OnSeven;
                @Seven.performed += instance.OnSeven;
                @Seven.canceled += instance.OnSeven;
                @Eight.started += instance.OnEight;
                @Eight.performed += instance.OnEight;
                @Eight.canceled += instance.OnEight;
                @Nine.started += instance.OnNine;
                @Nine.performed += instance.OnNine;
                @Nine.canceled += instance.OnNine;
                @Enter.started += instance.OnEnter;
                @Enter.performed += instance.OnEnter;
                @Enter.canceled += instance.OnEnter;
                @Exit.started += instance.OnExit;
                @Exit.performed += instance.OnExit;
                @Exit.canceled += instance.OnExit;
                @Delete.started += instance.OnDelete;
                @Delete.performed += instance.OnDelete;
                @Delete.canceled += instance.OnDelete;
                @Minus.started += instance.OnMinus;
                @Minus.performed += instance.OnMinus;
                @Minus.canceled += instance.OnMinus;
                @Plus.started += instance.OnPlus;
                @Plus.performed += instance.OnPlus;
                @Plus.canceled += instance.OnPlus;
            }
        }
    }
    public IslemYapUIActions @IslemYapUI => new IslemYapUIActions(this);

    // SayiAlUI
    private readonly InputActionMap m_SayiAlUI;
    private ISayiAlUIActions m_SayiAlUIActionsCallbackInterface;
    private readonly InputAction m_SayiAlUI_Delete;
    private readonly InputAction m_SayiAlUI_Exit;
    private readonly InputAction m_SayiAlUI_Enter;
    private readonly InputAction m_SayiAlUI_Nine;
    private readonly InputAction m_SayiAlUI_Eight;
    private readonly InputAction m_SayiAlUI_Seven;
    private readonly InputAction m_SayiAlUI_Six;
    private readonly InputAction m_SayiAlUI_Five;
    private readonly InputAction m_SayiAlUI_Four;
    private readonly InputAction m_SayiAlUI_Three;
    private readonly InputAction m_SayiAlUI_Two;
    private readonly InputAction m_SayiAlUI_One;
    private readonly InputAction m_SayiAlUI_Zero;
    private readonly InputAction m_SayiAlUI_GenerateQuestion;
    public struct SayiAlUIActions
    {
        private @PlayerControls m_Wrapper;
        public SayiAlUIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Delete => m_Wrapper.m_SayiAlUI_Delete;
        public InputAction @Exit => m_Wrapper.m_SayiAlUI_Exit;
        public InputAction @Enter => m_Wrapper.m_SayiAlUI_Enter;
        public InputAction @Nine => m_Wrapper.m_SayiAlUI_Nine;
        public InputAction @Eight => m_Wrapper.m_SayiAlUI_Eight;
        public InputAction @Seven => m_Wrapper.m_SayiAlUI_Seven;
        public InputAction @Six => m_Wrapper.m_SayiAlUI_Six;
        public InputAction @Five => m_Wrapper.m_SayiAlUI_Five;
        public InputAction @Four => m_Wrapper.m_SayiAlUI_Four;
        public InputAction @Three => m_Wrapper.m_SayiAlUI_Three;
        public InputAction @Two => m_Wrapper.m_SayiAlUI_Two;
        public InputAction @One => m_Wrapper.m_SayiAlUI_One;
        public InputAction @Zero => m_Wrapper.m_SayiAlUI_Zero;
        public InputAction @GenerateQuestion => m_Wrapper.m_SayiAlUI_GenerateQuestion;
        public InputActionMap Get() { return m_Wrapper.m_SayiAlUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SayiAlUIActions set) { return set.Get(); }
        public void SetCallbacks(ISayiAlUIActions instance)
        {
            if (m_Wrapper.m_SayiAlUIActionsCallbackInterface != null)
            {
                @Delete.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnDelete;
                @Delete.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnDelete;
                @Delete.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnDelete;
                @Exit.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnExit;
                @Exit.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnExit;
                @Exit.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnExit;
                @Enter.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnEnter;
                @Enter.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnEnter;
                @Enter.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnEnter;
                @Nine.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnNine;
                @Nine.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnNine;
                @Nine.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnNine;
                @Eight.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnEight;
                @Eight.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnEight;
                @Eight.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnEight;
                @Seven.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnSeven;
                @Seven.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnSeven;
                @Seven.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnSeven;
                @Six.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnSix;
                @Six.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnSix;
                @Six.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnSix;
                @Five.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnFive;
                @Five.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnFive;
                @Five.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnFive;
                @Four.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnFour;
                @Four.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnFour;
                @Four.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnFour;
                @Three.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnThree;
                @Three.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnThree;
                @Three.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnThree;
                @Two.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnTwo;
                @Two.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnTwo;
                @Two.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnTwo;
                @One.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnOne;
                @One.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnOne;
                @One.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnOne;
                @Zero.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnZero;
                @Zero.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnZero;
                @Zero.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnZero;
                @GenerateQuestion.started -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnGenerateQuestion;
                @GenerateQuestion.performed -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnGenerateQuestion;
                @GenerateQuestion.canceled -= m_Wrapper.m_SayiAlUIActionsCallbackInterface.OnGenerateQuestion;
            }
            m_Wrapper.m_SayiAlUIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Delete.started += instance.OnDelete;
                @Delete.performed += instance.OnDelete;
                @Delete.canceled += instance.OnDelete;
                @Exit.started += instance.OnExit;
                @Exit.performed += instance.OnExit;
                @Exit.canceled += instance.OnExit;
                @Enter.started += instance.OnEnter;
                @Enter.performed += instance.OnEnter;
                @Enter.canceled += instance.OnEnter;
                @Nine.started += instance.OnNine;
                @Nine.performed += instance.OnNine;
                @Nine.canceled += instance.OnNine;
                @Eight.started += instance.OnEight;
                @Eight.performed += instance.OnEight;
                @Eight.canceled += instance.OnEight;
                @Seven.started += instance.OnSeven;
                @Seven.performed += instance.OnSeven;
                @Seven.canceled += instance.OnSeven;
                @Six.started += instance.OnSix;
                @Six.performed += instance.OnSix;
                @Six.canceled += instance.OnSix;
                @Five.started += instance.OnFive;
                @Five.performed += instance.OnFive;
                @Five.canceled += instance.OnFive;
                @Four.started += instance.OnFour;
                @Four.performed += instance.OnFour;
                @Four.canceled += instance.OnFour;
                @Three.started += instance.OnThree;
                @Three.performed += instance.OnThree;
                @Three.canceled += instance.OnThree;
                @Two.started += instance.OnTwo;
                @Two.performed += instance.OnTwo;
                @Two.canceled += instance.OnTwo;
                @One.started += instance.OnOne;
                @One.performed += instance.OnOne;
                @One.canceled += instance.OnOne;
                @Zero.started += instance.OnZero;
                @Zero.performed += instance.OnZero;
                @Zero.canceled += instance.OnZero;
                @GenerateQuestion.started += instance.OnGenerateQuestion;
                @GenerateQuestion.performed += instance.OnGenerateQuestion;
                @GenerateQuestion.canceled += instance.OnGenerateQuestion;
            }
        }
    }
    public SayiAlUIActions @SayiAlUI => new SayiAlUIActions(this);

    // GameManager
    private readonly InputActionMap m_GameManager;
    private IGameManagerActions m_GameManagerActionsCallbackInterface;
    private readonly InputAction m_GameManager_Escape;
    private readonly InputAction m_GameManager_ReloadScene;
    public struct GameManagerActions
    {
        private @PlayerControls m_Wrapper;
        public GameManagerActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Escape => m_Wrapper.m_GameManager_Escape;
        public InputAction @ReloadScene => m_Wrapper.m_GameManager_ReloadScene;
        public InputActionMap Get() { return m_Wrapper.m_GameManager; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameManagerActions set) { return set.Get(); }
        public void SetCallbacks(IGameManagerActions instance)
        {
            if (m_Wrapper.m_GameManagerActionsCallbackInterface != null)
            {
                @Escape.started -= m_Wrapper.m_GameManagerActionsCallbackInterface.OnEscape;
                @Escape.performed -= m_Wrapper.m_GameManagerActionsCallbackInterface.OnEscape;
                @Escape.canceled -= m_Wrapper.m_GameManagerActionsCallbackInterface.OnEscape;
                @ReloadScene.started -= m_Wrapper.m_GameManagerActionsCallbackInterface.OnReloadScene;
                @ReloadScene.performed -= m_Wrapper.m_GameManagerActionsCallbackInterface.OnReloadScene;
                @ReloadScene.canceled -= m_Wrapper.m_GameManagerActionsCallbackInterface.OnReloadScene;
            }
            m_Wrapper.m_GameManagerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Escape.started += instance.OnEscape;
                @Escape.performed += instance.OnEscape;
                @Escape.canceled += instance.OnEscape;
                @ReloadScene.started += instance.OnReloadScene;
                @ReloadScene.performed += instance.OnReloadScene;
                @ReloadScene.canceled += instance.OnReloadScene;
            }
        }
    }
    public GameManagerActions @GameManager => new GameManagerActions(this);

    // BlackBoardUIManagement
    private readonly InputActionMap m_BlackBoardUIManagement;
    private IBlackBoardUIManagementActions m_BlackBoardUIManagementActionsCallbackInterface;
    private readonly InputAction m_BlackBoardUIManagement_Exit;
    public struct BlackBoardUIManagementActions
    {
        private @PlayerControls m_Wrapper;
        public BlackBoardUIManagementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Exit => m_Wrapper.m_BlackBoardUIManagement_Exit;
        public InputActionMap Get() { return m_Wrapper.m_BlackBoardUIManagement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BlackBoardUIManagementActions set) { return set.Get(); }
        public void SetCallbacks(IBlackBoardUIManagementActions instance)
        {
            if (m_Wrapper.m_BlackBoardUIManagementActionsCallbackInterface != null)
            {
                @Exit.started -= m_Wrapper.m_BlackBoardUIManagementActionsCallbackInterface.OnExit;
                @Exit.performed -= m_Wrapper.m_BlackBoardUIManagementActionsCallbackInterface.OnExit;
                @Exit.canceled -= m_Wrapper.m_BlackBoardUIManagementActionsCallbackInterface.OnExit;
            }
            m_Wrapper.m_BlackBoardUIManagementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Exit.started += instance.OnExit;
                @Exit.performed += instance.OnExit;
                @Exit.canceled += instance.OnExit;
            }
        }
    }
    public BlackBoardUIManagementActions @BlackBoardUIManagement => new BlackBoardUIManagementActions(this);
    public interface IGameplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
    public interface ILockedDoorUIActions
    {
        void OnZero(InputAction.CallbackContext context);
        void OnOne(InputAction.CallbackContext context);
        void OnTwo(InputAction.CallbackContext context);
        void OnThree(InputAction.CallbackContext context);
        void OnFour(InputAction.CallbackContext context);
        void OnFive(InputAction.CallbackContext context);
        void OnSix(InputAction.CallbackContext context);
        void OnSeven(InputAction.CallbackContext context);
        void OnEight(InputAction.CallbackContext context);
        void OnNine(InputAction.CallbackContext context);
        void OnEnter(InputAction.CallbackContext context);
        void OnExit(InputAction.CallbackContext context);
        void OnDelete(InputAction.CallbackContext context);
    }
    public interface IIslemYapUIActions
    {
        void OnZero(InputAction.CallbackContext context);
        void OnOne(InputAction.CallbackContext context);
        void OnTwo(InputAction.CallbackContext context);
        void OnThree(InputAction.CallbackContext context);
        void OnFour(InputAction.CallbackContext context);
        void OnFive(InputAction.CallbackContext context);
        void OnSix(InputAction.CallbackContext context);
        void OnSeven(InputAction.CallbackContext context);
        void OnEight(InputAction.CallbackContext context);
        void OnNine(InputAction.CallbackContext context);
        void OnEnter(InputAction.CallbackContext context);
        void OnExit(InputAction.CallbackContext context);
        void OnDelete(InputAction.CallbackContext context);
        void OnMinus(InputAction.CallbackContext context);
        void OnPlus(InputAction.CallbackContext context);
    }
    public interface ISayiAlUIActions
    {
        void OnDelete(InputAction.CallbackContext context);
        void OnExit(InputAction.CallbackContext context);
        void OnEnter(InputAction.CallbackContext context);
        void OnNine(InputAction.CallbackContext context);
        void OnEight(InputAction.CallbackContext context);
        void OnSeven(InputAction.CallbackContext context);
        void OnSix(InputAction.CallbackContext context);
        void OnFive(InputAction.CallbackContext context);
        void OnFour(InputAction.CallbackContext context);
        void OnThree(InputAction.CallbackContext context);
        void OnTwo(InputAction.CallbackContext context);
        void OnOne(InputAction.CallbackContext context);
        void OnZero(InputAction.CallbackContext context);
        void OnGenerateQuestion(InputAction.CallbackContext context);
    }
    public interface IGameManagerActions
    {
        void OnEscape(InputAction.CallbackContext context);
        void OnReloadScene(InputAction.CallbackContext context);
    }
    public interface IBlackBoardUIManagementActions
    {
        void OnExit(InputAction.CallbackContext context);
    }
}
