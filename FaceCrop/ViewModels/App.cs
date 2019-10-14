using System;
using MvvmCross.IoC;
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

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsSingleton();
        }
    }
}
