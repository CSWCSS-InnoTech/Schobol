using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace InnoTecheLearning
{
	public class Main : ContentPage
	{
		public Main ()
		{
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Color.White,
                Children = {
                 new Label {FontSize = 25,
                            BackgroundColor = Color.FromUint(4285098345),
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "CSWCSS eLearning App"
              }, new Label {HorizontalTextAlignment = TextAlignment.Center,
                            Text = "Developed by the"
              }, new Label {HorizontalTextAlignment = TextAlignment.Center,
                            FontAttributes = FontAttributes.Bold,
                            Text = "Innovative Technology Society of CSWCSS" },
           new StackLayout {HorizontalOptions = LayoutOptions.Start,
                Children = { new StackLayout {VerticalOptions = LayoutOptions.Start,
                            Children = { new Button {Image = (FileImageSource)ImageSource.FromFile("forum-message-3.png")} } }} }
                    }
            };
		}
	}
}
