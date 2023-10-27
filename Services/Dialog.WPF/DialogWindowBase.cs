using System.Threading.Tasks;
using System.Windows;

namespace EllipticBit.Services.Dialog.WPF
{
	public abstract class DialogWindowBase : System.Windows.Controls.ContentControl
	{
		public string Title { get { return (string)GetValue(TitleProperty); } protected set { SetValue(TitleProperty, value); } }
		public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(DialogWindowBase), new PropertyMetadata());

		public GridLength ContentMargin { get { return (GridLength)GetValue(ContentMarginProperty); } set { SetValue(ContentMarginProperty, value); } }
		public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register(nameof(ContentMargin), typeof(GridLength), typeof(DialogWindowBase), new PropertyMetadata());

		private readonly TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

		internal DialogHost Host { get; set; }

		protected DialogWindowBase() {
			this.Title = string.Empty;
			this.ContentMargin = new GridLength(20.0);
		}

		protected DialogWindowBase(string title) {
			this.Title = title;
			this.ContentMargin = new GridLength(20.0);
		}

		internal Task<object> WaitForCloseAsync() {
			this.Dispatcher.VerifyAccess();
			return tcs.Task;
		}

		public void CloseDialog() {
			this.Dispatcher.VerifyAccess();
			Host.SetDialog(null);
			tcs.SetResult(null);
		}

		public void CloseDialog<T>(T result) where T : class {
			this.Dispatcher.VerifyAccess();
			Host.SetDialog(null);
			tcs.SetResult(result);
		}

		public void CloseDialog(DialogResult result) {
			this.Dispatcher.VerifyAccess();
			Host.SetDialog(null);
			tcs.SetResult(result);
		}
	}
}
