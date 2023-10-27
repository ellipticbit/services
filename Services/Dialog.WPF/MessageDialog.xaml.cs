using System.Windows;

namespace EllipticBit.Services.Dialog.WPF
{
	internal partial class MessageDialog : DialogWindowBase
	{
		internal MessageDialog(string title, string message, DialogButtons buttons, MessageDialogSettings dialogSettings) : base(title) {
			InitializeComponent();

			this.FontSize = dialogSettings.TextFontSize;
			TitleContent.FontSize = dialogSettings.TitleFontSize;
			MessageContent.Content = message;
			MessageContent.FontSize = dialogSettings.TextFontSize;

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
		}

		private void Affirmative_Click(object sender, RoutedEventArgs e) {
			this.CloseDialog(DialogResult.Affirmative);
		}

		private void AltAffirmative_Click(object sender, RoutedEventArgs e) {
			this.CloseDialog(DialogResult.Affirmative);
		}

		private void Negative_Click(object sender, RoutedEventArgs e) {
			this.CloseDialog(DialogResult.Negative);
		}

		private void AltNegative_Click(object sender, RoutedEventArgs e) {
			this.CloseDialog(DialogResult.Negative);
		}

		private void Cancel_Click(object sender, RoutedEventArgs e) {
			this.CloseDialog(DialogResult.Canceled);
		}
	}
}
