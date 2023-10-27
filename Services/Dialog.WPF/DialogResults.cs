namespace EllipticBit.Services.Dialog.WPF
{
	public enum DialogResult
	{
		Affirmative,
		Negative,
		Canceled,
	}

	public class LoginDialogResult
	{
		public DialogResult Result { get; }
		public string Username { get; }
		public string Password { get; }
		public bool Remember { get; }

		internal LoginDialogResult(string username, string password, bool remember) {
			this.Result = DialogResult.Affirmative;
			this.Username = username;
			this.Password = password;
			this.Remember = remember;
		}

		internal LoginDialogResult(DialogResult result) {
			this.Result = result;
		}
	}
}
