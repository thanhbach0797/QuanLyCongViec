using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace TaskManagementApp.Helpers
{
    public class PasswordBoxBindingBehavior : Behavior<PasswordBox>
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordBoxBindingBehavior),
                new PropertyMetadata(string.Empty, OnPasswordChanged));

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.PasswordChanged -= PasswordBox_PasswordChanged;
            }
            base.OnDetaching();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject != null)
            {
                Password = AssociatedObject.Password; // Đảm bảo cập nhật ngay lập tức
            }
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behavior = (PasswordBoxBindingBehavior)d;
            if (behavior.AssociatedObject != null && behavior.AssociatedObject.Password != (string)e.NewValue)
            {
                behavior.AssociatedObject.Password = (string)e.NewValue ?? string.Empty; // Xử lý null
            }
        }
    }
}