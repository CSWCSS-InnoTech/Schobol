using System;
using Xamarin.Forms;
using CultureInfo = System.Globalization.CultureInfo;

namespace InnoTecheLearning
{
    partial class Utils
    {
        public class VocabBookLabelConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var s = value as (string Chi, string PoS, string Eng)?;
                return new FormattedString
                {
                    Spans =
                    {
                        new Span
                        {
                            Text = s.Value.Eng,
                            ForegroundColor = Color.Black,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            FontFamily = "Courier New, Georgia, Serif"
                        },
                        new Span
                        {
                            Text = s.Value.PoS,
                            ForegroundColor = Color.Gray,
                            FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                            FontFamily = "Courier New, Georgia, Serif"
                        },
                        new Span {
                            Text = s.Value.Chi,
                            ForegroundColor = Color.Black,
                            FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            FontFamily = "Courier New, Georgia, Serif"
                        }
                    }
                };
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
                throw new NotImplementedException("Not intended to be called.");
        }
    }
}