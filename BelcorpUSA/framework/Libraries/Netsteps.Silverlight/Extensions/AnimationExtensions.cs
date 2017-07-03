using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace NetSteps.Silverlight.Extensions
{
    /// <summary>
    /// Contains various extension methods for use in Silverlight applications.
    /// </summary>
    public static class AnimationExtensions
    {
        /// <summary>
        /// This extension method is used to find a Storyboard declared inside of the VisualStateManager.
        /// Returns null if the storyboard cannot be found.
        /// </summary>
        /// <param name="parent">The parent object containing the VisualStateManager declaration.</param>
        /// <param name="groupName">The group name containing the storyboard.</param>
        /// <param name="stateName">The name of the state or storyboard.</param>
        /// <returns></returns>
        public static Storyboard FindStoryboard(this FrameworkElement parent, string groupName, string stateName)
        {
            var vsgs = VisualStateManager.GetVisualStateGroups(parent);
            foreach (VisualStateGroup vsg in vsgs)
            {
                if (vsg.Name != groupName)
                    continue;
                foreach (VisualState vs in vsg.States)
                {
                    if (vs.Name == stateName)
                        return vs.Storyboard;
                }
            }
            return null;
        }

        /// <summary>
        /// Extension method to quickly animate a DependencyObject.  Simply provide the propertyPath and a timeline.
        /// </summary>
        /// <param name="element">The element to animate.</param>
        /// <param name="propertyPath">The property to animate.</param>
        /// <param name="timeline">The animation to apply to the element.</param>
        /// <param name="completed">The event handler to fire once the animation completes.</param>
        public static Storyboard Animate(this DependencyObject element, string propertyPath, Timeline timeline, EventHandler completed)
        {
            Storyboard sb = new Storyboard();
            if (completed != null)
                sb.Completed += completed;

            sb.Children.Add(timeline);
            Storyboard.SetTarget(sb, element);
            Storyboard.SetTargetProperty(sb, new PropertyPath(propertyPath));
            sb.Begin();

            return sb;
        }

        /// <summary>
        /// Extension method to quickly animate a DependencyObject.  Simply provide the propertyPath and a timeline.
        /// </summary>
        /// <param name="element">The element to animate.</param>
        /// <param name="propertyPath">The property to animate.</param>
        /// <param name="timeline">The animation to apply to the element.</param>
        public static void Animate(this DependencyObject element, string propertyPath, Timeline timeline)
        {
            Animate(element, propertyPath, timeline, null);
        }


        /// <summary>
        /// Extension method to quickly animate a DependencyObject with a DoubleAnimation.  Simply provide the propertyPath, toValue, and handler to fire once the animation completes.
        /// </summary>
        /// <param name="element">The element to animate.</param>
        /// <param name="propertyPath">The property to animate.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="toValue">The value to animate to.</param>
        /// <param name="completed">The event to fire once the animation completes.</param>
        public static Storyboard AnimateDouble(this DependencyObject element, string propertyPath, int duration, double toValue, EventHandler completed)
        {
            DoubleAnimation ani = new DoubleAnimation();
            ani.To = toValue;
            ani.Duration = TimeSpan.FromMilliseconds(duration);
            return Animate(element, propertyPath, ani, completed);
        }

        /// <summary>
        /// Extension method to quickly animate a DependencyObject with a DoubleAnimation.  Simply provide the propertyPath, toValue, and handler to fire once the animation completes.
        /// </summary>
        /// <param name="element">The element to animate.</param>
        /// <param name="propertyPath">The property to animate.</param>
        /// <param name="duration">The duration of the animation.</param>
        /// <param name="toValue">The value to animate to.</param>
        public static Storyboard AnimateDouble(this DependencyObject element, string propertyPath, int duration, double toValue)
        {
            return AnimateDouble(element, propertyPath, duration, toValue, null);
        }

        public static Storyboard AnimateDoubleTo(this UIElement element, int miliseconds, string propertyPath, double startValue, double endValue, EventHandler completed)
        {
            // Create a new storyboard
            var sb = new Storyboard();

            // Create a double animation
            var da = new DoubleAnimation();

            da.Duration = new TimeSpan(0, 0, 0, 0, miliseconds);

            // Set target property and target
            Storyboard.SetTargetProperty(da, new PropertyPath(propertyPath));

            Storyboard.SetTarget(da, element);

            da.From = startValue;
            da.To = endValue;

            // Add the doubleanimation to the storyboard
            sb.Children.Add(da);

            // Add the storyboard to the Rootvisual of the application
            ((FrameworkElement)Application.Current.RootVisual).Resources.Add(Guid.NewGuid().ToString(), sb);

            // Begin the animation
            sb.Begin();

            if (completed != null)
                sb.Completed += completed;

            return sb;
        }

        public static void AnimateScaleTo(this TranslateTransform translateTransform, int milliseconds, double x, double y, EventHandler completed)
        {
            var sb = new Storyboard();
            var daX = new DoubleAnimation();

            daX.Duration = new TimeSpan(0, 0, 0, 0, milliseconds);
            Storyboard.SetTargetProperty(daX, new PropertyPath("(TranslateTransform.X)"));
            Storyboard.SetTarget(daX, translateTransform);
            daX.To = x;

            var daY = new DoubleAnimation();
            daY.Duration = new TimeSpan(0, 0, 0, 0, milliseconds);
            Storyboard.SetTargetProperty(daY, new PropertyPath("(TranslateTransform.Y)"));
            Storyboard.SetTarget(daY, translateTransform);
            daY.To = y;

            sb.Children.Add(daX);
            sb.Children.Add(daY);

            ((FrameworkElement)Application.Current.RootVisual).Resources.Add(Guid.NewGuid().ToString(), sb);

            sb.Begin();

            if (completed != null)
                sb.Completed += completed;
        }



    }
}
