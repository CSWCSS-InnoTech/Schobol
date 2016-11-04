using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static InnoTecheLearning.Utils;
using static InnoTecheLearning.Utils.Create;
using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace InnoTecheLearning
{
    public class Main : ContentPage
    {
        public enum Pages : sbyte
        {
            Changelog = -1,
            Main,
            Forum,
            Translate,
            VocabBook,
            MathConverter,
            MathConverter_Duo,
            Factorizer,
            Sports,
            MusicTuner,
            MathSolver
        }
        Pages _Showing;
        Pages Showing
        {
            get { return _Showing; }
            set
            {
                switch (value)
                {
                    case Pages.Changelog:
                        Content = ChangelogView(this);
                        break;
                    case Pages.Main:
                        Content = MainView;
                        break;
                    case Pages.Forum:
                        break;
                    case Pages.Translate:
                        break;
                    case Pages.VocabBook:
                        break;
                    case Pages.MathConverter:
                        break;
                    case Pages.MathConverter_Duo:
                        break;
                    case Pages.Factorizer:
                        break;
                    case Pages.Sports:
                        break;
                    case Pages.MusicTuner:
                        break;
                    case Pages.MathSolver:
                        break;
                    default:
                        break;
                }
                _Showing = value;
            }
        }
        public Main()
        {
            MainView = _MainView;
            BackgroundColor = Color.White;
            //Alert(this, "Main constructor");
            Showing = Pages.Main;
        }
        protected override bool OnBackButtonPressed()
        {
            if (Showing != Pages.Main)
            {
                Showing = Pages.Main;
                return true;
            }
            else
                return base.OnBackButtonPressed();
        }

        public StackLayout MainView { get; }
        public StackLayout _MainView
        {
            get
            {
                return new StackLayout
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Orientation = StackOrientation.Vertical,
                    Children = {
                        Title("CSWCSS eLearning App"),
                        Society,

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
                         ),

                Button((Text)"Changelog", delegate {Showing = Pages.Changelog; }),
                Create.Version
                    }
                };
            }
        }
        StreamPlayer MusicSound { get; set; }
        public StackLayout MusicTuner
        {
            get
            {
                return new StackLayout
                {
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Orientation = StackOrientation.Vertical,
                    Children = {
                        Title("CSWCSS Music Tuner"),

                        Row(Image(ImageFile.Violin, delegate {Alert(this, "🎻♫♬♩♪♬♩♪♬"); })
                        , (Text)"Violin and Viola")
                    }
                };
            }
        }
    }
}
