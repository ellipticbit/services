using System;
using System.Windows;
using System.Windows.Controls;

namespace EllipticBit.Services.Dialog.WPF
{
	[Flags]
	public enum DialogButtons
	{
		Default = Affirmative | Negative,
		Affirmative = 1,
		AlternateAffirmative = 2,
		Negative = 4,
		AlternateNegative = 8,
		Cancel = 16
	}

	public class MessageDialogSettings
	{
		public string DefaultTitle { get; set; }
		public double TitleFontSize { get; set; } = 20;
		public double TextFontSize { get; set; } = 16;
		public double ButtonFontSize { get; set; } = 14;
		public double MaximumBodyHeight { get; set; } = -1;
		public string AffirmativeText { get; set; } = "OK";
		public string AlternateAffirmativeText { get; set; } = "Yes";
		public string NegativeText { get; set; } = "Cancel";
		public string AlternateNegativeText { get; set; } = "No";
		public string CancelText { get; set; } = "Cancel";
	}

	public class LoginDialogSettings : MessageDialogSettings
	{
		public bool HideUsername { get; set; }
		//public bool EnablePasswordPreview { get; set; }
		public string InitialPassword { get; set; }
		public string InitialUsername { get; set; }
		//public string PasswordWatermark { get; set; }
		//public string UsernameWatermark { get; set; }
		public CharacterCasing UsernameCharacterCasing { get; set; }

		public bool RememberCheckBoxChecked { get; set; }
		public string RememberCheckBoxText { get; set; }
		public Visibility RememberCheckBoxVisibility { get; set; }

		public LoginDialogSettings() {
			AffirmativeText = "Login";
			AlternateAffirmativeText = "Sign In";
			AlternateNegativeText = "Exit";

			HideUsername = false;
			UsernameCharacterCasing = CharacterCasing.Normal;
			RememberCheckBoxChecked = false;
			RememberCheckBoxText = "Remember Credentials";
			RememberCheckBoxVisibility = Visibility.Visible;
		}
	}
}
