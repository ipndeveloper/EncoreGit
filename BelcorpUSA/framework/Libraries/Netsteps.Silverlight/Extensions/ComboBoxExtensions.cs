using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetSteps.Silverlight.Extensions
{
    public static class ComboBoxExtensions
    {
        #region FillComboBoxWithEnums
        public enum EnumProperty
        {
            Name,
            Value
        }

        public static void FillEnums<T>(this ComboBox comboBox)
        {
            FillEnums<T>(comboBox, string.Empty, EnumProperty.Name);
        }
        public static void FillEnums<T>(this ComboBox comboBox, EnumProperty enumProperty)
        {
            FillEnums<T>(comboBox, string.Empty, enumProperty);
        }
        public static void FillEnums<T>(this ComboBox comboBox, string defaultValue, EnumProperty enumProperty)
        {
            IList<EnumNameValue> list = EnumExtensions.GetValuesList<T>();

            comboBox.DisplayMemberPath = enumProperty.ToString();
            comboBox.ItemsSource = list;
        }
        #endregion

        #region Conversion Methods
        public static DateTime? ToDateTime(this ComboBox comboBox, DateTime? dateToSetTimeOn)
        {
            return dateToSetTimeOn.SetTime(comboBox.SelectedItem.ToString());
        }

        public static T ToEnum<T>(this ComboBox comboBox)
        {
            return ToEnum<T>(comboBox, default(T));
        }
        public static T ToEnum<T>(this ComboBox comboBox, T defaultValue)
        {
            if (comboBox.SelectedItem == null)
                return defaultValue;
            else
            {
                Type objectType = typeof(T);
                return (T)Enum.Parse(objectType, comboBox.SelectedItem.ToString().RemoveSpaces(), true);
            }
        }
        #endregion

        #region SetSelectedValue
        /// <summary>
        /// This object selectedValue parameter is for Enums - JHE
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="selectedValue"></param>
        public static void SetSelectedValue(this ComboBox comboBox, object selectedValue)
        {
            SetSelectedValue(comboBox, selectedValue.ToString().PascalToSpaced());
        }
        public static void SetSelectedValueDesc(this ComboBox comboBox, object selectedValue)
        {
            SetSelectedValueDesc(comboBox, selectedValue.ToString());
        }
        public static void SetSelectedValue(this ComboBox comboBox, int selectedValue)
        {
            SetSelectedValue(comboBox, selectedValue.ToString());
        }
        public static void SetSelectedValue(this ComboBox comboBox, string selectedValue)
        {
            try
            {
                int selectedIndex = 0;
                foreach (EnumNameValue value in comboBox.Items)
                {
                    if (value.Name == selectedValue)
                        break;

                    selectedIndex++;
                }
                comboBox.SelectedIndex = selectedIndex;
            }
            catch { }
        }

        public static void SetSelectedValueDesc(this ComboBox comboBox, string selectedValue)
        {
            try
            {
                //int selectedIndex = 0;
                foreach (DescriptionValue value in comboBox.Items)
                {
                    if (value.Value.ToString() == selectedValue)
                    {
                        comboBox.SelectedItem = value;
                        return;
                    }
                    comboBox.SelectedIndex = 0;
                    //selectedIndex++;
                }
                //comboBox.SelectedIndex = selectedIndex;
                //comboBox.UpdateLayout();
            }
            catch (Exception ex)
            { }
        }

        #endregion


        public static void SetSelectedIndexSafe(this ComboBox comboBox, int index)
        {
            try
            {
                comboBox.SelectedIndex = index;
            }
            catch { }
        }


        public static void FillTimes(this ComboBox comboBox)
        {
            DateTime time = new DateTime(2008, 1, 1, 0, 0, 0);
            DateTime endTime = time.AddDays(1);
            List<string> times = new List<string>();
            while (time < endTime)
            {
                times.Add(time.ToString("h:mm tt"));
                time = time.AddMinutes(15);
            }

            comboBox.PopulateComboBox<string>(times, x => x, x => x);
        }

        public class DescriptionValue
        {
            public object Description { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Description.ToString();
            }
        }

        public delegate object SetDescription<T>(T param);
        public delegate object SetValue<T>(T param);

        public static void PopulateComboBox<T>(this ComboBox comboBox, IEnumerable<T> originalList, SetDescription<T> name, SetValue<T> value)
        {
            List<DescriptionValue> list = new List<DescriptionValue>();

            foreach (T o in originalList)
            {
                DescriptionValue descriptionValue = new DescriptionValue();
                descriptionValue.Description = name(o);
                descriptionValue.Value = value(o);

                list.Add(descriptionValue);
            }

            comboBox.DisplayMemberPath = "Description";
            comboBox.ItemsSource = list;
        }


        public static void UpdateComboBox<T>(this ComboBox comboBox, IEnumerable<T> originalList, SetDescription<T> name, SetValue<T> value)
        {
            List<DescriptionValue> list = (List<DescriptionValue>)(comboBox.ItemsSource);
            int i = 0;
            foreach (T o in originalList)
            {
                list[i].Value = value(o);
                list[i].Description = name(o);
                ++i;
            }
        }

        public static void PopulateComboBoxSelect<T>(this ComboBox comboBox, IEnumerable<T> originalList, SetDescription<T> name, SetValue<T> value, PropertyInfo propertyInfo)
        {
            //T is the type of objects from which the values in the combo box are taken e.g. AccountListValueModel
            //name is a function to be called to extract the diplay value from one of the objects for a combobox entry
            //value is a function to call to extract from one of the objects the selection return value associated with a combobox entry
            //propertyInfo 
            // example call:
            //    uxTaskCategory.PopulateComboBoxSelect<AccountListValueModel>(
            //AccountListValueController.Instance.Collection(AccountListValue.eListValueType.TaskCategory)
            //, (alvm) => { return alvm.Value; }, (alvm) => { return alvm.AccountListValueID; }, typeof(AccountTaskModel).GetProperty("CategoryID"));

            comboBox.PopulateComboBox<T>(originalList, name, value);

            if (propertyInfo != null)
            {
                if (comboBox.DataContext != null)
                {
                    string sss = propertyInfo.GetValue(comboBox.DataContext, null).ToString();
                    comboBox.SetSelectedValueDesc(propertyInfo.GetValue(comboBox.DataContext, null));
                }

                //comboBox.SelectionChanged += new SelectionChangedEventHandler(
                //    (o, e) =>
                //    {
                //        if (comboBox.DataContext != null && comboBox.SelectedItem != null)
                //            propertyInfo.SetValue(comboBox.DataContext, propertyInfo.GetValue(comboBox.SelectedItem, null), true), null);
                //    });
            }
        }



        public static void SelectByValue(this ComboBox comboBox, object value)
        {
            try
            {
                foreach (DescriptionValue item in comboBox.Items)
                {
                    if (string.Compare(item.Value.ToString(), value.ToString(), StringComparison.CurrentCultureIgnoreCase) == 0)
                    {
                        comboBox.SelectedItem = item;
                        return;
                    }
                }
            }
            catch { }
        }


        public static void SelectByContentValue(this ComboBox comboBox, string value)
        {
            try
            {
                foreach (System.Windows.Controls.ComboBoxItem item in comboBox.Items)
                {
                    if (item.Content.ToString() == value)
                    {
                        comboBox.SelectedItem = item;
                        return;
                    }
                }
            }
            catch { }
        }

        public static object SelectedValue(this ComboBox comboBox)
        {
            return ((DescriptionValue)comboBox.SelectedItem).Value;
        }


        public static void SetSolidForegroundColor(this ComboBox comboBox, string color)
        {
            Color ForegroundColor = new Color();
            ForegroundColor = ForegroundColor.Parse(color);
            comboBox.Foreground = new SolidColorBrush(ForegroundColor);
        }

        public static void SetSolidBackgroundColor(this ComboBox comboBox, string color, bool changeForegroundToContrast)
        {
            //try
            //{
            //    Color backgroundColor = new Color();
            //    backgroundColor = backgroundColor.Parse(color);
            //    comboBox.Background = new SolidColorBrush(backgroundColor);

            //    if (changeForegroundToContrast)
            //    {
            //        List<string> backgroundColors = "NotSet, Black, Green, Blue, Brown, Purple".ToStringList();
            //        if (backgroundColors.ContainsIgnoreCase(color))
            //            comboBox.SetSolidForegroundColor("White");
            //        else
            //            comboBox.SetSolidForegroundColor("Black");

            //        // TODO: Make this smarter later to not have to hardcode colors - JHE
            //        //int colorValue = backgroundColor.B.ToInt() + backgroundColor.G.ToInt() + backgroundColor.R.ToInt();
            //        //if (colorValue > 382 || backgroundColor.B >= 128 || backgroundColor.G >= 128 || backgroundColor.R >= 128)
            //        //    comboBox.SetSolidForegroundColor("White");
            //        //if (backgroundColor.B < 20 || backgroundColor.G > 200 && backgroundColor.R > 200)   // Yellow
            //        //    comboBox.SetSolidForegroundColor("Black");
            //    }
            //}
            //catch { }
        }

        public static void BindEnum<T>(this ComboBox comboBox, PropertyInfo propertyInfo)
        {
            Type enumType = typeof(T);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum");
            }

            //List<string> values = new List<string>();

            //var fields = from field in enumType.GetFields()
            //             where field.IsLiteral
            //             select field;

            //foreach (FieldInfo field in fields)
            //{
            //    object value = field.GetValue(enumType);
            //    values.Add(value.ToString());
            //}

            //_cbx.ItemsSource = values;
            comboBox.FillEnums<T>();


            if (propertyInfo != null)
            {
                if (comboBox.DataContext != null)
                    comboBox.SetSelectedValue(propertyInfo.GetValue(comboBox.DataContext, null));

                comboBox.SelectionChanged += new SelectionChangedEventHandler(
                    (o, e) =>
                    {
                        if (comboBox.DataContext != null && comboBox.SelectedItem != null)
                            propertyInfo.SetValue(comboBox.DataContext, (T)Enum.Parse(enumType, comboBox.SelectedItem.ToString().RemoveSpaces(), true), null);
                    });
            }
        }

    }
}
