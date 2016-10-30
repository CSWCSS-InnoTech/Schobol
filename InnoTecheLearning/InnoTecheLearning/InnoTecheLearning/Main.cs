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
            BackgroundColor = Color.White;
            //Alert(this, "Main constructor");
            Content = new StackLayout
            {
                //VerticalOptions = LayoutOptions.StartAndExpand,
                //Orientation  = StackOrientation.Vertical,
                Children = {
                 new Label {FontSize = 25,
                            BackgroundColor = Color.FromUint(4285098345),
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            //HorizontalTextAlignment = TextAlignment.Center,
                            Text = "CSWCSS eLearning App"
              }, new Label {//HorizontalTextAlignment = TextAlignment.Center,
                            TextColor = Color.Black,
                            //FormattedText = Format((Text)"Developed by the\n",Bold("Innovative Technology Society of CSWCSS"))
                            },
           //MainScreenRow(MainScreenItem("forum-message-3.png",delegate{Alert(this,"Test for button"); }, "Forum" ))
                }
            };
        }
    };
}