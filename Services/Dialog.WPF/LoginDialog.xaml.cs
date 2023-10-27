using System.Windows;

namespace EllipticBit.Services.Dialog.WPF
{
	internal partial class LoginDialog : DialogWindowBase
	{
		internal LoginDialog(string title, DialogButtons buttons, LoginDialogSettings dialogSettings) : base(title) {
			InitializeComponent();

			Username.CharacterCasing = dialogSettings.UsernameCharacterCasing;
			Username.Text = dialogSettings.InitialUsername;
			Password.Password = dialogSettings.InitialPassword;
			TitleContent.FontSize = dialogSettings.TitleFontSize;
			this.FontSize = dialogSettings.TextFontSize;

			Affirmative.Content = dialogSettings.AffirmativeText;
			Affirmative.FontSize = dialogSettings.ButtonFontSize;
			AltAffirmative.Content = dialogSettings.AlternateAffirmativeText;
			AltAffirmative.FontSize = dialogSettings.ButtonFontSize;
			Negative.Content = dialogSettings.NegativeText;
			Negative.FontSize = dialogSettings.ButtonFontSize;
			AltNegative.Content = dialogSettings.AlternateNegativeText;
			AltNegative.FontSize = dialogSettings.ButtonFontSize;
			Cancel.Content = dialogSettings.CancelText;
			Cancel.FontSize = dialogSettings.ButtonFontSize;

			if (buttons.HasFlag(DialogButtons.Affirmative)) Affirmative.Visibility = Visibility.Visible;
			if (buttons.HasFlag(DialogButtons.AlternateAffirmative)) AltAffirmative.Visibility = Visibility.Visible;
			if (buttons.HasFlag(DialogButtons.Negative)) Negative.Visibility = Visibility.Visible;
			if (buttons.HasFlag(DialogButtons.AlternateNegative)) AltNegative.Visibility = Visibility.Visible;
			if (buttons.HasFlag(DialogButtons.Cancel)) Cancel.Visibility = Visibility.Visible;

			RememberLogin.IsChecked = dialogSettings.RememberCheckBoxChecked;
			RememberLogin.Content = dialogSettings.RememberCheckBoxText;
			RememberLogin.Visibility = dialogSettings.RememberCheckBoxVisibility;
		}

		private void Affirmative_Click(object sender, RoutedEventArgs e) {
			this.CloseDialog(new LoginDialogResult(Username.Text, Password.Password, RememberLogin.IsChecked.Value));
		}

		private void AltAffirmative_Click(object sender, RoutedEventArgs e) {
			this.CloseDialog(new LoginDialogResult(Username.Text, Password.Password, RememberLogin.IsChecked.Value));
		}

		private void Negative_Click(object sender, RoutedEventArgs e) {
			this.CloseDialog(new LoginDialogResult(DialogResult.Negative));
		}

		private void AltNegative_Click(object sender, RoutedEventArgs e) {
			this.CloseDialog(new LoginDialogResult(DialogResult.Negative));
		}

		private void Cancel_Click(object sender, RoutedEventArgs e) {
			this.CloseDialog(new LoginDialogResult(DialogResult.Canceled));
		}
	}
}
