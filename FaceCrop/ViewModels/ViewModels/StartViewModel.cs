using MvvmCross.ViewModels;

namespace ViewModels.ViewModels
{
    public class StartViewModel : MvxViewModel
    {
        public override void ViewAppearing()
        {
            base.ViewAppearing();
        }

        public string WelcomeText => "Hello Xamarin.Forms with MvvmCross";
    }
}
