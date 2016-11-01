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
                VerticalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = {
                 new Label {FontSize = 25,
                            BackgroundColor = Color.FromUint(4285098345),
                            FontAttributes = FontAttributes.Bold,
                            TextColor = Color.White,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Text = "CSWCSS eLearning App"
              }, new Label {HorizontalTextAlignment = TextAlignment.Center,
                            TextColor = Color.Black,
                            FormattedText = Format((Text)"Developed by the\n",Bold("Innovative Technology Society of CSWCSS"))
                            },
           MainScreenRow(MainScreenItem(Image(ImageFile.Forum),delegate{Alert(this,"[2016-11-1 18:00:00] 1E03: Hi\n"+
               "[2016-11-1 18:00:09] 3F43: No one likes you loser\n[2016-11-1 18:00:16] 1E03: 😢😭😢😭😢😭😢😭😢\n"+
               "[2016-11-1 18:00:22] 2E12: Hey don't bully him!\n[2016-11-1 18:00:28] 3F43: Go kill yourself because you"+
               " are a F-ing faggot\n[2016-11-1 18:00:34] 2E12: I am going to rape you\n"+
               "[2016-11-1 18:00:55] 3F43: "+StrDup("😢😭😢😭😢😭😢😭😢",5)); }, BoldLabel("Forum") ),
                         MainScreenItem(Image(ImageFile.Translate), delegate{Alert(this,
                          "I'm a translator.\nInput: eifj[vguowhfuy9q727969y\nOutput: Gud mornin turists, we spek Inglish"); },
                         BoldLabel("Translator") ),
                         MainScreenItem(Image(ImageFile.VocabBook),delegate {Alert(this,"Ida = 捱打，伸張靜儀、儆惡懲奸，\n" +
"      救死扶傷、伸張靜儀、鋤強扶弱、儆惡懲奸、修身齊家、知足常樂"); },BoldLabel("Vocab Book"))),

           MainScreenRow(MainScreenItem(Image(ImageFile.MathConverter),delegate {
                             Alert(this, "1+1=2"); },BoldLabel("Math Converter")),
                         MainScreenItem(Image(ImageFile.MathConverter_Duo),delegate {
                             Alert(this, StrDup("1+",100) + "1\n=101"); },BoldLabel("Math Converter Duo")),
                         MainScreenItem(Image(ImageFile.Factorizer),delegate {Alert(this,
                             "Factorize 3𝐗²(𝐗−1)²+2𝐗(𝐗−1)³\n = 𝐗(𝐗−1)²(5𝐗−2)"
                             ); },BoldLabel("Quadratic Factorizer"))),

           MainScreenRow(MainScreenItem(Image(ImageFile.Sports), delegate {
                             Alert(this,"🏃🏃🏃長天長跑🏃🏃🏃"); },BoldLabel("Sports")),
                         MainScreenItem(Image(ImageFile.MusicTuner), delegate {
                             Alert(this,"🎼♯♩♪♭♫♬🎜🎝♮🎵🎶\n🎹🎻🎷🎺🎸"); },BoldLabel("Music Tuner")),
                         MainScreenItem(Image(ImageFile.MathSolver), delegate {
                             Alert(this, "🔥🔥🔥🔥🔥🔥🐲🐉"); },BoldLabel("Maths Solver Minigame"))
                         )
                }
            };
        }
    };
}
