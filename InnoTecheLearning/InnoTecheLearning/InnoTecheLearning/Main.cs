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
            Alert(this, "Main constructor");
            Content = new StackLayout
            {
                VerticalOptions = Alert(LayoutOptions.Start, this, "Content.VerticalOptions"),
                BackgroundColor = Color.White,
                WidthRequest = App.Current.MainPage.Width,
                HeightRequest = Alert(App.Current.MainPage.Height, this, "Content.HeightRequest"),
                Children = {
                 new Label {FontSize = Alert(25, this, "Label1.FontSize"),
                            BackgroundColor = Color.FromUint(4285098345),
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = Alert("CSWCSS eLearning App", this, "Label1.Text")
              }, new Label {HorizontalTextAlignment = Alert(TextAlignment.Center, this, "Label2.TextAlign"),
                            FormattedText = Format((Text)"Developed by the\n",Bold("Innovative Technology Society of CSWCSS"))
                            },
           MainScreenRow(Alert(MainScreenItem(Alert("forum-message-3.png",this,"1"),delegate{}, "Forum" ),this,"2"))
                }
            };
        }
    };
}