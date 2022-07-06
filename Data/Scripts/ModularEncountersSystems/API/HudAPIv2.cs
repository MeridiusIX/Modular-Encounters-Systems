using ProtoBuf;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Text;
using VRage;
using VRage.Input;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;
using BlendTypeEnum = VRageRender.MyBillboard.BlendTypeEnum;

namespace ModularEncountersSystems.API
{
	public class HudAPIv2
	{

		public const string DefaultFont = "white";
		public const BlendTypeEnum DefaultHUDBlendType = BlendTypeEnum.PostPP;
		public const BlendTypeEnum DefaultWorldBlendType = BlendTypeEnum.Standard;

		private static HudAPIv2 instance;
		private const long REGISTRATIONID = 573804956;
		private bool registered = false;
		private Action m_onRegisteredAction;

		private Func<int, object> MessageFactory;
		private Action<object, int, object> MessageSetter;
		private Func<object, int, object> MessageGetter;
		private Action<object> RemoveMessage;

		private Action m_onScreenDimensionsChanged;

		public Action OnScreenDimensionsChanged
		{
			get
			{
				return m_onScreenDimensionsChanged;
			}

			set
			{
				m_onScreenDimensionsChanged = value;
			}
		}

		public enum TextOrientation : byte
		{
			ltr = 1,
			center = 2,
			rtl = 3
		}

		/// <summary>
		/// Create a HudAPI Instance. Please only create one per mod. 
		/// </summary>
		/// <param name="onRegisteredAction">Callback once the HudAPI is active. You can Instantiate HudAPI objects in this Action</param>
		public HudAPIv2(Action onRegisteredAction = null)
		{
			if (instance != null)
			{

					return;
			}
			instance = this;
			m_onRegisteredAction = onRegisteredAction;
            MyAPIGateway.Utilities.RegisterMessageHandler(REGISTRATIONID, RegisterComponents);
        }

		public void Close()
		{
			Unload();
		}
		/// <summary>
		/// Unregisters mod and frees references. 
		/// </summary>
		public void Unload()
		{
			MyAPIGateway.Utilities.UnregisterMessageHandler(REGISTRATIONID, RegisterComponents);
			MessageFactory = null;
			MessageSetter = null;
			MessageGetter = null;
			RemoveMessage = null;
			registered = false;
			m_onRegisteredAction = null;
			if(instance == this)
				instance = null;
        }
		private enum RegistrationEnum : int
		{
			OnScreenUpdate = 2000
		}
		private void RegisterComponents(object obj)
		{
			if (registered)
				return;
			if(obj is MyTuple<Func<int, object>, Action<object, int, object>, Func<object, int, object>, Action<object>>)
            {
				var Handlers = (MyTuple<Func<int, object>, Action<object, int, object>, Func<object, int, object>, Action<object>>)obj;
				MessageFactory = Handlers.Item1;
				MessageSetter = Handlers.Item2;
				MessageGetter = Handlers.Item3;
				RemoveMessage = Handlers.Item4;

				registered = true;
				if (m_onRegisteredAction != null)
					m_onRegisteredAction();
				APIDialog.GetDialogMethods(MessageGetter);
				MessageSet(null, (int)RegistrationEnum.OnScreenUpdate, new MyTuple<Action>(ScreenChangedHandle));
				
            }
        }

		/// <summary>
		/// If Heartbeat is true you may call any constructor in this class. Do not call any constructor or set properties if this is false.
		/// </summary>
		public bool Heartbeat
		{
			get
			{
				return registered;
			}
		}



		#region Intercomm
		private void DeleteMessage(object BackingObject)
		{
			if(BackingObject != null)
				RemoveMessage(BackingObject);
		}
		private object CreateMessage(MessageTypes type)
		{
			return MessageFactory((int)type);
		}
		private object MessageGet(object BackingObject, int Member )
		{
			return MessageGetter(BackingObject, Member);
		}
		private void MessageSet(object BackingObject, int Member, object Value)
		{
			MessageSetter(BackingObject, Member, Value);
		}
		private void RegisterCheck()
		{
			if (instance.registered == false)
			{
				throw new InvalidOperationException("HudAPI: Failed to create backing object. Do not instantiate without checking if heartbeat is true.");
			}
		}
		private void ScreenChangedHandle()
		{
			if(m_onScreenDimensionsChanged != null)
			{
				m_onScreenDimensionsChanged();
            }
        }
		#endregion
		private enum MessageTypes : int
		{
			HUDMessage = 0,
			BillBoardHUDMessage,
			EntityMessage,
			SpaceMessage,
			BillboardTriHUDMessage,

			MenuItem = 20,
			MenuSubCategory,
			MenuRootCategory,
			MenuScreenInput,
			MenuSliderItem,
			MenuTextInput,
			MenuKeybindInput,
			MenuColorPickerInput,

			BoxUIContainer = 40,
			BoxUIText,
			BoxUIImage,

			UIDefinition = 60,
			UIBehaviourDefinition
		}
		#region CustomDialogs
		public static class APIDialog
		{
			private enum APIDialogs : int
			{
				ColorPickerDialog = 1100,
				TextDialog,
				KeybindDialog,
				ScreenInputDialog,
				SliderDialog
			}

			private static Func<StringBuilder, Color, Action<Color>, Action<Color>, Action, bool, bool, bool> ColorPickerDialogDelagete;
			private static Func<Action<string>, StringBuilder, bool> TextDialogDelagete;
			private static Func<Action<MyKeys, bool, bool, bool>, StringBuilder, bool> KeybindDialogDelagete;
			private static Func<StringBuilder, Vector2D, Vector2D, Action<Vector2D>, Action<Vector2D>, Action, bool> ScreenInputDialogDelagete;
			private static Func<StringBuilder, Action<float>, float, Func<float, object>, Action, bool> SliderDialogDelagete;
			internal static void GetDialogMethods(Func<object, int, object> messageGetter)
			{
				ColorPickerDialogDelagete = messageGetter.Invoke((int)APIinfo.APIinfoMembers.GetDialog, (int)APIDialogs.ColorPickerDialog)
					as Func<StringBuilder, Color, Action<Color>, Action<Color>, Action, bool, bool, bool>;
				TextDialogDelagete = messageGetter.Invoke((int)APIinfo.APIinfoMembers.GetDialog, (int)APIDialogs.TextDialog)
					as Func<Action<string>, StringBuilder, bool>;
				KeybindDialogDelagete = messageGetter.Invoke((int)APIinfo.APIinfoMembers.GetDialog, (int)APIDialogs.KeybindDialog)
					as Func<Action<MyKeys, bool, bool, bool>, StringBuilder, bool>;
				ScreenInputDialogDelagete = messageGetter.Invoke((int)APIinfo.APIinfoMembers.GetDialog, (int)APIDialogs.ScreenInputDialog)
					as Func<StringBuilder, Vector2D, Vector2D, Action<Vector2D>, Action<Vector2D>, Action, bool>;
				SliderDialogDelagete = messageGetter.Invoke((int)APIinfo.APIinfoMembers.GetDialog, (int)APIDialogs.SliderDialog)
					as Func<StringBuilder, Action<float>, float, Func<float, object>, Action, bool>;
			}

			public static bool ColorPickerDialog(StringBuilder Title, Color InitialColor, Action<Color> onSubmit, Action<Color> onUpdate, Action onCancel, bool showAlpha, bool usehsv = false)
			{
				return ColorPickerDialogDelagete?.Invoke(Title, InitialColor, onSubmit, onUpdate, onCancel, showAlpha, usehsv) ?? false;
			}
			
			public static bool TextDialog(Action<string> onSubmit, StringBuilder Title)
			{
				return TextDialogDelagete?.Invoke(onSubmit, Title) ?? false;
			}
			
			public static bool KeybindDialog(Action<MyKeys, bool, bool, bool> onSubmit, StringBuilder Title)
			{
				return KeybindDialogDelagete?.Invoke(onSubmit, Title) ?? false;
			}
			
			public static bool ScreenInputDialog(StringBuilder title, Vector2D origin, Vector2D size, Action<Vector2D> onSubmit, Action<Vector2D> onUpdate, Action onCancel)
			{
				return ScreenInputDialogDelagete?.Invoke(title, origin, size, onSubmit, onUpdate, onCancel) ?? false;
			}
			
			static public bool SliderDialog(StringBuilder title, Action<float> onSubmit, float initialvalue, Func<float, object> SliderPercentToValue, Action OnCancel)
			{
				return SliderDialogDelagete?.Invoke(title, onSubmit, initialvalue, SliderPercentToValue, OnCancel) ?? false;
			}
		}

		#endregion

		#region Info
		public static class APIinfo
		{
			internal enum APIinfoMembers : int
			{
				ScreenPositionOnePX = 1000,
				OnScreenUpdate,
				GetBoxUIDefinition,
				GetBoxUIBehaviour,
				GetFontDefinition,
				GetFonts,
				GetDialog
			}


			/// <summary>
			/// Returns the distance for one pixel in x and y directions, can be multiplied and fed into Origin, Offset, and Size parameters for precise manipulation of HUD objects. 
			/// </summary>
			public static Vector2D ScreenPositionOnePX
			{
				get
				{
					return (Vector2D)instance.MessageGet(null, (int)APIinfoMembers.ScreenPositionOnePX);
				}
			}
			/// <summary>
			/// Available definitions: None, Default, Square
			/// </summary>
			/// <param name="DefinitionName"></param>
			/// <returns></returns>
			public static BoxUIDefinition GetBoxUIDefinition(MyStringId DefinitionName)
			{
				return new BoxUIDefinition(instance.MessageGet(DefinitionName, (int)APIinfoMembers.GetBoxUIDefinition));

			}
			public static BoxUIBehaviourDef GetBoxUIBehaviour(MyStringId DefinitionName)
			{
				return new BoxUIBehaviourDef(instance.MessageGet(DefinitionName, (int)APIinfoMembers.GetBoxUIBehaviour));

			}

			public static FontDefinition GetFontDefinition(MyStringId DefinitionName)
			{
				object retval = instance.MessageGet(DefinitionName, (int)APIinfoMembers.GetFontDefinition);
                return new FontDefinition(retval);

			}
			/// <summary>
			/// Gives a list of fonts currently available in the TextHudAPI
			/// </summary>
			/// <param name="collection">Fonts will be added to the collection, if null a new collection will be allocated</param>
			public static void GetFonts(List<MyStringId> collection)
			{
				instance.MessageGet(collection, (int)APIinfoMembers.GetFonts);
			}


		}
		#endregion
		#region Messages
		public enum Options : byte
		{
			None = 0x0,
			HideHud = 0x1,
			Shadowing = 0x2,
			Fixed = 0x4,
			FOVScale = 0x8,
			Pixel = 0x10
		}
		private enum MessageBaseMembers : int
		{
			Message = 0,
			Visible,
			TimeToLive,
			Scale,
			TextLength,
			Offset,
			BlendType,
			Draw,
			Flush,
			SkipLinearRGB
		}
		public abstract class MessageBase
		{
			internal object BackingObject;

			#region Properties
			/// <summary>
			/// Note that if you update the stringbuilder anywhere it will update the message automatically. Use this property to set the stringbuilder object to your own or use the one generated by the constructor.
			/// </summary>
			public StringBuilder Message
			{
				get
				{
					return (StringBuilder)(instance.MessageGet(BackingObject, (int)MessageBaseMembers.Message));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MessageBaseMembers.Message, value);
				}
			}


			/// <summary>
			/// True if HUD Element is visible, note that this will still be true if the player has their hud activated and HideHud option is set. 
			/// </summary>
			public bool Visible
			{
				get
				{
					return (bool)(instance.MessageGet(BackingObject, (int)MessageBaseMembers.Visible));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MessageBaseMembers.Visible, value);
				}
			}

			/// <summary>
			/// Time to live in Draw ticks. At 0 class will close itself and will no longer update.
			/// </summary>
			public int TimeToLive
			{
				get
				{
					return (int)(instance.MessageGet(BackingObject, (int)MessageBaseMembers.TimeToLive));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MessageBaseMembers.TimeToLive, value);
				}
			}


			/// <summary>
			/// Scale of the text elements or billboard
			/// </summary>
			public double Scale
			{
				get
				{
					return (double)(instance.MessageGet(BackingObject, (int)MessageBaseMembers.Scale));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MessageBaseMembers.Scale, value);
				}
			}


			/// <summary>
			/// Offset the text element by this amount. Note this takes the result of GetTextLength, be sure to clear Offset.Y if you do not want to start at the lower left corner of the previous element
			/// </summary>
			public Vector2D Offset
			{
				get
				{
					return (Vector2D)(instance.MessageGet(BackingObject, (int)MessageBaseMembers.Offset));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MessageBaseMembers.Offset, value);
				}
			}

			/// <summary>
			/// put using BlendTypeEnum = VRageRender.MyBillboard.BlendTypeEnum; on top of your script to use this property.
			/// </summary>
			public BlendTypeEnum Blend
			{
				get
				{
					return (BlendTypeEnum)(instance.MessageGet(BackingObject, (int)MessageBaseMembers.BlendType));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MessageBaseMembers.BlendType, value);
				}
			}
			/// <summary>
			/// Skips LinearRGB call in TextHUDAPI
			/// </summary>
			public bool SkipLinearRGB
			{
				get
				{
					return (bool)(instance.MessageGet(BackingObject, (int)MessageBaseMembers.SkipLinearRGB));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MessageBaseMembers.SkipLinearRGB, value);
				}
			}
			#endregion

			public abstract void DeleteMessage();

			/// <summary>
			/// Gets the offset of the lower right corner of the text element from the upper left. The value returned is a local translation. Screen space for screen messages, world space for world messages. Please note that the Y value is negative in screen space. 
			/// </summary>
			/// <returns>Lower Right Corner</returns>
			public Vector2D GetTextLength()
			{
				return (Vector2D)(instance.MessageGet(BackingObject, (int)MessageBaseMembers.TextLength));
			}

			/// <summary>
			/// Manual draw method
			/// </summary>
			public void Draw()
			{
				instance.MessageGet(BackingObject, (int)MessageBaseMembers.Draw);
			}

			/// <summary>
			/// Clears the object cache
			/// </summary>
			public void Flush()
			{
				instance.MessageGet(BackingObject, (int)MessageBaseMembers.Flush);
			}

		}
		public class EntityMessage : MessageBase
		{
			private enum EntityMembers : int
			{
				Entity = 10,
				LocalPosition,
				Up,
				Forward,
				Orientation,
				Max,
				TransformMatrix,
				Font
			}

			#region Properties
			/// <summary>
			/// Entity text will be centered on / attached to. 
			/// </summary>
			public IMyEntity Entity
			{
				get
				{
					return instance.MessageGet(BackingObject, (int)EntityMembers.Entity) as IMyEntity;
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Entity, value);
				}
			}


			/// <summary>
			/// Local translation of where the text will be in relation to the Entity it is attached to. Used to construct the TransformMatrix
			/// </summary>
			public Vector3D LocalPosition
			{
				get
				{
					return (Vector3D)instance.MessageGet(BackingObject, (int)EntityMembers.LocalPosition);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.LocalPosition, value);
				}
			}

			/// <summary>
			///  Up, value used to construct the TransformMatrix
			/// </summary>
			public Vector3D Up
			{
				get
				{
					return (Vector3D)instance.MessageGet(BackingObject, (int)EntityMembers.Up);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Up, value);
				}
			}

			/// <summary>
			/// Forward, value used to construct the TransformMatrix
			/// </summary>
			public Vector3D Forward
			{
				get
				{
					return (Vector3D)instance.MessageGet(BackingObject, (int)EntityMembers.Forward);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Forward, value);
				}
			}

			/// <summary>
			/// Flag that sets from what direction text is written
			/// </summary>
			public TextOrientation Orientation
			{
				get
				{
					return (TextOrientation)instance.MessageGet(BackingObject, (int)EntityMembers.Orientation);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Orientation, (byte)value);
				}
			}


			/// <summary>
			/// World Boundries
			/// </summary>
			public Vector2D Max
			{
				get
				{
					return (Vector2D)instance.MessageGet(BackingObject, (int)EntityMembers.Max);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Max, value);
				}
			}

			/// <summary>
			/// Sets the transformation matrix directly, use instead of LocalPosition, Up, Forward
			/// </summary>
			public MatrixD TransformMatrix
			{
				get
				{
					return (MatrixD)instance.MessageGet(BackingObject, (int)EntityMembers.TransformMatrix);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.TransformMatrix, value);
				}
			}
			/// <summary>
			/// Font, default is "white", "monospace" is also included. 
			/// </summary>
			public string Font
			{
				get
				{
					return (string)(instance.MessageGet(BackingObject, (int)EntityMembers.Font));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Font, value);
				}
			}
			#endregion
			public EntityMessage(StringBuilder Message, IMyEntity Entity, MatrixD TransformMatrix, int TimeToLive = -1, double Scale = 1, TextOrientation Orientation = TextOrientation.ltr, Vector2D? Offset = null, Vector2D? Max = null, string Font = DefaultFont)
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.EntityMessage);
				if (BackingObject != null)
				{
					if (Max.HasValue)
						this.Max = Max.Value;
					this.Message = Message;
					this.Entity = Entity;
					this.TransformMatrix = TransformMatrix;
					this.TimeToLive = TimeToLive;
					this.Scale = Scale;
					this.Visible = true;
					this.Orientation = Orientation;
					this.Blend = DefaultWorldBlendType;
					if (Offset.HasValue)
					{
						this.Offset = Offset.Value;
					}
					else
					{
						this.Offset = Vector2D.Zero;
					}
					this.Font = Font;
				}

			}
			public EntityMessage(StringBuilder Message, IMyEntity Entity, Vector3D LocalPosition, Vector3D Forward, Vector3D Up, int TimeToLive = -1, double Scale = 1, TextOrientation Orientation = TextOrientation.ltr, Vector2D? Offset = null, Vector2D? Max = null, BlendTypeEnum Blend = DefaultWorldBlendType, string Font = DefaultFont)
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.EntityMessage);
				if(BackingObject != null)
				{
					if (Max.HasValue)
						this.Max = Max.Value;
					this.Message = Message;
					this.Entity = Entity;
					this.LocalPosition = LocalPosition;
					this.Forward = Forward;
					this.Up = Up;
					this.TimeToLive = TimeToLive;
					this.Scale = Scale;
					this.Visible = true;
					this.Orientation = Orientation;
					this.Blend = Blend;
					if (Offset.HasValue)
					{
						this.Offset = Offset.Value;
					}
					else
					{
						this.Offset = Vector2D.Zero;
					}
					this.Font = Font;
				}

			}

			public EntityMessage()
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.EntityMessage);

			}

			/// <summary>
			/// Do not use this class after deleting it. 
			/// </summary>
			public override void DeleteMessage()
			{
				instance.DeleteMessage(BackingObject);
				BackingObject = null;
			}
		}
		public class HUDMessage : MessageBase
		{
			private enum EntityMembers : int
			{
				Origin = 10,
				Options,
				ShadowColor,
				Font,
				InitalColor
			}
			#region Properties
			/// <summary>
			/// top left is -1, 1, bottom right is 1 -1
			/// </summary>
			public Vector2D Origin
			{
				get
				{
					return (Vector2D)(instance.MessageGet(BackingObject, (int)EntityMembers.Origin));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Origin, value);
				}
			}


			/// <summary>
			/// HideHud - hides when hud is hidden, shadow draw a shadow behind the text. 
			/// </summary>
			public Options Options
			{
				get
				{
					return (Options)(instance.MessageGet(BackingObject, (int)EntityMembers.Options));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Options, (byte)value);
				}
			}

			/// <summary>
			/// Color of shadow behind the text
			/// </summary>
			public Color ShadowColor
			{
				get
				{
					return (Color)(instance.MessageGet(BackingObject, (int)EntityMembers.ShadowColor));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.ShadowColor, value);
				}
			}
			/// <summary>
			/// Font, default is "white", "monospace" also supported, modded fonts will be supported in the future.
			/// </summary>
			public string Font
			{
				get
				{
					return (string)(instance.MessageGet(BackingObject, (int)EntityMembers.Font));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Font, value);
				}
			}

			/// <summary>
			/// Sets the initial color of the text, Default: White
			/// </summary>
			public Color InitialColor
			{
				get
				{
					return (Color)(instance.MessageGet(BackingObject, (int)EntityMembers.InitalColor));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.InitalColor, value);
				}
			}
			#endregion

			public HUDMessage(StringBuilder Message, Vector2D Origin, Vector2D? Offset = null, int TimeToLive = -1,   double Scale = 1.0d, bool HideHud = true, bool Shadowing = false, Color? ShadowColor = null, BlendTypeEnum Blend = DefaultHUDBlendType, string Font = DefaultFont)
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.HUDMessage);
				if(BackingObject != null)
				{
					this.TimeToLive = TimeToLive;
					this.Origin = Origin;
					this.Options = Options.None;
					if (HideHud)
						Options |= Options.HideHud;
					if (Shadowing)
						Options |= Options.Shadowing;
					var blackshadow = Color.Black;
					if (ShadowColor.HasValue)
						ShadowColor = ShadowColor.Value;
					this.Scale = Scale;
					this.Message = Message;
					this.Blend = Blend;
					if (Offset.HasValue)
					{
						this.Offset = Offset.Value;
					}
					else
					{
						this.Offset = Vector2D.Zero;
					}
					this.Font = Font;
				}
			}
			public HUDMessage()
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.HUDMessage);
			}

			public override void DeleteMessage()
			{
				instance.DeleteMessage(BackingObject);
				BackingObject = null;
			}

		}
		public class BillBoardHUDMessage : MessageBase
		{

			private enum EntityMembers : int
			{
				Origin = 10,
				Options,
				BillBoardColor,
				Material,
				Rotation,
				Width,
				Height,
				uvOffset,
				uvSize,
				TextureSize,
				uvEnabled
			}

			#region Properties
			/// <summary>
			/// top left is -1, 1, bottom right is 1 -1
			/// </summary>
			public Vector2D Origin
			{
				get
				{
					return (Vector2D)instance.MessageGet(BackingObject, (int)EntityMembers.Origin);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Origin, value);
				}
			}

			/// <summary>
			/// Use MyStringId.GetOrCompute to turn a string into a MyStringId. 
			/// </summary>
			public MyStringId Material
			{
				get
				{
					return (MyStringId)instance.MessageGet(BackingObject, (int)EntityMembers.Material);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Material, value);
				}
			}


			/// <summary>
			/// Set Options, HideHud to true will hide billboard when hud is hidden. Shadowing will draw the element on the shadow layer (behind the text layer)
			/// </summary>
			public Options Options
			{
				get
				{
					return (Options)instance.MessageGet(BackingObject, (int)EntityMembers.Options);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Options, (byte)value);
				}
			}


			/// <summary>
			/// Sets the color mask of the billboard, not all billboards support this parameter. 
			/// </summary>
			public Color BillBoardColor
			{
				get
				{
					return (Color)instance.MessageGet(BackingObject, (int)EntityMembers.BillBoardColor);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.BillBoardColor, value);
				}
			}

			/// <summary>
			/// Rotate billboard in radians.
			/// </summary>
			public float Rotation
			{
				get
				{
					return (float)instance.MessageGet(BackingObject, (int)EntityMembers.Rotation);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Rotation, value);
				}
			}


			/// <summary>
			/// Multiplies the width of the billboard by this amount. Set Scale to 1 if you want to use this to finely control the width of the billboard, such as a value from GetTextLength
			/// You might need to multiply the result of GetTextLength by 250 or maybe 500 if Scale is 1. Will need experiementing
			/// </summary>
			public float Width
			{
				get
				{
					return (float)instance.MessageGet(BackingObject, (int)EntityMembers.Width);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Width, value);
				}
			}


			/// <summary>
			/// Multiplies the height of the billboard by this amount. Set Scale to 1 if you want to use this to finely control the height of the billboard, such as a value from GetTextLength
			/// </summary>
			public float Height
			{
				get
				{
					return (float)instance.MessageGet(BackingObject, (int)EntityMembers.Height);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Height, value);
				}
			}

			/// <summary>
			/// UV offset in pixels
			/// </summary>
			public Vector2 uvOffset
			{
				get
				{
					return (Vector2)instance.MessageGet(BackingObject, (int)EntityMembers.uvOffset);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.uvOffset, value);
				}
			}
			/// <summary>
			/// Size in pixels
			/// </summary>
			public Vector2 uvSize
			{
				get
				{
					return (Vector2)instance.MessageGet(BackingObject, (int)EntityMembers.uvSize);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.uvSize, value);
				}
			}
			/// <summary>
			/// Size of image in pixels (please note the height and width of the image must be the same)
			/// </summary>
			public float TextureSize
			{
				get
				{
					return (float)instance.MessageGet(BackingObject, (int)EntityMembers.TextureSize);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.TextureSize, value);
				}
			}
			/// <summary>
			/// Use uv parameters. Default is false.
			/// </summary>
			public bool uvEnabled
			{
				get
				{
					return (bool)instance.MessageGet(BackingObject, (int)EntityMembers.uvEnabled);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.uvEnabled, value);
				}
			}
			#endregion

			public BillBoardHUDMessage(MyStringId Material, Vector2D Origin, Color BillBoardColor, Vector2D? Offset = null, int TimeToLive = -1, double Scale = 1d, float Width = 1f, float Height = 1f, float Rotation = 0, bool HideHud = true, bool Shadowing = true, BlendTypeEnum Blend = DefaultHUDBlendType)
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.BillBoardHUDMessage);

				if(BackingObject != null)
				{
					this.TimeToLive = TimeToLive;
					this.Origin = Origin;
					this.Options = Options.None;
					if (HideHud)
						this.Options |= Options.HideHud;
					if (Shadowing)
						this.Options |= Options.Shadowing;
					this.BillBoardColor = BillBoardColor;
					this.Scale = Scale;
					this.Material = Material;
					this.Rotation = Rotation;
					this.Blend = Blend;
					if (Offset.HasValue)
					{
						this.Offset = Offset.Value;
					}
					else
					{
						this.Offset = Vector2D.Zero;
					}
					this.Width = Width;
					this.Height = Height;
				}


			}

			public BillBoardHUDMessage(MyStringId Material, Vector2D Origin, Color BillBoardColor, Vector2 uvOffset, Vector2 uvSize, float TextureSize, Vector2D? Offset = null, int TimeToLive = -1, double Scale = 1d, float Width = 1f, float Height = 1f, float Rotation = 0, bool HideHud = true, bool Shadowing = true, BlendTypeEnum Blend = DefaultHUDBlendType)
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.BillBoardHUDMessage);

				if (BackingObject != null)
				{
					this.uvEnabled = true;
					this.uvOffset = uvOffset;
					this.uvSize = uvSize;
					this.TextureSize = TextureSize;
					this.TimeToLive = TimeToLive;
					this.Origin = Origin;
					this.Options = Options.None;
					if (HideHud)
						this.Options |= Options.HideHud;
					if (Shadowing)
						this.Options |= Options.Shadowing;
					this.BillBoardColor = BillBoardColor;
					this.Scale = Scale;
					this.Material = Material;
					this.Rotation = Rotation;
					this.Blend = Blend;
					if (Offset.HasValue)
					{
						this.Offset = Offset.Value;
					}
					else
					{
						this.Offset = Vector2D.Zero;
					}
					this.Width = Width;
					this.Height = Height;
				}


			}

			public BillBoardHUDMessage()
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.BillBoardHUDMessage);
			}

			public override void DeleteMessage()
			{
				instance.DeleteMessage(BackingObject);
				BackingObject = null;
			}
		}

		public class BillBoardTriHUDMessage : MessageBase
		{

			private enum EntityMembers : int
			{
				Message = 0,
				Origin = 10,
				Options,
				BillBoardColor,
				Material,
				Rotation,
				Width,
				Height,
				p0,
				p1,
				p2
			}

			#region Properties
			/// <summary>
			/// top left is -1, 1, bottom right is 1 -1
			/// </summary>
			public Vector2D Origin
			{
				get
				{
					return (Vector2D)instance.MessageGet(BackingObject, (int)EntityMembers.Origin);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Origin, value);
				}
			}

			/// <summary>
			/// Use MyStringId.GetOrCompute to turn a string into a MyStringId. 
			/// </summary>
			public MyStringId Material
			{
				get
				{
					return (MyStringId)instance.MessageGet(BackingObject, (int)EntityMembers.Material);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Material, value);
				}
			}


			/// <summary>
			/// Set Options, HideHud to true will hide billboard when hud is hidden. Shadowing will draw the element on the shadow layer (behind the text layer)
			/// </summary>
			public Options Options
			{
				get
				{
					return (Options)instance.MessageGet(BackingObject, (int)EntityMembers.Options);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Options, (byte)value);
				}
			}


			/// <summary>
			/// Sets the color mask of the billboard, not all billboards support this parameter. 
			/// </summary>
			public Color BillBoardColor
			{
				get
				{
					return (Color)instance.MessageGet(BackingObject, (int)EntityMembers.BillBoardColor);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.BillBoardColor, value);
				}
			}

			/// <summary>
			/// Rotate billboard in radians.
			/// </summary>
			public float Rotation
			{
				get
				{
					return (float)instance.MessageGet(BackingObject, (int)EntityMembers.Rotation);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Rotation, value);
				}
			}


			/// <summary>
			/// Multiplies the width of the billboard by this amount. Set Scale to 1 if you want to use this to finely control the width of the billboard, such as a value from GetTextLength
			/// You might need to multiply the result of GetTextLength by 250 or maybe 500 if Scale is 1. Will need experiementing
			/// </summary>
			public float Width
			{
				get
				{
					return (float)instance.MessageGet(BackingObject, (int)EntityMembers.Width);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Width, value);
				}
			}


			/// <summary>
			/// Multiplies the height of the billboard by this amount. Set Scale to 1 if you want to use this to finely control the height of the billboard, such as a value from GetTextLength
			/// </summary>
			public float Height
			{
				get
				{
					return (float)instance.MessageGet(BackingObject, (int)EntityMembers.Height);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Height, value);
				}
			}

			/// <summary>
			/// UV P0 (note this is percentage based between 0-1 for X,Y)
			/// </summary>
			public Vector2 P0
			{
				get
				{
					return (Vector2)instance.MessageGet(BackingObject, (int)EntityMembers.p0);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.p0, value);
				}
			}
			/// <summary>
			/// UV P1 (note this is percentage based between 0-1 for X,Y)
			/// </summary>
			public Vector2 P1
			{
				get
				{
					return (Vector2)instance.MessageGet(BackingObject, (int)EntityMembers.p1);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.p1, value);
				}
			}
			/// <summary>
			/// UV P2 (note this is percentage based between 0-1 for X,Y)
			/// </summary>
			public Vector2 P2
			{
				get
				{
					return (Vector2)instance.MessageGet(BackingObject, (int)EntityMembers.p2);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.p2, value);
				}
			}

			#endregion



			public BillBoardTriHUDMessage(MyStringId Material, Vector2D Origin, Color BillBoardColor, Vector2 p0, Vector2 p1, Vector2 p2, Vector2D? Offset = null, int TimeToLive = -1, double Scale = 1d, float Width = 1f, float Height = 1f, float Rotation = 0, bool HideHud = true, bool Shadowing = true, BlendTypeEnum Blend = DefaultHUDBlendType)
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.BillboardTriHUDMessage);

				if (BackingObject != null)
				{

					this.TimeToLive = TimeToLive;
					this.Origin = Origin;
					this.Options = Options.None;
					if (HideHud)
						this.Options |= Options.HideHud;
					if (Shadowing)
						this.Options |= Options.Shadowing;
					this.BillBoardColor = BillBoardColor;
					this.Scale = Scale;
					this.Material = Material;
					this.Rotation = Rotation;
					this.Blend = Blend;
					if (Offset.HasValue)
					{
						this.Offset = Offset.Value;
					}
					else
					{
						this.Offset = Vector2D.Zero;
					}
					this.Width = Width;
					this.Height = Height;
					this.P0 = p0;
					this.P1 = p1;
					this.P2 = p2;
				}


			}

			public BillBoardTriHUDMessage()
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.BillboardTriHUDMessage);
			}

			public override void DeleteMessage()
			{
				instance.DeleteMessage(BackingObject);
				BackingObject = null;
			}
		}

		public class SpaceMessage : MessageBase
		{
			private enum EntityMembers : int
			{
				WorldPosition = 10,
				Up,
				Left,
				TxtOrientation,
				Font

			}
			#region Properties
			/// <summary>
			/// Position
			/// </summary>
			public Vector3D WorldPosition
			{
				get
				{
					return (Vector3D)instance.MessageGet(BackingObject, (int)EntityMembers.WorldPosition);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.WorldPosition, value);
				}
			}


			/// <summary>
			/// Up vector for textures
			/// </summary>
			public Vector3D Up
			{
				get
				{
					return (Vector3D)instance.MessageGet(BackingObject, (int)EntityMembers.Up);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Up, value);
				}
			}


			/// <summary>
			/// Left Vector for Textures
			/// </summary>
			public Vector3D Left
			{
				get
				{
					return (Vector3D)instance.MessageGet(BackingObject, (int)EntityMembers.Left);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Left, value);
				}
			}
			/// <summary>
			/// Font, default is "white", "monospace" also supported, modded fonts will be supported in the future.
			/// </summary>
			public string Font
			{
				get
				{
					return (string)(instance.MessageGet(BackingObject, (int)EntityMembers.Font));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.Font, value);
				}
			}

			/// <summary>
			/// Text orientation, from what edge text is aligned.
			/// </summary>
			public TextOrientation TxtOrientation
			{
				get
				{
					return (TextOrientation)instance.MessageGet(BackingObject, (int)EntityMembers.TxtOrientation);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)EntityMembers.TxtOrientation, (byte)value);
				}
			}
			#endregion


			public SpaceMessage(StringBuilder Message, Vector3D WorldPosition, Vector3D Up, Vector3D Left, double Scale = 1, Vector2D? Offset = null, int TimeToLive = -1,  TextOrientation TxtOrientation = TextOrientation.ltr, BlendTypeEnum Blend = DefaultWorldBlendType, string Font = DefaultFont)
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.SpaceMessage);
				if(BackingObject != null)
				{
					this.TimeToLive = TimeToLive;
					this.Scale = Scale;
					this.WorldPosition = WorldPosition;
					this.Up = Up;
					this.Left = Left;
					this.TxtOrientation = TxtOrientation;
					this.Message = Message;
					this.Blend = Blend;
					if (Offset.HasValue)
					{
						this.Offset = Offset.Value;
					}
					else
					{
						this.Offset = Vector2D.Zero;
					}
					this.Font = Font;
				}

			}

			public SpaceMessage()
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.SpaceMessage);
			}

			public override void DeleteMessage()
			{
				instance.DeleteMessage(BackingObject);
				BackingObject = null;
			}
		}
		#endregion

		#region Menu
		public abstract class MenuItemBase
		{
			private enum MenuItemBaseMembers : int
			{
				Text = 0,
				Interactable
			}
			internal object BackingObject;

			/// <summary>
			/// Text displayed in the category list
			/// </summary>
			public string Text
			{
				get
				{
					return (string)(instance.MessageGet(BackingObject, (int)MenuItemBaseMembers.Text));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuItemBaseMembers.Text, value);
				}
			}
			/// <summary>
			/// User can select this item. true by default
			/// </summary>
			public bool Interactable
			{
				get
				{
					return (bool)(instance.MessageGet(BackingObject, (int)MenuItemBaseMembers.Interactable));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuItemBaseMembers.Interactable, value);
				}
			}
		}
		public class MenuItem : MenuItemBase
		{
			private enum MenuItemMembers : int
			{
				OnClickAction = 100,
				Parent
			}
			/// <summary>
			/// On click event that will be fired if the user selects this item.
			/// </summary>
			public Action OnClick
			{
				get
				{
					return (Action)(instance.MessageGet(BackingObject, (int)MenuItemMembers.OnClickAction));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuItemMembers.OnClickAction, value);
				}
			}
			/// <summary>
			/// Must be either a MenuRootCategory or MenuSubCategory object
			/// </summary>
			public MenuCategoryBase Parent
			{
				set
				{
					instance.MessageSet(BackingObject, (int)MenuItemMembers.Parent, value.BackingObject);
				}
			}
			/// <summary>
			/// Basic toggle. You can use this to create on/off toggles, checkbox lists or option lists. 
			/// </summary>
			/// <param name="Text">Text displayed in the category list</param>
			/// <param name="Parent">Must be either a MenuRootCategory or MenuSubCategory object</param>
			/// <param name="OnClick">On click event that will be fired if the user selects this item.</param>
			/// <param name="Interactable">User can select this item. true by default</param>
			public MenuItem(string Text, MenuCategoryBase Parent, Action OnClick = null, bool Interactable = true)
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.MenuItem);

				this.Text = Text;
				this.Parent = Parent;
				this.OnClick = OnClick;
				this.Interactable = Interactable;
			}
		}
		public abstract class MenuCategoryBase : MenuItemBase
		{
			private enum MenuBaseCategoryMembers : int
			{
				Header = 100
			}
			/// <summary>
			/// Header text of the menu list.
			/// </summary>
			public string Header
			{
				get
				{
					return (string)(instance.MessageGet(BackingObject, (int)MenuBaseCategoryMembers.Header));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuBaseCategoryMembers.Header, value);
				}
			}
		}
		public class MenuRootCategory : MenuCategoryBase
		{
			public enum MenuFlag : int
			{
				None = 0,
				PlayerMenu = 1,
				AdminMenu = 2
			}
			private enum MenuRootCategoryMembers : int
			{
				MenuFlag = 200
				
			}
			/// <summary>
			/// Which menu to attach to, either Player or Admin menus. 
			/// </summary>
			public MenuFlag Menu
			{
				get
				{
					return (MenuFlag)(instance.MessageGet(BackingObject, (int)MenuRootCategoryMembers.MenuFlag));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuRootCategoryMembers.MenuFlag, (int)value);
				}
			}
			/// <summary>
			/// Create only one of these per mod. Automatically attaches to parent lists. 
			/// </summary>
			/// <param name="Text">Text displayed in the root menu list</param>
			/// <param name="AttachedMenu">Which menu to attach to, either Player or Admin menus. </param>
			/// <param name="HeaderText">Header text of this menu list.</param>
			public MenuRootCategory(string Text, MenuFlag AttachedMenu = MenuFlag.None, string HeaderText = "Default Header")
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.MenuRootCategory);
				this.Text = Text;
				Header = HeaderText;
				Menu = AttachedMenu;
			}
		}
		public class MenuSubCategory : MenuCategoryBase
		{
			private enum MenuSubCategoryMembers : int
			{
				Parent = 200
			}

			/// <summary>
			/// Must be either a MenuRootCategory or MenuSubCategory objectMust be either a MenuRootCategory or MenuSubCategory object
			/// </summary>
			public MenuCategoryBase Parent
			{
				set
				{
					instance.MessageSet(BackingObject, (int)MenuSubCategoryMembers.Parent, value.BackingObject);
				}
			}

			/// <summary>
			/// Creates a sub category, must attach to either Root or another Sub Category.
			/// </summary>
			/// <param name="Text">Text displayed in the category list</param>
			/// <param name="Parent">Must be either a MenuRootCategory or MenuSubCategory objectMust be either a MenuRootCategory or MenuSubCategory object</param>
			/// <param name="HeaderText">Header text of this menu list.</param>
			public MenuSubCategory(string Text, MenuCategoryBase Parent, string HeaderText = "Default Header")
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.MenuSubCategory);
				this.Text = Text;
				this.Header = HeaderText;
				this.Parent = Parent;
			}
		}
		public class MenuColorPickerInput : MenuItemBase
		{
			private enum MenuColorPickerInputMembers : int
			{
				OnSubmitAction = 100,
				Parent,
				InputDialogTitle,
				OnUpdateAction,
				OnCancelAction,
				InitialColor,
				ShowAlpha,
				UseHSV
			}

			/// <summary>
			/// Must be either a MenuRootCategory or MenuSubCategory object
			/// </summary>
			public MenuCategoryBase Parent
			{
				set
				{
					instance.MessageSet(BackingObject, (int)MenuColorPickerInputMembers.Parent, value.BackingObject);
				}
			}

			/// <summary>
			/// Titlebar of the Dialog window. 
			/// </summary>
			public string InputDialogTitle
			{
				get
				{
					return (string)(instance.MessageGet(BackingObject, (int)MenuColorPickerInputMembers.InputDialogTitle));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuColorPickerInputMembers.InputDialogTitle, value);
				}
			}

			/// <summary>
			/// Returns inputted color on submit. 
			/// </summary>
			public Action<Color> OnSubmitAction
			{
				get
				{
					return (Action<Color>)(instance.MessageGet(BackingObject, (int)MenuColorPickerInputMembers.OnSubmitAction));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuColorPickerInputMembers.OnSubmitAction, value);
				}
			}

			/// <summary>
			/// Returns color as client is manipulating the dialog. 
			/// </summary>
			public Action<Color> OnUpdateAction
			{
				get
				{
					return (Action<Color>)(instance.MessageGet(BackingObject, (int)MenuColorPickerInputMembers.OnUpdateAction));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuColorPickerInputMembers.OnUpdateAction, value);
				}
			}

			/// <summary>
			/// Canceled the dialog 
			/// </summary>
			public Action OnCancelAction
			{
				get
				{
					return (Action)(instance.MessageGet(BackingObject, (int)MenuColorPickerInputMembers.OnCancelAction));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuColorPickerInputMembers.OnCancelAction, value);
				}
			}

			/// <summary>
			/// Initial color in the dialog box
			/// </summary>
			public Color InitialColor
			{
				get
				{
					return (Color)(instance.MessageGet(BackingObject, (int)MenuColorPickerInputMembers.InitialColor));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuColorPickerInputMembers.InitialColor, value);
				}
			}

			/// <summary>
			/// Shows alpha slider if true (default true)
			/// </summary>
			public bool ShowAlpha
			{
				get
				{
					return (bool)(instance.MessageGet(BackingObject, (int)MenuColorPickerInputMembers.ShowAlpha));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuColorPickerInputMembers.ShowAlpha, value);
				}
			}

			/// <summary>
			/// UseHSV Values
			/// </summary>
			public bool UseHSV
			{
				get
				{
					return (bool)(instance.MessageGet(BackingObject, (int)MenuColorPickerInputMembers.UseHSV));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuColorPickerInputMembers.UseHSV, value);
				}
			}

			/// <summary>
			/// Summons a dialog box that allows the user to specify a color.
			/// </summary>
			/// <param name="Text">Text displayed in the category list</param>
			/// <param name="Parent">Must be either a MenuRootCategory or MenuSubCategory object</param>
			/// <param name="initialColor">Initial color set in the dialog box</param>
			/// <param name="InputDialogTitle">Dialog Title</param>
			/// <param name="onSubmit">On Submit Callback, returns color in the dialog</param>
			/// <param name="onUpdate">Update callback, will call per tick with the current selected color in the dialog</param>
			/// <param name="onCancel">User canceled the dialog</param>
			/// <param name="showAlpha">Shows alpha slider if true</param>
			/// <param name="useHSV">Have Sliders Represent HSV Values</param>
			public MenuColorPickerInput(string Text, MenuCategoryBase Parent, Color initialColor, string InputDialogTitle = "Enter text value", Action<Color> onSubmit = null, Action<Color> onUpdate = null, Action onCancel = null, bool showAlpha = true, bool useHSV = false)
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.MenuColorPickerInput);
				this.Text = Text;
				this.InputDialogTitle = InputDialogTitle;
				this.OnSubmitAction = onSubmit;
				this.Parent = Parent;
				this.InitialColor = initialColor;
				this.OnUpdateAction = onUpdate;
				this.OnCancelAction = onCancel;
				this.ShowAlpha = showAlpha;
				this.UseHSV = useHSV;
			}
		}
		public class MenuTextInput : MenuItemBase
		{
			private enum MenuTextInputMembers : int
			{
				OnSubmitAction = 100,
				Parent,
				InputDialogTitle
			}

			/// <summary>
			/// Must be either a MenuRootCategory or MenuSubCategory object
			/// </summary>
			public MenuCategoryBase Parent
			{
				set
				{
					instance.MessageSet(BackingObject, (int)MenuTextInputMembers.Parent, value.BackingObject);
				}
			}

			/// <summary>
			/// Titlebar of the Dialog window. 
			/// </summary>
			public string InputDialogTitle
			{
				get
				{
					return (string)(instance.MessageGet(BackingObject, (int)MenuTextInputMembers.InputDialogTitle));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuTextInputMembers.InputDialogTitle, value);
				}
			}

			/// <summary>
			/// Returns inputted string on submit. 
			/// </summary>
			public Action<string> OnSubmitAction
			{
				get
				{
					return (Action<string>)(instance.MessageGet(BackingObject, (int)MenuTextInputMembers.OnSubmitAction));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuTextInputMembers.OnSubmitAction, value);
				}
			}

			/// <summary>
			/// Opens a text input dialog box when user selects this item.
			/// </summary>
			/// <param name="Text">Text displayed in the category list</param>
			/// <param name="Parent">Must be either a MenuRootCategory or MenuSubCategory object</param>
			/// <param name="InputDialogTitle">Titlebar of the Dialog window. </param>
			/// <param name="onSubmit">Returns inputted string on submit. </param>
			public MenuTextInput(string Text, MenuCategoryBase Parent, string InputDialogTitle = "Enter text value", Action<string> onSubmit = null)
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.MenuTextInput);
				this.Text = Text;
				this.InputDialogTitle = InputDialogTitle;
				this.OnSubmitAction = onSubmit;
				this.Parent = Parent;
			}
		}
		public class MenuKeybindInput : MenuItemBase
		{
			private enum MenuKeybindInputMembers : int
			{
				OnSubmitAction = 100,
				Parent,
				InputDialogTitle
			}

			/// <summary>
			/// Must be either a MenuRootCategory or MenuSubCategory object
			/// </summary>
			public MenuCategoryBase Parent
			{
				set
				{
					instance.MessageSet(BackingObject, (int)MenuKeybindInputMembers.Parent, value.BackingObject);
				}
			}

			/// <summary>
			/// Titlebar of the Dialog window. 
			/// </summary>
			public string InputDialogTitle
			{
				get
				{
					return (string)(instance.MessageGet(BackingObject, (int)MenuKeybindInputMembers.InputDialogTitle));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuKeybindInputMembers.InputDialogTitle, value);
				}
			}

			/// <summary>
			/// Called with Key pressed, Shift Pressed, Ctrl Pressed, Alt Pressed when user Submits the dialog. 
			/// </summary>
			public Action<MyKeys, bool, bool, bool> OnSubmitAction
			{
				get
				{
					return (Action<MyKeys, bool, bool, bool>)(instance.MessageGet(BackingObject, (int)MenuKeybindInputMembers.OnSubmitAction));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuKeybindInputMembers.OnSubmitAction, value);
				}
			}

			/// <summary>
			/// Opens up a keybind dialog box which lets the user submit a Key + Modifiers.
			/// </summary>
			/// <param name="Text">Text displayed in the category list</param>
			/// <param name="Parent">Must be either a MenuRootCategory or MenuSubCategory object</param>
			/// <param name="InputDialogTitle">Titlebar of the Dialog window. </param>
			/// <param name="onSubmit">Called with Key pressed, Shift Pressed, Ctrl Pressed, Alt Pressed when user Submits the dialog. </param>
			public MenuKeybindInput(string Text, MenuCategoryBase Parent, string InputDialogTitle = "Keybind - Press any key", Action<MyKeys, bool, bool, bool> onSubmit = null)
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.MenuKeybindInput);
				this.Text = Text;
				this.InputDialogTitle = InputDialogTitle;
				this.OnSubmitAction = onSubmit;
				this.Parent = Parent;
			}
		}
		public class MenuScreenInput : MenuItemBase
		{
			private enum MenuScreenInputMembers : int
			{
				OnSubmitAction = 100,
				Parent,
				InputDialogTitle,
				Origin,
				Size,
				OnUpdateAction,
				Cancel,
				OnSelect
			}

			/// <summary>
			/// Must be either a MenuRootCategory or MenuSubCategory object
			/// </summary>
			public MenuCategoryBase Parent
			{
				set
				{
					instance.MessageSet(BackingObject, (int)MenuScreenInputMembers.Parent, value.BackingObject);
				}
			}

			/// <summary>
			/// Titlebar of the Dialog window. 
			/// </summary>
			public string InputDialogTitle
			{
				get
				{
					return (string)(instance.MessageGet(BackingObject, (int)MenuScreenInputMembers.InputDialogTitle));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuScreenInputMembers.InputDialogTitle, value);
				}
			}
			/// <summary>
			/// Called when user does not click the dialog box window to move it and cancels out of the dialog box. 
			/// </summary>
			public Action OnCancel
			{
				get
				{
					return (Action)(instance.MessageGet(BackingObject, (int)MenuScreenInputMembers.Cancel));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuScreenInputMembers.Cancel, value);
				}
			}
			/// <summary>
			/// Screen position origin of the dialog box. 
			/// </summary>
			public Vector2D Origin
			{
				get
				{
					return (Vector2D)(instance.MessageGet(BackingObject, (int)MenuScreenInputMembers.Origin));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuScreenInputMembers.Origin, value);
				}
			}
			/// <summary>
			/// Size of the dialog box. Use GetTextLength() on a Hud Object to manipulate this. Or you can specify a manual width and height APIinfo can get you the width and height of a single PX.
			/// </summary>
			public Vector2D Size
			{
				get
				{
					return (Vector2D)(instance.MessageGet(BackingObject, (int)MenuScreenInputMembers.Size));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuScreenInputMembers.Size, value);
				}
			}
			/// <summary>
			/// Called when user lets go of the dialog box with the final position. Please note that the result may be off the screen. Recommend clamping between -1 and 1 on each axis. 
			/// </summary>
			public Action<Vector2D> OnSubmitAction
			{
				get
				{
					return (Action<Vector2D>)(instance.MessageGet(BackingObject, (int)MenuScreenInputMembers.OnSubmitAction));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuScreenInputMembers.OnSubmitAction, value);
				}
			}

			/// <summary>
			/// Called every tick while the user is manipulating the dialog. 
			/// </summary>
			public Action<Vector2D> UpdateAction
			{
				get
				{
					return (Action<Vector2D>)(instance.MessageGet(BackingObject, (int)MenuScreenInputMembers.OnUpdateAction));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuScreenInputMembers.OnUpdateAction, value);
				}
			}

			public Action OnSelect
			{
				get
				{
					return (Action)instance.MessageGet(BackingObject, (int)MenuScreenInputMembers.OnSelect);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuScreenInputMembers.OnSelect, value);
				}
			}

			/// <summary>
			/// Summons a dialog box that gives you a screen position when completed. 
			/// </summary>
			/// <param name="Text">Text displayed in the category list</param>
			/// <param name="Parent">Must be either a MenuRootCategory or MenuSubCategory object</param>
			/// <param name="Origin">Screen position origin of the dialog box. </param>
			/// <param name="Size">Size of the dialog box. Use GetTextLength() on a Hud Object to manipulate this. Or you can specify a manual width and height APIinfo can get you the width and height of a single PX.</param>
			/// <param name="InputDialogTitle">Titlebar of the Dialog window. </param>
			/// <param name="OnSubmit"> Called when user lets go of the dialog box with the final position. </param>
			/// <param name="Update">Called every tick while the user is manipulating the dialog. </param>
			/// <param name="Cancel">Called when user does not click the dialog box window to move it and cancels out of the dialog box.</param>
			/// <param name="OnSelect">Called when user invokes this dialog box use to refresh the Size property</param>
			public MenuScreenInput(string Text, MenuCategoryBase Parent, Vector2D Origin, Vector2D Size, string InputDialogTitle = "Move this element",  Action<Vector2D> OnSubmit = null, Action<Vector2D> Update = null, Action Cancel = null, Action OnSelect = null)
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.MenuScreenInput);
				this.Text = Text;
				this.InputDialogTitle = InputDialogTitle;
				this.OnSubmitAction = OnSubmit;
				this.UpdateAction = Update;
				this.Origin = Origin;
				this.Size = Size;
				this.OnCancel = Cancel;
				this.OnSelect = OnSelect;
				this.Parent = Parent;
				
			}
		}
		public class MenuSliderInput : MenuItemBase
		{
			private enum MenuSliderItemMembers : int
			{
				OnSubmitAction = 100,
				Parent,
				InputDialogTitle,
				InitialPercent,
				SliderPercentToValue,
				OnCancel
			}
			/// <summary>
			/// Must be either a MenuRootCategory or MenuSubCategory object
			/// </summary>
			public MenuCategoryBase Parent
			{
				set
				{
					instance.MessageSet(BackingObject, (int)MenuSliderItemMembers.Parent, value.BackingObject);
				}
			}

			/// <summary>
			/// Titlebar of the Dialog window. 
			/// </summary>
			public string InputDialogTitle
			{
				get
				{
					return (string)(instance.MessageGet(BackingObject, (int)MenuSliderItemMembers.InputDialogTitle));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuSliderItemMembers.InputDialogTitle, value);
				}
			}
			/// <summary>
			/// When the dialog box first opens set the position as a percentage based on this number. Expected value between 0 and 1. 
			/// </summary>
			public float InitialPercent
			{
				get
				{
					return (float)(instance.MessageGet(BackingObject, (int)MenuSliderItemMembers.InitialPercent));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuSliderItemMembers.InitialPercent, value);
				}
			}
			/// <summary>
			/// Percentage value of the slider when the user submits the dialog
			/// </summary>
			public Action<float> OnSubmitAction
			{
				get
				{
					return (Action<float>)(instance.MessageGet(BackingObject, (int)MenuSliderItemMembers.OnSubmitAction));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuSliderItemMembers.OnSubmitAction, value);
				}
			}
			/// <summary>
			/// Called when the user cancels the dialog window or otherwise closes the dialog box without confirming. 
			/// </summary>
			public Action OnCancel
			{
				get
				{
					return (Action)(instance.MessageGet(BackingObject, (int)MenuSliderItemMembers.OnCancel));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuSliderItemMembers.OnCancel, value);
				}
			}

			/// <summary>
			/// Returned value calls toString to print the text in the dialog box. Value fed to this function is the slider percentage value. 
			/// </summary>
			public Func<float, object> SliderPercentToValue
			{
				get
				{
					return (Func<float, object>)(instance.MessageGet(BackingObject, (int)MenuSliderItemMembers.SliderPercentToValue));
				}
				set
				{
					instance.MessageSet(BackingObject, (int)MenuSliderItemMembers.SliderPercentToValue, value);
				}
			}

			/// <summary>
			/// Creates a dialog object and adds it to the Parent list. 
			/// </summary>
			/// <param name="Text">Text displayed in the category list</param>
			/// <param name="Parent">Must be either a MenuRootCategory or MenuSubCategory object</param>
			/// <param name="InitialPercent">When the dialog box first opens set the position as a percentage based on this number. Expected value between 0 and 1.</param>
			/// <param name="InputDialogTitle">Titlebar of the Dialog window. </param>
			/// <param name="onSubmitAction">Percentage value of the slider when the user submits the dialog</param>
			/// <param name="SliderPercentToValue">Returned value calls toString to print the text in the dialog box. Value fed to this function is the slider percentage value.</param>
			/// <param name="OnCancel">Called when the user cancels the dialog window or otherwise closes the dialog box without confirming.</param>
			public MenuSliderInput(string Text, MenuCategoryBase Parent, float InitialPercent, string InputDialogTitle = "Adjust Slider to modify value", Action<float> OnSubmitAction = null, Func<float, object> SliderPercentToValue = null, Action OnCancel = null)
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.MenuSliderItem);
				this.Text = Text;
				this.InputDialogTitle = InputDialogTitle;
				this.OnSubmitAction = OnSubmitAction;
				this.SliderPercentToValue = SliderPercentToValue;
				this.InitialPercent = InitialPercent;
				this.OnCancel = OnCancel;
				this.Parent = Parent;
			}
		}
		#endregion


		/// <summary>
		/// Class to allow adding fonts to the TextHUDApi
		/// </summary>
		public class FontDefinition
		{
			enum FontDefinitionMembers : int
			{
				AddCharacter = 0,
				DefineFont,
				AddKerning,
				ReadOnly

			}

			[ProtoContract]
			public struct FontCharacterDefinitionData
			{
				[ProtoMember(1)]
				public char character;
				[ProtoMember(2)]
				public int texturesize;
				[ProtoMember(3)]
				public string charactercode;
				[ProtoMember(4)]
				public int uv1x;
				[ProtoMember(5)]
				public int uv1y;
				[ProtoMember(6)]
				public int sizex;
				[ProtoMember(7)]
				public int sizey;
				[ProtoMember(8)]
				public int aw;
				[ProtoMember(9)]
				public int lsb;
				[ProtoMember(10)]
				public bool forcewhite;
				[ProtoMember(11)]
				public MyStringId MaterialId;
			}

			public object BackingDefinition;

			public FontDefinition(object BackingObject)
			{
				BackingDefinition = BackingObject;
			}

			/// <summary>
			/// Checks to see if the object is readonly. Once the definition is read only none of its properties can be modified or character definitions replaced. New character definitions can still be added if none exist.
			/// </summary>
			public bool ReadOnly
			{
				get
				{
					return (bool)instance.MessageGet(BackingDefinition, (int)FontDefinitionMembers.ReadOnly);
				}
				set
				{
					instance.MessageSet(BackingDefinition, (int)FontDefinitionMembers.ReadOnly, value);
				}
			}

			/// <summary>
			/// Sets the global parameters for a font
			/// </summary>
			public void DefineFont(int fontbase, int lineheight, int fontsize)
			{
				MyTuple<int, int, int> data = new MyTuple<int, int, int>(fontbase, lineheight, fontsize);
				instance.MessageSet(BackingDefinition, (int)FontDefinitionMembers.DefineFont, data);
			}

			/// <summary>
			/// Adds a character glyph
			/// </summary>
			/// <param name="material">TransparentMaterial definitions subtype id</param>
			/// <param name="materialtexturesize">must be square</param>
			/// <param name="charactercode">code in hex</param>
			/// <param name="uv1x">origin x</param>
			/// <param name="uv1y">origin y</param>
			/// <param name="sizex">size x</param>
			/// <param name="sizey">size y</param>
			/// <param name="aw">advance width</param>
			/// <param name="lsb">left side bearing</param>
			/// <param name="forcewhite">force character to grayscale in render</param>
			public void AddCharacter(char character, MyStringId material, int materialtexturesize, string charactercode, int uv1x, int uv1y, int sizex, int sizey, int aw, int lsb, bool forcewhite = false)
			{
				var data = new FontCharacterDefinitionData { character = character, MaterialId = material, texturesize = materialtexturesize, charactercode = charactercode, uv1x = uv1x, uv1y = uv1y, sizex = sizex, sizey = sizey, aw = aw, lsb = lsb, forcewhite = forcewhite };
				instance.MessageSet(BackingDefinition, (int)FontDefinitionMembers.AddCharacter, MyAPIGateway.Utilities.SerializeToBinary(data));
			}

			/// <summary>
			/// Sets the kerning parameters. Right character must be defined first!
			/// </summary>
			/// <param name="adjust">how many pixels to move the right char</param>
			/// <param name="right">char that will be moved by the kerning</param>
			/// <param name="left"></param>
			public void AddKerning(int adjust, char right, char left)
			{
				MyTuple<int, char, char> data = new MyTuple<int, char, char>(adjust, right, left);
				instance.MessageSet(BackingDefinition, (int)FontDefinitionMembers.AddKerning, data);
			}

		}


		#region BoxUI
		/// <summary>
		/// Creates a new BoxUIDefinition. This defines exactly how the UI texture is laid out on the screen. 
		/// </summary>
		public class BoxUIDefinition
		{

			public object BackingDefinition;
			[ProtoContract]
			public struct BoxUIDefinitionData
			{
				[ProtoMember(1)]
				public MyStringId Material;
				[ProtoMember(2)]
				public int imagesize;
				[ProtoMember(3)]
				public int topwidthpx;
				[ProtoMember(4)]
				public int leftwidthpx;
				[ProtoMember(5)]
				public int bottomwidthpx;
				[ProtoMember(6)]
				public int rightwidthpx;
				[ProtoMember(7)]
				public int margin;
				[ProtoMember(8)]
				public int padding;
            }

			public BoxUIDefinitionData BoxUIDef
			{
				set
				{
					instance.MessageSet(BackingDefinition, (int)BoxUIDefinitionMembers.Definition, MyAPIGateway.Utilities.SerializeToBinary(value));
				}
			}

			/// <summary>
			/// Returns the margin + padding + border values. subtract this from the total size to get the size of the content area of the object. 
			/// </summary>
			public Vector2I Min
			{
				get
				{
					return (Vector2I)instance.MessageGet(BackingDefinition, (int)BoxUIDefinitionMembers.Min);
				}
			}

			enum BoxUIDefinitionMembers : int
			{
				Definition = 0,
				Min
			}

						

			public BoxUIDefinition()
			{
				BackingDefinition = instance.CreateMessage(MessageTypes.UIDefinition);
			}
			public BoxUIDefinition(MyStringId Material, int imagesize, int topwidthpx, int leftwidthpx, int bottomwidthpx, int rightwidthpx, int margin = 0, int padding = 0)
			{
				BackingDefinition = instance.CreateMessage(MessageTypes.UIDefinition);
				var data = new BoxUIDefinitionData()
					{
						Material = Material,
						imagesize = imagesize,
						topwidthpx = topwidthpx,
						leftwidthpx = leftwidthpx,
						bottomwidthpx = bottomwidthpx,
						rightwidthpx = rightwidthpx,
						margin = margin,
						padding = padding
					};
				BoxUIDef = data;

            }
			public BoxUIDefinition(object BackingObject)
			{
				BackingDefinition = BackingObject;
			}
		}

		
		/// <summary>
		/// Unused at the moment, but will be expanded on in the future. 
		/// </summary>
		public class BoxUIBehaviourDef
		{
			public object BackingDefinition;

			public BoxUIBehaviourDef()
			{
				BackingDefinition = instance.CreateMessage(MessageTypes.UIBehaviourDefinition);
			}
			public BoxUIBehaviourDef(object BackingObject)
			{
				BackingDefinition = BackingObject;
			}
		}

		public abstract class BoxUIBase
		{
			internal object BackingObject;
			internal BoxUIBase m_Parent;
			private enum BoxUIBaseMembers : int
			{
				Origin = 0,
				BackgroundColor,
				Width,
				Height,
				Definition,
				Behaviour,
				Visible,
				Parent,
				HideHud
			}

			/// <summary>
			/// Sets the BoxUI's position in PX values. 
			/// </summary>
			public Vector2I Origin
			{
				get
				{
					return (Vector2I)instance.MessageGet(BackingObject, (int)BoxUIBaseMembers.Origin);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)BoxUIBaseMembers.Origin, value);
				}
			}

			/// <summary>
			/// Sets the width of the box in PX values
			/// </summary>
			public int Width
			{
				get
				{
					return (int)instance.MessageGet(BackingObject, (int)BoxUIBaseMembers.Width);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)BoxUIBaseMembers.Width, value);
				}
			}

			/// <summary>
			/// Sets the Height in PX values
			/// </summary>
			public int Height
			{
				get
				{
					return (int)instance.MessageGet(BackingObject, (int)BoxUIBaseMembers.Height);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)BoxUIBaseMembers.Height, value);
				}
			}

			/// <summary>
			/// Sets the background color, default White
			/// </summary>
			public Color BackgroundColor
			{
				get
				{
					return (Color)instance.MessageGet(BackingObject, (int)BoxUIBaseMembers.BackgroundColor);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)BoxUIBaseMembers.BackgroundColor, value);
				}
			}
			/// <summary>
			/// Element and subelements visible
			/// </summary>
			public bool Visible
			{
				get
				{
					return (bool)instance.MessageGet(BackingObject, (int)BoxUIBaseMembers.Visible);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)BoxUIBaseMembers.Visible, value);
				}
			}

			/// <summary>
			/// Sets the backing UI definition, please set using SetDefinition
			/// </summary>
			public object DefinitionObject
			{
				get
				{
					return (object)instance.MessageGet(BackingObject, (int)BoxUIBaseMembers.Definition);
				}
				set
				{
					if(value is BoxUIDefinition)
					{
						instance.MessageSet(BackingObject, (int)BoxUIBaseMembers.Definition, (value as BoxUIDefinition).BackingDefinition);
						return;
					}
					instance.MessageSet(BackingObject, (int)BoxUIBaseMembers.Definition, value);
				}
			}

			/// <summary>
			/// Sets the backing behaviour object, please set using SetBehaviour
			/// </summary>
			public object BehaviourObject
			{
				get
				{
					return (object)instance.MessageGet(BackingObject, (int)BoxUIBaseMembers.Behaviour);
				}
				set
				{
					if(value is BoxUIBehaviourDef)
					{
						instance.MessageSet(BackingObject, (int)BoxUIBaseMembers.Behaviour, (value as BoxUIDefinition).BackingDefinition);
						return;
					}
						
					instance.MessageSet(BackingObject, (int)BoxUIBaseMembers.Behaviour, value);
				}
			}

			/// <summary>
			/// Defaults to true. 
			/// </summary>
			public bool HideHud
			{
				get
				{
					return (bool)instance.MessageGet(BackingObject, (int)BoxUIBaseMembers.HideHud);
				}
				set
				{
					instance.MessageSet(BackingObject, (int)BoxUIBaseMembers.HideHud, value);
				}
			}
			
			/// <summary>
			/// Gets or sets the parent object, please be careful not to create a circular reference. Sub objects are automatically offset by the top left corner of the parent object. 
			/// </summary>
			public BoxUIBase Parent
			{
				get
				{
					return m_Parent;

				}
				set
				{
					m_Parent = value;
					instance.MessageSet(BackingObject, (int)BoxUIBaseMembers.Parent, m_Parent.BackingObject);
				}
			}


			public void SetDefinition(BoxUIDefinition def)
			{
				DefinitionObject = def.BackingDefinition;
			}

			public void SetBehaviour(BoxUIBehaviourDef def)
			{
				BehaviourObject = def.BackingDefinition;
			}
			
		}
		public class BoxUIContainer : BoxUIBase
		{


			public BoxUIContainer()
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.BoxUIContainer);
			}
		}
		public class BoxUIText : BoxUIBase
		{

			enum BoxUITextMembers : int
			{
				SetTextContent = 100
			}

			public BoxUIText()
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.BoxUIText);
			}

			/// <summary>
			/// Automatically sets the message to use the pixel offset types. Please note that the BoxUI will control the .Origin of the Message, you can use the offset value in PX to move it. Please note Scale is now the pt size of the font. 
			/// </summary>
			/// <param name="Message"></param>
			public void SetTextContent(HUDMessage Message)
			{
				instance.MessageSet(BackingObject, (int)BoxUITextMembers.SetTextContent, Message.BackingObject);
			}
		}

		public class BoxUIImage : BoxUIBase
		{
			enum BoxUIImageMembers : int
			{
				SetImageContent = 100
			}

			public BoxUIImage()
			{
				instance.RegisterCheck();
				BackingObject = instance.CreateMessage(MessageTypes.BoxUIImage);
			}

			/// <summary>
			/// Sets the image, please note that image parameters will now be forced to the Pixel setting. Height and Width parameters are measured in px
			/// </summary>
			/// <param name="Message"></param>
			public void SetImageContent(BillBoardHUDMessage Message)
			{
				instance.MessageSet(BackingObject, (int)BoxUIImageMembers.SetImageContent, Message.BackingObject);
			}
		}
		#endregion
	}
}