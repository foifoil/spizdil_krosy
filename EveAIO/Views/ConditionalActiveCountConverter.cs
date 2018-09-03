namespace EveAIO.Views
{
    using EveAIO.Pocos;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows.Data;

    public class ConditionalActiveCountConverter : IValueConverter
    {
        public ConditionalActiveCountConverter()
        {
            Class7.RIuqtBYzWxthF();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int num = 0;
            ReadOnlyObservableCollection<object> observables = value as ReadOnlyObservableCollection<object>;
            if (observables == null)
            {
                return num;
            }
            List<TaskObject> source = new List<TaskObject>();
            foreach (object obj2 in observables)
            {
                source.Add((TaskObject) obj2);
            }
            return source.Count<TaskObject>(delegate (TaskObject i) {
                if (i.State != TaskObject.StateEnum.running)
                {
                    return (i.State == TaskObject.StateEnum.waiting);
                }
                return true;
            });
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        [Serializable, CompilerGenerated]
        private sealed class <>c
        {
            public static readonly ConditionalActiveCountConverter.<>c <>9;
            public static Func<TaskObject, bool> <>9__0_0;

            static <>c()
            {
                Class7.RIuqtBYzWxthF();
                <>9 = new ConditionalActiveCountConverter.<>c();
            }

            public <>c()
            {
                Class7.RIuqtBYzWxthF();
            }

            internal bool <Convert>b__0_0(TaskObject i)
            {
                if (i.State != TaskObject.StateEnum.running)
                {
                    return (i.State == TaskObject.StateEnum.waiting);
                }
                return true;
            }
        }
    }
}

