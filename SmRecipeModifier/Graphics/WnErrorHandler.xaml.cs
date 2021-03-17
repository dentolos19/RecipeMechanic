using System;
using System.Windows;
using SmRecipeModifier.Core;

namespace SmRecipeModifier.Graphics
{

    public partial class WnErrorHandler
    {

        public WnErrorHandler(Exception error)
        {
            InitializeComponent();
            MessageText.Text = error.Message;
            StackTraceText.Text = error.StackTrace ?? "The error info doesn't contain stack trace data.";
        }

        private void Restart(object sender, RoutedEventArgs args)
        {
            Utilities.RestartApp();
        }

        private void Exit(object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

    }

}