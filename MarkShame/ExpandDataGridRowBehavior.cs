using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace MarkShame
{
    public static class DataGridRowBehavior
    {
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.RegisterAttached("IsExpanded", typeof(bool), typeof(DataGridRowBehavior),
                new PropertyMetadata(false, OnIsExpandedChanged));

        public static bool GetIsExpanded(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsExpandedProperty);
        }

        public static void SetIsExpanded(DependencyObject obj, bool value)
        {
            obj.SetValue(IsExpandedProperty, value);
        }

        private static void OnIsExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DataGridRow row)
            {
                row.IsSelected = (bool)e.NewValue;
            }
        }
    }

}
