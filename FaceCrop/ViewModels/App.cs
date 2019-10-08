using System;
using MvvmCross.ViewModels;
using ViewModels.ViewModels;

namespace ViewModels
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            RegisterAppStart<StartViewModel>();
        }
    }
}
