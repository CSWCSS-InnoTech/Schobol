using System;
using Xamarin.Forms;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public class VocabBookLabelConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                var s = value as (string Chi, string PoS, string Eng)?;
                return new Label
                {
                    FormattedText = new FormattedString
                    {
                        Spans = {
                            new Span
                            {
                                Text = s.Value.PoS,
                                ForegroundColor = Color.Gray,
                                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                                FontFamily = "Courier New, Georgia, Serif"
                            },
                            new Span {
                                Text = s.Value.Chi + "\n",
                                ForegroundColor = Color.Black,
                                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                FontFamily = "Courier New, Georgia, Serif"
                            }
                        }
                    }
                };
            }
            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
            {
                var s = value as string;
                if (s == null)
                    return value;
                return new string(s.Reverse().ToArray());
            }
        }
}