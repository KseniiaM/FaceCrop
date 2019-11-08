using System;
using FaceCrop.CustomControls;
using Xamarin.Forms;

namespace FaceCrop.Behaviors
{
    public class CustomImageViewRefreshBehavior : Behavior<CustomImageView>
    {
        public event Action RefreshSelectionEventHandler;

        public static readonly BindableProperty RefreshButtonPressedProperty = BindableProperty.Create(
            "RefreshButtonEvent",
            typeof(bool),
            typeof(CustomImageViewRefreshBehavior),
            propertyChanged: RefreshButtonPressedChanged);

        public bool RefreshButtonPressed
        {
            get { return (bool)GetValue(RefreshButtonPressedProperty); }
            set { SetValue(RefreshButtonPressedProperty, value); }
        }

        private static void RefreshButtonPressedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CustomImageViewRefreshBehavior behavior && (bool)newValue)
            {
                behavior.RefreshSelectionEventHandler.Invoke();
            }
        }
    }
}
