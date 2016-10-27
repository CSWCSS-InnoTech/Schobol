using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static InnoTecheLearning.Utils;
using static InnoTecheLearning.Utils.Create;
using Xamarin.Forms;

namespace InnoTecheLearning
{
    public class Main : ContentPage
    {
        public Main()
        {
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Color.White,
                WidthRequest = App.Current.MainPage.Width,
                HeightRequest = App.Current.MainPage.Height,
                Children = {
                 new Label {FontSize = 25,
                            BackgroundColor = Color.FromUint(4285098345),
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "CSWCSS eLearning App"
              }, new Label {HorizontalTextAlignment = TextAlignment.Center,
                            FormattedText = Format((Text)"Developed by the\n",Bold("Innovative Technology Society of CSWCSS"))
                            },
           MainScreenRow(MainScreenItem("forum-message-3.png",delegate{}, "Forum" ))
                }
            };
        }
    };
}